using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class ReadyRoom : MonoBehaviourPunCallbacks
{
    [Header("�� ����")]
    [SerializeField] private TextMeshProUGUI roomname = null;
    [Header("PC�� �÷��̾� ����")]
    [SerializeField] private TextMeshProUGUI pc_nickname = null;
    [Header("VR�� �÷��̾� ����")]
    [SerializeField] private TextMeshProUGUI vr_nickname = null;
    [Header("���� ��ư")]
    [SerializeField] private Button startbutton = null;
    [Header("PC�� �հ�")]
    [SerializeField] private Image pc_crown = null;
    [Header("VR�� �հ�")]
    [SerializeField] private Image vr_crown = null;
    [Header("PC�� Ready")]
    [SerializeField] private Image pc_ready = null;
    [Header("VR�� Ready")]
    [SerializeField] private Image vr_ready = null;
    [Header("PC�� ������")]
    [SerializeField] private Image pc_astro = null;
    [Header("VR�� ������")]
    [SerializeField] private Image vr_astro = null;

    #region["�ٸ� �÷��̾ ���������� ����Ǵ� �޼ҵ�"] 
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogError("Here!");
        if (XRSettings.enabled)
        {
            //���� �÷��̾ VR => �ٸ� �÷��̾�� PC! 
            pc_nickname.text = "(PC)" + newPlayer.NickName;
            pc_astro.gameObject.SetActive(true); 
        }
        else
        {
            //���� �÷��̾ PC => �ٸ� �÷��̾�� VR! 
            vr_nickname.text = "(VR)" + newPlayer.NickName;
            vr_astro.gameObject.SetActive(true); 
        }
        if(PhotonNetwork.IsMasterClient)
        {
            startbutton.interactable = true;
            startbutton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
        }
    }
    #endregion

    #region["�ٸ� �÷��̾ �������� ����Ǵ� �޼ҵ�"]
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer); 

        if (otherPlayer == PhotonNetwork.MasterClient)
        {
            Debug.LogError("Master Client Exit Game."); 
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            if (XRSettings.enabled)
            {
                pc_nickname.text = "(PC)";
                pc_astro.gameObject.SetActive(false); 
            }
            else
            {
                vr_nickname.text = "(VR)";
                vr_astro.gameObject.SetActive(false); 
            }
            startbutton.interactable = false;
            startbutton.GetComponentInChildren<TextMeshProUGUI>().text = "Ready";
        }
    }
    #endregion


    private void Awake()
    {
        if (PhotonNetwork.IsConnected)
        {
            roomname.text = PhotonNetwork.CurrentRoom.Name;
            if (XRSettings.enabled)
            {
                //VR 
                vr_nickname.text = "(VR)" + PhotonNetwork.LocalPlayer.NickName;
                pc_astro.gameObject.SetActive(false); 
            }
            else
            {
                //PC
                pc_nickname.text = "(PC)" + PhotonNetwork.LocalPlayer.NickName;
                vr_astro.gameObject.SetActive(false); 
            }
            ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            if ((bool)ht["IsMasterPC"])
            {
                Debug.LogError("PC is Roommaster.");
                //PC�� �����϶� 
                pc_crown.gameObject.SetActive(true);
                vr_crown.gameObject.SetActive(false);
                pc_ready.gameObject.SetActive(false);
                vr_ready.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("VR is Rommmaster.");
                //VR�� �����϶� 
                vr_crown.gameObject.SetActive(true);
                pc_crown.gameObject.SetActive(false);
                vr_ready.gameObject.SetActive(false);
                pc_ready.gameObject.SetActive(true);
            }
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                if(XRSettings.enabled)
                {
                    pc_nickname.text = "(PC)" + PhotonNetwork.PlayerListOthers[0].NickName;
                    pc_astro.gameObject.SetActive(true); 
                }
                else
                {
                    vr_nickname.text = "(VR)" + PhotonNetwork.PlayerListOthers[0].NickName;
                    vr_astro.gameObject.SetActive(true); 
                }

                if(PhotonNetwork.IsMasterClient)
                {
                    startbutton.interactable = true;
                    startbutton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
                }
            }
        }

    }

    #region["���� ����!"] 
    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("GoGameScene", RpcTarget.All);
        }
    }
    #endregion

    [PunRPC]
    public void GoGameScene()
    {
        Debug.LogError("Time to Start Game...");
        SceneManager.LoadScene("5_Loading");
    }

    #region["���� ������"]
    public void ExitGame()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region["���� �������� ����Ǵ� �޼ҵ�"] 
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("3_Lobby");
    }
    #endregion

}
