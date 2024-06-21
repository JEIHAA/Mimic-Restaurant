using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class WelcomeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI welcometext = null;

    public static WelcomeController instance = null; //Singleton 

    private string id = string.Empty;

    private void Awake()
    {
        instance = this; 
    }

    #region["아이디 설정"] 
    public void SetText(string _id)
    {
        welcometext.text = _id + "님 환영합니다.";
        id = _id; 
    }
    #endregion

    #region["게임 시작 버튼"] 
    public void StartButton()
    {
        //방 만드는 Scene로 이동 
        SceneManager.LoadScene("Scene_RoomName");
        //아이디를 싱글톤으로 보내야 하기 때문에 여기서 게임 오브젝트를 파괴하지 않음. 
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region["싱글톤 객체에서 아이디를 불러오기 위해 사용"] 
    public string getId()
    {
        return id; 
    }
    #endregion

}
