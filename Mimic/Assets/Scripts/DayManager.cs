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
    private float seconds_hidden = 0f; //
    private float breaktime = 0f;

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
            //110초 또는 360초: 쉬는시간 시작 
            if (seconds == 100f)
            {
                seconds = 0f;
                StartCoroutine(BreakTimeCoroutine());
                yield break;
            }
            else
            {
                ++seconds;
            }
            if(seconds_hidden == 260f)
            {
                seconds_hidden = 0f;
                if(day < 4)
                {
                    ++day;
                    Debug.LogError("day: " + day); 
                }
            }
            ++seconds_hidden;
            Debug.LogError("seconds_hidden: " + seconds_hidden); 
            //!XRSettings.enabled
            if (PhotonNetwork.IsMasterClient) //PC -> VR
            {
                photonView.RPC("SetSecondandDay", RpcTarget.OthersBuffered, day, seconds, seconds_hidden);
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
        CustomerSpawnManager.instance.BreakTime(); //쉬는시간 시작 
        while (breaktime < 30f)
        {
            ++breaktime;
            ++seconds_hidden;
            Debug.LogError("seconds_hidden: " + seconds_hidden); 
            //!XRSettings.enabled 
            if (PhotonNetwork.IsMasterClient) //PC -> VR 
            {
                photonView.RPC("SetBreakTime", RpcTarget.OthersBuffered, breaktime, seconds_hidden);
            }
            dateuipresenter.SetBreakTime(breaktime);
            yield return new WaitForSeconds(1f);
        }
        breaktime = 0f;
        CustomerSpawnManager.instance.Restart();   //쉬는시간 끝 
        StartCoroutine(AddDayCoroutine()); //시계 시작 
        if (XRSettings.enabled)
        {
            SpawnManager.instance.GoNextWave();
        }
        yield break;
    }
    #endregion


    #region["VR와 PC끼리 동기화"] 
    [PunRPC]
    public void SetSecondandDay(int _day, float _seconds, float _seconds_hidden)
    {
        seconds = _seconds;
        seconds_hidden = _seconds_hidden; 
        day = _day;
    }

    [PunRPC]
    public void SetBreakTime(float _breaktime, float _seconds_hidden)
    {
        breaktime = _breaktime;
        seconds_hidden = _seconds_hidden; 
    }
    #endregion


}
