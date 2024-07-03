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
        while (seconds <= 360f)
        {
            Debug.LogError("PhotonNetwork.Time: " + PhotonNetwork.Time);
            ++seconds;
            if (seconds == 30f)
            {
                //쉬는시간
                Debug.LogError("Break Time!");
            }
            if (seconds == 60f)
            {
                seconds = 0f;
                //XRSettings.enabled
                if (!PhotonNetwork.IsMasterClient) 
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
            //!XRSettings.enabled 
            if(PhotonNetwork.IsMasterClient)
            {
                //PC쪽에서 VR쪽으로 정보를 보내준다. 
                photonView.RPC("SetSecondandDay", RpcTarget.OthersBuffered, day, seconds); 
            }
            secondtext.text = "Second: " + seconds;
            daytext.text = "Day: " + day;
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
