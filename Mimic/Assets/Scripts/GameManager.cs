using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

//2024-05-22: CUSTOM UNITY TEMPLATE 
/*
 2024-06-20 작성자 : 고영석 
 수정 내용 : 
*/
public class GameManager : MonoBehaviourPun
{
    [Header("몬스터 매니저")]
    [SerializeField] private MonsterManager monstermanager = null;
    [Header("스폰 매니저")]
    [SerializeField] private SpawnManager spawnmanager = null;
    [Header("날짜 매니저")]
    [SerializeField] private DayManager daymanager = null;
    [Header("VR 플레이어")]
    [SerializeField] private Transform vrplayer_transform = null;
    [Header("PC 플레이어")]
    [SerializeField] private Transform pcplayer_transform = null;
    [Header("PC UI")]
    [SerializeField] private PCUIManager pcui = null;
    [Header("VR 정산 UI")]
    [SerializeField] private AdjustUIManager vradjustui = null;

    [Header("게임 시작 상태")]
    [SerializeField] private bool isGameStarted = false;

    [Header("PC 게임 종료 안내 UI")]
    [SerializeField] private GameObject pcexit_ui = null;
    [Header("VR한테 알려주기")]
    [SerializeField] private GameObject vrexit_ui = null;
    private GameObject pcexit_ui_instantiate = null;
    [SerializeField] private GameObject vrexit_ui_instantiate = null;


    #region["Awake is called when enable scriptable instance is loaded."] 
    private void Awake()
    {
        if (XRSettings.enabled)
        {
            //VR 
            pcplayer_transform.gameObject.SetActive(false);
        }
        else
        {
            //PC 
            vrplayer_transform.gameObject.SetActive(false);
            pcplayer_transform.GetComponentInChildren<Camera>().targetDisplay = 0;
            vrexit_ui_instantiate = null;
        }
        vradjustui.gameObject.SetActive(false);
        if (vrexit_ui_instantiate != null)
        {
            vrexit_ui_instantiate.SetActive(false);
        }
        daymanager.AdjustOnClick = AdjustOnClick;
    }
    #endregion


    #region["Start is called before the first frame update"] 
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            CheckinMainScene();
        }
        else
        {
            CustomerSpawnManager.instance.StartSpawnCustomer(); //임시(네트워크X) 
            daymanager.StartTimer(); //임시(네트워크X) 
        }
        AudioManager.instance.PlayBGM();
    }
    #endregion

    #region["들어왔을때 바로 실행"] 
    private void CheckinMainScene()
    {
        ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.LocalPlayer.CustomProperties;
        ht["IsMainSceneLoaded"] = true; //메인 Scene 로드 상태를 true로 바꾼다. 
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
        //나머지 1명이 들어오기 전까지 대기하기 위해서 코루틴을 돌린다. 
        StartCoroutine(CheckAllPlayersSceneLoaded());
    }
    #endregion

    #region["모든 플레이어가 다 들어왔는지 검사: 모든 플레이어가 다 들어와야 타이머가 돌아간다."] 
    private IEnumerator CheckAllPlayersSceneLoaded()
    {
        while (true)
        {
            Debug.LogError("Other Player's Name: " + PhotonNetwork.PlayerListOthers[0].NickName);
            //다른 플레이어의 Scene Load상태가 true일때 => 다른 플레이어의 Start 메소드가 실행되었다는 뜻임. 
            if ((bool)PhotonNetwork.PlayerListOthers[0].CustomProperties["IsMainSceneLoaded"])
            {
                //만약 다른 플레이어가 이미 들어와있는데 누가 나가서 게임이 멈췄을 경우 그 플레이어의 게임을 다시 시작시킴. 
                photonView.RPC("RestartGameOtherSide", RpcTarget.Others);
                isGameStarted = true;
                daymanager.StartTimer();
                if (!XRSettings.enabled)
                {
                    CustomerSpawnManager.instance.StartSpawnCustomer();
                }
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion



    #region["Update is called once per frame"] 
    private void Update()
    {
        if (XRSettings.enabled && isGameStarted)
        {
            monstermanager?.MoveAll(vrplayer_transform);
        }
        //monstermanager?.MoveAll(vrplayer_transform); 
        if (Input.GetKey(KeyCode.Escape)) //PC용 => 종료 
        {
            if (pcexit_ui_instantiate == null)
            {
                pcexit_ui_instantiate = Instantiate(pcexit_ui);
                pcexit_ui_instantiate.GetComponent<MessageUI>().SetText(2);
                pcexit_ui_instantiate.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => ExitGame(true));
            }
        }
    }
    #endregion

    #region["현재 플레이어가 나가면 다른 플레이어는 게임이 멈춤."]
    [PunRPC]
    public void STX_BSDC1()
    {
        Time.timeScale = 0f;
        if (vrexit_ui_instantiate == null)
        {
            //PC -> VR 
            vrexit_ui_instantiate = Instantiate(vrexit_ui);
            vrexit_ui_instantiate.GetComponent<MessageUI>().SetText(1);
        }
        else
        {
            //VR -> PC 
            vrexit_ui_instantiate.SetActive(true);
            vrexit_ui_instantiate.GetComponent<MessageUI>().SetText(2);
        }
        vrexit_ui_instantiate.GetComponentsInChildren<Button>()[0].onClick.RemoveAllListeners();
        vrexit_ui_instantiate.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => ExitGame(false));
    }
    #endregion

    #region["게임 종료"] 
    public void ExitGame(bool _isyou)
    {
        if (PhotonNetwork.IsConnected)
        {
            if (_isyou)
            {
                photonView.RPC("STX_BSDC1", RpcTarget.Others);
            }
            PhotonNetwork.LeaveRoom();
        }
        Application.Quit();
    }
    #endregion


    #region["새로운 플레이어가 들어오면 게임을 다시 시작"]
    [PunRPC]
    public void RestartGameOtherSide()
    {
        Time.timeScale = 1f;
    }
    #endregion 


    public void AdjustOnClick()
    {
        //정산 UI 출력
        if (!XRSettings.enabled)
        {
            pcui.AdjustUI();
        }
        else
        {
            vradjustui.gameObject.SetActive(true);
            vradjustui.GetComponent<AdjustUIManager>().RunAdjustUI();
        }
    }
}
