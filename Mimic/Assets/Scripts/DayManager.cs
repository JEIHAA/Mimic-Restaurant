using JetBrains.Annotations;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;


public class DayManager : MonoBehaviourPun
{
    private int day = 1; //1,2,3,4,5,6 
    private float seconds = 0f;

    [Header("PC UI Presenter")]
    [SerializeField] private DateUIPresenter pcuipresenter = null;
    [Header("VR(손목시계) UI Presenter")]
    [SerializeField] private DateUIPresenter vruipresenter = null; 

    #region["게임매니저로 옮길예정"] 
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            StartMethod(); 
        }
        else
        {
            /*
            string scenename = "2_Login_StartGame"; 
            if (XRSettings.enabled)
            {
                scenename += "_VR";
            }
            SceneManager.LoadScene(scenename);
            */
        }
    }
    #endregion

    #region["이쪽도 게임매니저로 옮기는게 좋겠음"]  
    public void StartMethod()
    {
        ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.LocalPlayer.CustomProperties;
        ht["IsMainSceneLoaded"] = true; //메인 Scene 로드 상태를 true로 바꾼다. 
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
        //나머지 1명이 들어오기 전까지 대기하기 위해서 코루틴을 돌린다. 
        StartCoroutine(CheckAllPlayersSceneLoaded());
    }
    #endregion 

    #region["시간 더하는 메소드"] 
    //[PunRPC]
    public void StartTimer() 
    {
        Debug.LogError("All Player's Scene Loaded... Now Start Game...");
        StartCoroutine(AddDayCoroutine());
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
                StartTimer(); 
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion



    #region["날짜 더하는 코루틴"] 
    private IEnumerator AddDayCoroutine()
    {
        while (seconds <= 360f)
        {
            Debug.LogError("PhotonNetwork.Time: " + PhotonNetwork.Time);
            ++seconds;
            if(day < 6f)
            {
                if (seconds == 60f)
                {
                    seconds = 0f;
                    if (XRSettings.enabled) 
                    {
                        SpawnManager.instance.GoNextWave();
                    }
                    else
                    {
                        Debug.LogError("Client Wave...");
                        //손님 다음 웨이브 
                    }
                    ++day;
                }
            }
            if (seconds == 30f)
            {
                //쉬는시간
                Debug.LogError("Break Time!");
            }
            //!XRSettings.enabled 
            if(PhotonNetwork.IsMasterClient)
            {
                //PC쪽에서 VR쪽으로 정보를 보내준다. 
                photonView.RPC("SetSecondandDay", RpcTarget.OthersBuffered, day, seconds); 
            }
            //secondtext.text = "Second: " + seconds;
            if(XRSettings.enabled)
            {
                //vruipresenter.SetDay(day);
                //vruipresenter.SetSecond(seconds); 
            }
            else
            {
                pcuipresenter.SetDay(day); 
            }
            yield return new WaitForSeconds(1f);
        }
        yield break;
    }
    #endregion 

    [PunRPC]
    public void SetSecondandDay(int _day, float _seconds)
    {
        seconds = _seconds;
        day = _day; 
    }

    public float GetSeconds()
    {
        return seconds; 
    }

    public int GetDay()
    {
        return day; 
    }
}
