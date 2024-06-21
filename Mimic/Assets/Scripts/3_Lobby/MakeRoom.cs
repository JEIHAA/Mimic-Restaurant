using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject roomListPanel; // 방 리스트를 표시할 UI 패널
    [SerializeField] private GameObject roomListItemPrefab; // 방 리스트 아이템의 프리팹
    [SerializeField] private GameObject makeRoomPanel; // 방 이름 입력창을 포함한 패널
    [SerializeField] private TMP_InputField roomNameInputField; // 방 이름을 입력받을 InputField
    [SerializeField] private Button confirmRoomButton; // 방 생성 확인 버튼 
    [SerializeField] private string gameVersion; //게임 버전


    private void Awake()
    {
        makeRoomPanel.SetActive(false);
    }

    private void Start()
    {
        // 포톤 서버에 연결
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // 마스터 서버에 연결되면 로비에 입장
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // 로비에 입장하면 방 리스트 요청
        PhotonNetwork.GetCustomRoomList(TypedLobby.Default, "");
    }

    /*

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 기존 방 리스트 아이템 제거
        foreach (Transform child in roomListPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // 최대 10개의 방 리스트를 표시
        int count = 0;
        foreach (RoomInfo roomInfo in roomList)
        {
            if (count >= 10) break;

            // 방 리스트 아이템 생성 및 설정
            GameObject roomListItem = Instantiate(roomListItemPrefab, roomListPanel.transform);
            roomListItem.transform.Find("RoomNameText").GetComponent<Text>().text = roomInfo.Name;
            roomListItem.transform.Find("VRText").GetComponent<Text>().text = roomInfo.CustomProperties.ContainsKey("VR") ? "VR" : "";
            roomListItem.transform.Find("PCText").GetComponent<Text>().text = roomInfo.CustomProperties.ContainsKey("PC") ? "PC" : "";
            roomListItem.transform.Find("GameStartImage").gameObject.SetActive(roomInfo.IsVisible); // 게임 시작 상태 표시
            roomListItem.transform.Find("GameReadyImage").gameObject.SetActive(roomInfo.PlayerCount < roomInfo.MaxPlayers); // 게임 준비 상태 표시
            roomListItem.GetComponent<Button>().onClick.AddListener(() => JoinRoom(roomInfo.Name));

            count++;
        }
    }
    */


    private void JoinRoom(string roomName)
    {
        // 방에 들어가기 전에 확인 알림창 표시
        ConfirmJoinRoom(roomName);
    }

    private void ConfirmJoinRoom(string roomName)
    {
        // 확인 알림창 표시 후 확인 버튼 클릭 시 방 입장
        bool confirmed = true; // 실제로는 확인 알림창의 결과를 받아야 함

        if (confirmed)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public void OpenMakeRoomPanel()
    {
        // 방 이름 입력창 패널 표시
        makeRoomPanel.SetActive(true);
    }
    
    public void CloseMakeRoomPanel()
    {
         // 방 이름 입력창 닫기
         makeRoomPanel.SetActive(false); 
    }

    public void CreateRoom()
    {
        string roomName = roomNameInputField.text;

        // 방 이름이 비어있다면 에러 메시지 출력 후 종료
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogError("Room name is empty!");
            return;
        }

        // 방 옵션 설정 및 방 생성
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 10 };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "VR", true }, { "PC", true } }; // 예제 커스텀 속성
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Room Created");
        Debug.Log("Room Name: " + roomName); 
    }

    
    public override void OnJoinedRoom()
    {
        // 방에 입장한 후 다음 씬으로 전환
        SceneManager.LoadScene("4_Room"); 
    }
}
