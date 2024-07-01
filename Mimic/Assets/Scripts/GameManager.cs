using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//2024-05-22: CUSTOM UNITY TEMPLATE 
/*
 2024-06-20 �ۼ��� : ���� 
 ���� ���� : 
*/
public class GameManager : MonoBehaviourPun 
{
    [Header("���� �Ŵ���")]
    [SerializeField] private MonsterManager monstermanager = null;
    [Header("���� �Ŵ���")]
    [SerializeField] private SpawnManager spawnmanager = null;
    [Header("��¥ �Ŵ���")]
    [SerializeField] private DayManager daymanager = null;
    [Header("VR �÷��̾�")]
    [SerializeField] private Transform vrplayer_transform = null;
    [Header("PC �÷��̾�")]
    [SerializeField] private Transform pcplayer_transform = null;

    [Header("���� ���� ����")] 
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

    #region["�������� �ٷ� ����"] 
    private void CheckinMainScene()
    {
        ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.LocalPlayer.CustomProperties;
        ht["IsMainSceneLoaded"] = true; //���� Scene �ε� ���¸� true�� �ٲ۴�. 
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
        //������ 1���� ������ ������ ����ϱ� ���ؼ� �ڷ�ƾ�� ������. 
        StartCoroutine(CheckAllPlayersSceneLoaded());
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

    #region["PC �ʿ��� ����Ǿ� �ϴ°�"]
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
