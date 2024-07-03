using JetBrains.Annotations;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;


public class DayManager : MonoBehaviourPun
{
    [SerializeField] private DateUIPresenter dateuipresenter = null;

    private int day = 1; //1,2,3,4
    private float seconds = 0f;
    private float breaktime = 0f;

    private bool isBreakTime = false;

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
        while (true)
        {
            Debug.LogError("PhotonNetwork.Time: " + PhotonNetwork.Time);
            if (seconds == 110f || seconds == 360f)
            {
                isBreakTime = true; 
                StartCoroutine(BreakTimeCoroutine()); //쉬는시간 코루틴
                if(!isBreakTime) 
                {
                    ++seconds; 
                }
            }
            else
            {
                ++seconds;
            }
            if(!XRSettings.enabled)
            {
                //PC쪽에서 VR쪽으로 정보를 보내준다. 
                photonView.RPC("SetSecondandDay", RpcTarget.OthersBuffered, day, seconds); 
            }
            dateuipresenter.SetSecond(seconds);
            dateuipresenter.SetDay(day);
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

    #region["쉬는시간 코루틴"] 
    private IEnumerator BreakTimeCoroutine()
    {
        if(seconds == 360f) 
        {
            //1라운드가 끝나면 1일 올린다. 
            ++day; 
        }
        if(day == 4)
        {
            //게임 끝? 
        }
        seconds = 0f;
        CustomerSpawnManager.instance.BreakTime(); //쉬는시간 시작 
        while (breaktime < 20f)
        {
            ++breaktime;
            dateuipresenter.SetBreakTime(breaktime); 
            yield return new WaitForSeconds(1f);
        }
        breaktime = 0f;
        isBreakTime = false; 
        CustomerSpawnManager.instance.Restart();   //쉬는시간 끝 
        if (XRSettings.enabled)
        {
            SpawnManager.instance.GoNextWave();
        }
        yield break; 
    }
    #endregion

    #region["VR와 PC끼리 동기화"] 
    [PunRPC]
    public void SetSecondandDay(int _day, float _seconds, float _breaktime)
    {
        seconds = _seconds;
        day = _day;
        breaktime = _breaktime; 
    }
    #endregion

}
