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

    //�����δ� ���⼭ ���� ������ ��������� ��������.  
    [SerializeField] private TextMeshProUGUI daytext = null;
    [SerializeField] private TextMeshProUGUI minutetext = null;
    [SerializeField] private TextMeshProUGUI secondtext = null;
    //�����δ� Ʃ�丮���� �� ������ ���°��� true�� �ٲ�. 

    #region["�ð� ���ϴ� �޼ҵ�"] 
    public void StartTimer() 
    {
        Debug.LogError("All Player's Scene Loaded... Now Start Game...");
        StartCoroutine(AddDayCoroutine());
    }
    #endregion

    #region["��¥ ���ϴ� �ڷ�ƾ"] 
    private IEnumerator AddDayCoroutine()
    {
        while (seconds <= 360f)
        {
            Debug.LogError("PhotonNetwork.Time: " + PhotonNetwork.Time);
            ++seconds;
            if (seconds == 30f)
            {
                //���½ð�
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
                    //�մ� ���� ���̺� 
                }
                ++day;
            }
            //!XRSettings.enabled 
            if(PhotonNetwork.IsMasterClient)
            {
                //PC�ʿ��� VR������ ������ �����ش�. 
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
