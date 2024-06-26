using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Drawing;
using System.Collections;
using UnityEngine.XR;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("�� �̸� �Է�â")]
    [SerializeField] private GameObject makeRoomPanel; // �� �̸� �Է�â�� ������ �г�
    [Header("�� �̸� �Է� �ؽ�Ʈ ����")]
    [SerializeField] private TMP_InputField roomNameInputField = null;
    [Header("�� ��ư")]
    [SerializeField] private GameObject roomBtn = null;
    [Header("�� �̸�")]
    [SerializeField] private TextMeshProUGUI roomBtn_text = null;
    [Header("PC �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI pctext_text = null;
    [Header("VR �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI vrtext_text = null;
    [Header("���� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI statustext = null;
    [Header("�� ����� ��ư")]
    [SerializeField] private Button makeroombtn = null;


    [SerializeField] private string gameVersion; //���� ����

    private string nickname_player = string.Empty; //�÷��̾� �г��� 

    private string roomname = string.Empty;

    private void Awake()
    {
        makeRoomPanel.SetActive(false);
        InitRoomSettings();
        //Singleton���� ���̵� �������� 
        if (!string.IsNullOrEmpty(WelcomeController.instance.getId()))
        {
            nickname_player = WelcomeController.instance.getId();
            Debug.LogError("nickname_player: " + nickname_player);
            //Destroy(WelcomeController.instance.gameObject); 
        }
        else
        {
            SceneManager.LoadScene("2_Login_StartGame");
        }
    }

    private void InitRoomSettings()
    {
        roomBtn.SetActive(false);
    }

    private void Start()
    {
        // ���� ������ ����
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // ������ ������ ����Ǹ� �κ� ����
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // �κ� �����ϸ� �� ����Ʈ ��û
        PhotonNetwork.GetCustomRoomList(TypedLobby.Default, "");
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roominfo in roomList)
        {
            //��ó���� �������� 
            if (string.IsNullOrEmpty(roomname))
            {
                makeroombtn.interactable = false;
                roomname = roominfo.Name;
                Debug.Log("Room Name: " + roomname);
                roomBtn.SetActive(true);
                roomBtn.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                roomBtn.GetComponentInChildren<Button>().onClick.AddListener(() => JoinRoomBtn(roomname));
                roomBtn_text.text = roomname; //�� �̸� ����
                if (XRSettings.enabled)
                {
                    vrtext_text.text = "1/1";
                }
                else
                {
                    pctext_text.text = "1/1";
                }
                makeroombtn.interactable = false;
            }
            else
            {
                switch(roominfo.PlayerCount)
                {
                    case 0: //�濡 �ƹ��� ������ 
                        makeroombtn.interactable = true;
                        roomBtn.SetActive(false);
                        roomBtn.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                        break;
                    case 1: //1�� �������� 
                        //���� �������� ����: �ݴ����� �ٲ��ش�. 
                        if (XRSettings.enabled)
                        {
                            pctext_text.text = "0/1";
                        }
                        else
                        {
                            vrtext_text.text = "0/1";
                        }
                        statustext.text = "Ready"; 
                        break;
                    case 2:
                        //�� �������� ����: �ݴ����� 1�� �ø���. 
                        if (XRSettings.enabled)
                        {
                            pctext_text.text = "1/1";
                        }
                        else
                        {
                            vrtext_text.text = "1/1";
                        }
                        statustext.text = "Playing";
                        break; 
                }
            }
        }
    }


    private void JoinRoom(string roomName)
    {
        // �濡 ���� ���� Ȯ�� �˸�â ǥ��
        ConfirmJoinRoom(roomName);
    }

    private void ConfirmJoinRoom(string roomName)
    {
        // Ȯ�� �˸�â ǥ�� �� Ȯ�� ��ư Ŭ�� �� �� ����
        bool confirmed = true; // �����δ� Ȯ�� �˸�â�� ����� �޾ƾ� ��

        if (confirmed)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public void OpenMakeRoomPanel()
    {
        // �� �̸� �Է�â �г� ǥ��
        makeRoomPanel.SetActive(true);
    }

    public void CloseMakeRoomPanel()
    {
        // �� �̸� �Է�â �ݱ�
        makeRoomPanel.SetActive(false);
    }

    public void CreateRoom()
    {
        string roomName = roomNameInputField.text;

        // �� �̸��� ����ִٸ� ���� �޽��� ��� �� ����
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogError("Room name is empty!");
            return;
        }

        // �� �ɼ� ���� �� �� ����
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };
        //roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "VR", true }, { "PC", true } }; // ���� Ŀ���� �Ӽ�
        roomOptions.PlayerTtl = 0;
        roomOptions.EmptyRoomTtl = 0;
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        //���� ���� ����� ������. 
        if (XRSettings.enabled)
        {
            //VR�� �����϶� 
            ht.Add("IsMasterPC", false);
        }
        else
        {
            //PC�� �����϶� 
            ht.Add("IsMasterPC", true);
        }
        roomOptions.CustomRoomProperties = ht;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        PhotonNetwork.NickName = nickname_player;

        Debug.Log("Room Created");
        Debug.Log("Room Name: " + roomName);
    }


    public override void OnJoinedRoom()
    {
        // �濡 ������ �� ���� ������ ��ȯ
        Debug.Log("MasterClient join this room.");
        string scenename = "4_Room"; 
        if(XRSettings.enabled)
        {
            scenename += "_VR"; 
        }
        SceneManager.LoadScene(scenename); 
    }

    #region["�̹� ������� �濡 ����"] 
    public void JoinRoomBtn(string _roomname)
    {
        PhotonNetwork.JoinRoom(_roomname);
        PhotonNetwork.NickName = nickname_player;
    }
    #endregion

}
