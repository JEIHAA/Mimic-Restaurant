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
    [Header("PC쪽 아이콘")]
    [SerializeField] private Image pc_astro = null;
    [Header("VR쪽 아이콘")]
    [SerializeField] private Image vr_astro = null;
    [Header("스토리 UI")]
    [SerializeField] private GameObject story_ui = null;
    [Header("스토리 버튼 이미지")]
    [SerializeField] private Image story_btn_img = null;

    private GameObject story_ui_instantiated = null; 

    #region["다른 플레이어가 입장했을때 실행되는 메소드"] 
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogError("Here!");
        if (XRSettings.enabled)
        {
            //현재 플레이어가 VR => 다른 플레이어는 PC! 
            pc_nickname.text = "(PC)" + newPlayer.NickName;
            pc_astro.gameObject.SetActive(true); 
        }
        else
        {
            //현재 플레이어가 PC => 다른 플레이어는 VR! 
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
        StartCoroutine(StoryUIBtnGIFCoroutine()); 
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
                //PC가 방장일때 
                pc_crown.gameObject.SetActive(true);
                vr_crown.gameObject.SetActive(false);
                pc_ready.gameObject.SetActive(false);
                vr_ready.gameObject.SetActive(true);
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

    #region["게임 시작!"] 
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


    #region["스토리 UI"]
    public void GoStoryUI()
    {
        if(story_ui_instantiated == null)
        {
            story_ui_instantiated = Instantiate(story_ui);
            story_ui_instantiated.GetComponentsInChildren<Button>()[0].onClick.AddListener(CloseStoryUI); 
            gameObject.SetActive(false); 
        }
    }
    #endregion

    #region["스토리 UI 닫기"] 
    public void CloseStoryUI()
    {
        Destroy(story_ui_instantiated);
        gameObject.SetActive(true); 
    }
    #endregion

    #region["스토리 UI 실행 버튼 GIF 코루틴"]
    public IEnumerator StoryUIBtnGIFCoroutine()
    {
        int count = 1; 
        while(true)
        {
            if(count > 2)
            {
                count = 1; 
            }
            story_btn_img.sprite = Resources.Load<Sprite>("Images\\Icons\\StoryBookGIF\\" + count);
            yield return new WaitForEndOfFrame(); 
            ++count; 
        }
    }
    #endregion 

}
