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
    [Header("방 이름 입력창")]
    [SerializeField] private GameObject makeRoomPanel; // 방 이름 입력창을 포함한 패널
    [Header("방 이름 입력 텍스트 상자")]
    [SerializeField] private TMP_InputField roomNameInputField = null;
    [Header("방 버튼")]
    [SerializeField] private GameObject roomBtn = null;
    [Header("방 이름")]
    [SerializeField] private TextMeshProUGUI roomBtn_text = null;
    [Header("PC 텍스트")]
    [SerializeField] private TextMeshProUGUI pctext_text = null;
    [Header("VR 텍스트")]
    [SerializeField] private TextMeshProUGUI vrtext_text = null;
    [Header("상태 텍스트")]
    [SerializeField] private TextMeshProUGUI statustext = null;
    [Header("방 만들기 버튼")]
    [SerializeField] private Button makeroombtn = null;


    [SerializeField] private string gameVersion; //게임 버전

    private string nickname_player = string.Empty; //플레이어 닉네임 

    private string roomname = string.Empty;

    private void Awake()
    {
        makeRoomPanel.SetActive(false);
        InitRoomSettings();
        //Singleton으로 아이디를 가져오기 
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


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roominfo in roomList)
        {
            //맨처음에 들어왔을때 
            if (string.IsNullOrEmpty(roomname))
            {
                makeroombtn.interactable = false;
                roomname = roominfo.Name;
                Debug.Log("Room Name: " + roomname);
                roomBtn.SetActive(true);
                roomBtn.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                roomBtn.GetComponentInChildren<Button>().onClick.AddListener(() => JoinRoomBtn(roomname));
                roomBtn_text.text = roomname; //방 이름 설정
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
                    case 0: //방에 아무도 없을때 
                        makeroombtn.interactable = true;
                        roomBtn.SetActive(false);
                        roomBtn.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                        break;
                    case 1: //1명만 들어왔을때 
                        //누가 나갔을때 실행: 반대쪽을 바꿔준다. 
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
                        //다 들어왔을때 실행: 반대쪽을 1씩 올린다. 
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
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };
        //roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "VR", true }, { "PC", true } }; // 예제 커스텀 속성
        roomOptions.PlayerTtl = 0;
        roomOptions.EmptyRoomTtl = 0;
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        //방을 만든 사람이 반장임. 
        if (XRSettings.enabled)
        {
            //VR가 방장일때 
            ht.Add("IsMasterPC", false);
        }
        else
        {
            //PC가 방장일때 
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
        // 방에 입장한 후 다음 씬으로 전환
        Debug.Log("MasterClient join this room.");
        string scenename = "4_Room"; 
        if(XRSettings.enabled)
        {
            scenename += "_VR"; 
        }
        SceneManager.LoadScene(scenename); 
    }

    #region["이미 만들어진 방에 들어가기"] 
    public void JoinRoomBtn(string _roomname)
    {
        PhotonNetwork.JoinRoom(_roomname);
        PhotonNetwork.NickName = nickname_player;
    }
    #endregion

}
