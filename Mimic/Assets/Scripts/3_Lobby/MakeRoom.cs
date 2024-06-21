using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject roomListPanel; // �� ����Ʈ�� ǥ���� UI �г�
    [SerializeField] private GameObject roomListItemPrefab; // �� ����Ʈ �������� ������
    [SerializeField] private GameObject makeRoomPanel; // �� �̸� �Է�â�� ������ �г�
    [SerializeField] private TMP_InputField roomNameInputField; // �� �̸��� �Է¹��� InputField
    [SerializeField] private Button confirmRoomButton; // �� ���� Ȯ�� ��ư 
    [SerializeField] private string gameVersion; //���� ����


    private void Awake()
    {
        makeRoomPanel.SetActive(false);
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

    /*

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // ���� �� ����Ʈ ������ ����
        foreach (Transform child in roomListPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // �ִ� 10���� �� ����Ʈ�� ǥ��
        int count = 0;
        foreach (RoomInfo roomInfo in roomList)
        {
            if (count >= 10) break;

            // �� ����Ʈ ������ ���� �� ����
            GameObject roomListItem = Instantiate(roomListItemPrefab, roomListPanel.transform);
            roomListItem.transform.Find("RoomNameText").GetComponent<Text>().text = roomInfo.Name;
            roomListItem.transform.Find("VRText").GetComponent<Text>().text = roomInfo.CustomProperties.ContainsKey("VR") ? "VR" : "";
            roomListItem.transform.Find("PCText").GetComponent<Text>().text = roomInfo.CustomProperties.ContainsKey("PC") ? "PC" : "";
            roomListItem.transform.Find("GameStartImage").gameObject.SetActive(roomInfo.IsVisible); // ���� ���� ���� ǥ��
            roomListItem.transform.Find("GameReadyImage").gameObject.SetActive(roomInfo.PlayerCount < roomInfo.MaxPlayers); // ���� �غ� ���� ǥ��
            roomListItem.GetComponent<Button>().onClick.AddListener(() => JoinRoom(roomInfo.Name));

            count++;
        }
    }
    */


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
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 10 };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "VR", true }, { "PC", true } }; // ���� Ŀ���� �Ӽ�
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Room Created");
        Debug.Log("Room Name: " + roomName); 
    }

    
    public override void OnJoinedRoom()
    {
        // �濡 ������ �� ���� ������ ��ȯ
        SceneManager.LoadScene("4_Room"); 
    }
}
