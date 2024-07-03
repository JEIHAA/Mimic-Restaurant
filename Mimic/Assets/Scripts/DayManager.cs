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
    public void StartTimer() 
    {
        Debug.LogError("All Player's Scene Loaded... Now Start Game...");
        StartCoroutine(AddDayCoroutine());
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

}
