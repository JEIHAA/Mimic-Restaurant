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
    private int day = 1; //1,2,3,4
    private float seconds = 0f;
    private float breaktime = 0f; 
    //실제로는 여기서 직접 내용을 출력하지는 않을꺼임.  
    [SerializeField] private TextMeshProUGUI daytext = null;
    [SerializeField] private TextMeshProUGUI minutetext = null;
    [SerializeField] private TextMeshProUGUI secondtext = null;
    //실제로는 튜토리얼이 다 끝나면 상태값을 true로 바꿈. 

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
            ++seconds;
            if (seconds == 110f || seconds == 360f)
            {
                StartCoroutine(BreakTimeCoroutine()); //쉬는시간 코루틴
            }
            if(XRSettings.enabled)
            {
                //PC쪽에서 VR쪽으로 정보를 보내준다. 
                photonView.RPC("SetSecondandDay", RpcTarget.OthersBuffered, day, seconds); 
            }
            secondtext.text = "Second: " + seconds;
            daytext.text = "Day: " + day;
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
            yield return new WaitForSeconds(1f);
        }
        breaktime = 0f;
        CustomerSpawnManager.instance.Restart();   //쉬는시간 끝 
        if (XRSettings.enabled)
        {
            SpawnManager.instance.GoNextWave();
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
