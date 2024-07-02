using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("게임 시작 상태")] 
    [SerializeField] private bool isGameStarted = false;
    #region["Awake is called when enable scriptable instance is loaded."] 
    private void Awake()
    {
        if(XRSettings.enabled)
        {
            //VR 
            pcplayer_transform.gameObject.SetActive(false); 
        }
        else
        {
            //PC 
            vrplayer_transform.gameObject.SetActive(false);
            pcplayer_transform.GetComponentInChildren<Camera>().targetDisplay = 0; 
        }
    }
    #endregion

    
    #region["Start is called before the first frame update"] 
    private void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            CheckinMainScene();
        }
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
                /*
                 * 방장일때만 모든 플레이어에서 StartGame 메소드를 실행한다.
                 * => 방장은 항상 존재하며, 방장 플레이어 쪽에서 이 메소드가 실행되면 모든 플레이어에서 StartGame 메소드가 실행하게 된다.
                 *    다른 플레이어에서는 IF문 조건 때문에 실행 자체가 안되어 메소드가 중복 실행되는 일이 없다. 
                */
                /*
                if(PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("StartTimer", RpcTarget.All);
                }
                */
                isGameStarted = true; 
                //daymanager.StartTimer();
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion



    #region["Update is called once per frame"] 
    private void Update()
    {
        if(XRSettings.enabled && isGameStarted)
        {
            monstermanager?.MoveAll(vrplayer_transform);
        }
    }
    #endregion

    #region["PC 쪽에서 실행되야 하는거"]
    [PunRPC]
    public void MonsterArrivedToRestaurant()
    {
        if(!XRSettings.enabled)
        {
            CustomerSpawnManager.instance.IsMonsterArrivedToRestaurant(); 
        }
    }
    #endregion

}
