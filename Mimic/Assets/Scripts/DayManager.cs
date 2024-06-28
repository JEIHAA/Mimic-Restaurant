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
    [Header("VR(�ո�ð�) UI Presenter")]
    [SerializeField] private DateUIPresenter vruipresenter = null; 

    #region["���ӸŴ����� �ű濹��"] 
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

    #region["���ʵ� ���ӸŴ����� �ű�°� ������"]  
    public void StartMethod()
    {
        ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.LocalPlayer.CustomProperties;
        ht["IsMainSceneLoaded"] = true; //���� Scene �ε� ���¸� true�� �ٲ۴�. 
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
        //������ 1���� ������ ������ ����ϱ� ���ؼ� �ڷ�ƾ�� ������. 
        StartCoroutine(CheckAllPlayersSceneLoaded());
    }
    #endregion 

    #region["�ð� ���ϴ� �޼ҵ�"] 
    //[PunRPC]
    public void StartTimer() 
    {
        Debug.LogError("All Player's Scene Loaded... Now Start Game...");
        StartCoroutine(AddDayCoroutine());
    }
    #endregion

    #region["��� �÷��̾ �� ���Դ��� �˻�: ��� �÷��̾ �� ���;� Ÿ�̸Ӱ� ���ư���."] 
    private IEnumerator CheckAllPlayersSceneLoaded()
    {
        while (true)
        {
            Debug.LogError("Other Player's Name: " + PhotonNetwork.PlayerListOthers[0].NickName);
            //�ٸ� �÷��̾��� Scene Load���°� true�϶� => �ٸ� �÷��̾��� Start �޼ҵ尡 ����Ǿ��ٴ� ����. 
            if ((bool)PhotonNetwork.PlayerListOthers[0].CustomProperties["IsMainSceneLoaded"])
            {
                /*
                 * �����϶��� ��� �÷��̾�� StartGame �޼ҵ带 �����Ѵ�.
                 * => ������ �׻� �����ϸ�, ���� �÷��̾� �ʿ��� �� �޼ҵ尡 ����Ǹ� ��� �÷��̾�� StartGame �޼ҵ尡 �����ϰ� �ȴ�.
                 *    �ٸ� �÷��̾���� IF�� ���� ������ ���� ��ü�� �ȵǾ� �޼ҵ尡 �ߺ� ����Ǵ� ���� ����. 
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



    #region["��¥ ���ϴ� �ڷ�ƾ"] 
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
                        //�մ� ���� ���̺� 
                    }
                    ++day;
                }
            }
            if (seconds == 30f)
            {
                //���½ð�
                Debug.LogError("Break Time!");
            }
            //!XRSettings.enabled 
            if(PhotonNetwork.IsMasterClient)
            {
                //PC�ʿ��� VR������ ������ �����ش�. 
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
