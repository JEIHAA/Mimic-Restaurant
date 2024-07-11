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

    public delegate void OnAdjustDelegate();
    private OnAdjustDelegate adjustonclick = null;
    public OnAdjustDelegate AdjustOnClick
    {
        set { adjustonclick = value;  }
    }

    private int day = 1; //1,2,3,4,5
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
            ++seconds;  
            if (seconds == 100f)
            {
                StartCoroutine(BreakTimeCoroutine());
            }
            if(seconds >= 224f)
            {
                //이때부터 손님 스폰을 중단한다. 
                CustomerSpawnManager.instance.BreakTime(); 
            }
            if(seconds == 240f)
            {
                if(day < 5)
                {
                    seconds = 0f;
                    StartCoroutine(BreakTimeCoroutine()); 
                    //정산화면 출력 
                    adjustonclick?.Invoke(); 
                    yield break; 
                }
            }
            //!XRSettings.enabled
            if (!XRSettings.enabled) //PC -> VR
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
        if(XRSettings.enabled)
        {
            //쉬는시간이 시작되면 몬스터를 삭제하고 리스트를 초기화한다. 
            SpawnManager.instance.deleteMonster(); 
        }
        while (breaktime < 20f)
        {
            ++breaktime;
            //!XRSettings.enabled 
            if (!XRSettings.enabled) //PC -> VR 
            {
                photonView.RPC("SetBreakTime", RpcTarget.OthersBuffered, breaktime, seconds_hidden);
            }
            dateuipresenter.SetBreakTime(breaktime);
            yield return new WaitForSeconds(1f);
        }
        breaktime = 0f;
        if (XRSettings.enabled && seconds < 240f) 
        {
            //240초가 되면 쉬는시간이 끝나지만 라운드가 끝나기 때문에 정산이 끝날때까지는 몬스터를 스폰하지 않는다. 
            //몬스터 다시 스폰 
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

    public void PlusDay()
    {
        ++day; 
    }
}
