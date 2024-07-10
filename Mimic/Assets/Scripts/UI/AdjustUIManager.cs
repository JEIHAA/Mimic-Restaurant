using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class AdjustUIManager : MonoBehaviourPun
{
    [Header("GIF 이미지: 운석과 행성")]
    [SerializeField] private Image meteor = null;
    [SerializeField] private Image planet = null;

    [Header("PC 전용 버튼: VR에서는 비활성화")]
    [SerializeField] private Button[] pcbtn = null;

    [Header("PC 상점 UI")]
    [SerializeField] private GameObject pcstore = null;

    private int level = 1; //조건 레벨

    [Header("다음 라운드로 넘어갈 수 있는 최소 손님 수")]
    [SerializeField] private int min_customer = 10;
    [Header("다음 라운드로 넘어갈 수 있는 최소 돈")]
    [SerializeField] private int[] min_money = { 5000, 10000, 20000, 40000, 60000 };

    [Header("날짜 매니저")]
    [SerializeField] private DayManager daymanager = null;
    [Header("스폰 매니저")]
    [SerializeField] private SpawnManager spawnmanager = null;
    [Header("손님 스폰 매니저")]
    [SerializeField] private CustomerSpawnManager customerspawnmanager = null;
    [Header("고기 매니저")]
    [SerializeField] private MeatManager meatmanager = null;
    [Header("몬스터 매니저")]
    [SerializeField] private MonsterManager monstermanager = null;
    [Header("VR 정산 UI")]
    [SerializeField] private AdjustUIManager vradjustui = null; 

    [Header("받은 손님 텍스트")]
    [SerializeField] private TextMeshProUGUI customer_text = null;
    [Header("받지 못한 손님 텍스트")]
    [SerializeField] private TextMeshProUGUI customer_notget_text = null;
    [Header("번 돈 텍스트")]
    [SerializeField] private TextMeshProUGUI money_text = null;
    [Header("죽인 몬스터 텍스트")]
    [SerializeField] private TextMeshProUGUI monster_text = null;
    [Header("고기 텍스트")]
    [SerializeField] private TextMeshProUGUI meat_text = null;

    [Header("다음 날짜로 이동 안내 메시지")]
    [SerializeField] private GameObject nextday_ui = null;
    private GameObject nextday_ui_instantiate = null;

    //PC 
    private int currentcustomer = 0;
    private int currentcustomer_notgetted = 0;
    private int currentmoney = 0;

    //VR
    private int currentmonster = 0;
    private int currentmeat = 0;

    //유니티 생명 주기: Awake -> OnEnable -> Start 
    #region["활성화될때 실행"] 
    private void OnEnable()
    {
        StartCoroutine(GIFCoroutine1());
        StartCoroutine(GIFCoroutine2()); 
        if(XRSettings.enabled)
        {
            //VR에서는 버튼 비활성화 
            foreach(Button btn in pcbtn)
            {
                btn.gameObject.SetActive(false); 
            }
            //VR
            if(PhotonNetwork.IsConnected)
            {
                photonView.RPC("SetTextVR", RpcTarget.All);
            }
        }
        else
        {
            //PC 
            if(PhotonNetwork.IsConnected)
            {
                photonView.RPC("SetTextPC", RpcTarget.All);
            }
        }
    }
    #endregion

    #region["PC 쪽 텍스트 설정"] 
    [PunRPC]
    public void SetTextPC()
    {
        currentcustomer = customerspawnmanager.GetCustomerNum();
        currentcustomer_notgetted = customerspawnmanager.NotGetCustomerNum();
        currentmoney = customerspawnmanager.GetMoney();
        customer_text.text = "받은 손님:" + currentcustomer + "명";
        customer_notget_text.text = "받지 못한 손님:" + currentcustomer_notgetted + "명";
        money_text.text = "번 돈:" + currentmoney.ToString();
    }
    #endregion

    #region["VR 쪽 텍스트 설정"] 
    [PunRPC]
    public void SetTextVR()
    {
        currentmonster = monstermanager.GetMonster_Killed(); 
        currentmeat = meatmanager.GetMeatNumAcummlated();
        monster_text.text = "몬스터 잡은 수: " + currentmonster + "마리";
        meat_text.text = "고기 수:" + currentmeat + "개"; 
    }
    #endregion

    #region["비활성화될때 실행"] 
    private void OnDisable()
    {
        StopCoroutine(GIFCoroutine1());
        StopCoroutine(GIFCoroutine2()); 
    }
    #endregion

    private IEnumerator GIFCoroutine1() //Meteor 
    {
        int count = 1; 
        while(true)
        {
            if(count > 48)
            {
                count = 1; 
            }
            meteor.sprite = Resources.Load<Sprite>("Images\\Sprites\\Meteor_GIF\\" + count);
            yield return new WaitForSeconds(0.5f);  
            ++count; 
        }
    }

    private IEnumerator GIFCoroutine2() //Planet
    {
        int count = 1;
        while(true)
        {
            if(count > 15)
            {
                count = 1; 
            }
            planet.sprite = Resources.Load<Sprite>("Images\\Sprites\\Planet_GIF\\" + count);
            yield return new WaitForSeconds(0.1f); 
            ++count;
        }
    }

    #region["PC 상점 버튼"] 
    public void StoreBtn()
    {
        gameObject.SetActive(false);
        pcstore.SetActive(true);     
    }
    #endregion

    #region["정산하기"]
    public void Adjust()
    {
        Boolean success = currentcustomer >= min_customer && currentmoney >= min_money[level - 1]; //성공 조건 
        if(nextday_ui_instantiate == null)
        {
            nextday_ui_instantiate = Instantiate(nextday_ui);
            nextday_ui_instantiate.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => NextRoundBtn(success)); 
        }
    }
    #endregion

    public void NextRoundBtn(Boolean _success)
    {
        photonView.RPC("NextRound", RpcTarget.All, _success); 
    }

    #region["정산이 끝나고 다음 라운드로 이동"] 
    [PunRPC]
    public void NextRound(Boolean _success)
    {
        if(_success)
        {
            //조건 만족 
            ++level;
            min_customer += 10; 
            spawnmanager.GoNextWave();
            daymanager.PlusDay(); 
        }
        else
        {
            //조건 달성 실패
            spawnmanager.InitMonster(); //웨이브를 올리지 않고 몬스터 스폰함. 
        }
        customerspawnmanager.Restart(); //손님 스폰 다시 시작 
        customerspawnmanager.ClearCustomer(); //PC 정산 정보 초기화
                                              //VR 정산 정보 초기화 
        monstermanager.ClearMonster();
        meatmanager.ClearMeatAcummlated();
        daymanager.StartTimer();
        gameObject.SetActive(false);
        if (vradjustui != null)
        {
            vradjustui.gameObject.SetActive(false);
        }
        Destroy(nextday_ui_instantiate); 
    }
    #endregion
}
