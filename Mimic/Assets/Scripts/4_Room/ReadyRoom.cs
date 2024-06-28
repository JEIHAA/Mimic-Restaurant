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
    [Header("방 제목")]
    [SerializeField] private TextMeshProUGUI roomname = null;
    [Header("PC쪽 플레이어 별명")]
    [SerializeField] private TextMeshProUGUI pc_nickname = null;
    [Header("VR쪽 플레이어 별명")]
    [SerializeField] private TextMeshProUGUI vr_nickname = null;
    [Header("시작 버튼")]
    [SerializeField] private Button startbutton = null;
    [Header("PC쪽 왕관")]
    [SerializeField] private Image pc_crown = null;
    [Header("VR쪽 왕관")]
    [SerializeField] private Image vr_crown = null;
    [Header("PC쪽 Ready")]
    [SerializeField] private Image pc_ready = null;
    [Header("VR쪽 Ready")]
    [SerializeField] private Image vr_ready = null;

    #region["다른 플레이어가 입장했을때 실행되는 메소드"] 
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogError("Here!");
        if (XRSettings.enabled)
        {
            //현재 플레이어가 VR => 다른 플레이어는 PC! 
            pc_nickname.text = "(PC)" + newPlayer.NickName;
        }
        else
        {
            //현재 플레이어가 PC => 다른 플레이어는 VR! 
            vr_nickname.text = "(VR)" + newPlayer.NickName;
        }
        startbutton.interactable = true;
        startbutton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
    }
    #endregion

    #region["다른 플레이어가 나갔을때 실행되는 메소드"]
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
            }
            else
            {
                vr_nickname.text = "(VR)";
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
            }
            else
            {
                //PC
                pc_nickname.text = "(PC)" + PhotonNetwork.LocalPlayer.NickName;
            }
            ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            if ((bool)ht["IsMasterPC"])
            {
                Debug.LogError("PC is Roommaster.");
                //PC가 방장일때 
                pc_crown.gameObject.SetActive(true);
                vr_crown.gameObject.SetActive(false);
                pc_ready.gameObject.SetActive(false);
                vr_crown.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("VR is Rommmaster.");
                //VR가 방장일때 
                vr_crown.gameObject.SetActive(true);
                pc_crown.gameObject.SetActive(false);
                vr_ready.gameObject.SetActive(false);
                pc_ready.gameObject.SetActive(true);
            }
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                startbutton.interactable = true;
                startbutton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
            }
        }

    }

    #region["게임 시작!"] 
    public void StartGame()
    {
        Debug.LogError("Time to Start Game...");
        SceneManager.LoadScene("5_Loading");
    }
    #endregion

    #region["게임 나가기"]
    public void ExitGame()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region["방을 나갔을때 실행되는 메소드"] 
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("3_Lobby");
    }
    #endregion

}
