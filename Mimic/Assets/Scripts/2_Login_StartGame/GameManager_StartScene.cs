using Keyboard;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class GameManager_StartScene : MonoBehaviour
{
    //VR전용
    public enum UIStatus
    {
        Login,
        Signup,
        Welcome 
    }


    private LoginController logincontroller = null;
    private SignupController signupcontroller = null;
    private WelcomeController welcomecontroller = null;

    [SerializeField] private GameObject messageui = null;
    [SerializeField] private GameObject messageui_instantiated = null;

    [SerializeField] private KeyboardManager vr_keyboard = null;

    //VR전용
    private int status_ui = 0;


    #region["Awake is called when enable scriptable instance is loaded."] 
    private void Awake()
    {
        logincontroller = GetComponentInChildren<LoginController>();
        signupcontroller = GetComponentInChildren<SignupController>(); 
        welcomecontroller = GetComponentInChildren<WelcomeController>();     

        logincontroller.OnLoginOnClick = OnLoginOnClick;
        logincontroller.OnSignupOnClick = OnSignupOnClick;

        signupcontroller.OnClickInSignupOnClick = OnClickInSignupOnClick; 

        signupcontroller.gameObject.SetActive(false);
        welcomecontroller.gameObject.SetActive(false); 

        if(SceneManager.GetActiveScene().name.Contains("VR"))
        {
            //VR일때
            CreateErrorMessageUI(2, 0);
            status_ui = (int)UIStatus.Login; 
        }
    }
    #endregion

    #region["로그인 버튼을 눌렀을때 실행되는 콜백 함수"] 
    public void OnLoginOnClick(int _result, string _id)
    {
        switch(_result)
        {
            case 0:
                //서버 오류 
                CreateErrorMessageUI(1, 2); 
                break;
            case 1:
                //로그인 성공
                logincontroller.gameObject.SetActive(false);
                welcomecontroller.gameObject.SetActive(true);
                welcomecontroller.SetText(_id);
                status_ui = (int)UIStatus.Welcome; 
                break;
            case 2:
                //아이디와 비밀번호가 올바르지 않음. 
                CreateErrorMessageUI(1, 0); 
                break;
            case 3:
                //아이디와 비밀번호 둘 중 하나를 입력하지 않았음. 
                CreateErrorMessageUI(1, 1); 
                break;
            default:
                break; 
        }
    }
    #endregion

    #region["회원가입 버튼을 눌렀을때 실행되는 콜백 함수"]
    public void OnSignupOnClick()
    {
        status_ui = (int)UIStatus.Signup; 
        logincontroller.gameObject.SetActive(false);
        signupcontroller.gameObject.SetActive(true); 
    }
    #endregion

    #region["회원가입 메뉴에서 회원가입 버튼을 눌렀을때 실행되는 콜백 함수"]
    public void OnClickInSignupOnClick(int _result)
    {
        switch (_result)
        {
            case 0:
                //서버 오류 
                CreateErrorMessageUI(0, 3); 
                break;
            case 1:
                //회원가입 성공(또는 그냥 닫기) 
                signupcontroller.gameObject.SetActive(false);
                logincontroller.gameObject.SetActive(true);
                if(XRSettings.enabled)
                {
                    CreateErrorMessageUI(3, 0); 
                }
                break;
            case 2:
                //아이디 중복  
                CreateErrorMessageUI(0, 1); 
                break;
            case 3:
                //비밀번호와 비밀번호 확인이 일치하지 않음 
                CreateErrorMessageUI(0, 2); 
                break;
            case 4:
                //비어 있을 때 
                CreateErrorMessageUI(0, 0); 
                break;
            default:
                break;
        }
    }
    #endregion

    #region["메시지 UI 생성"] 
    public void CreateErrorMessageUI(int _type, int _statusindex)
    { 
        if(messageui_instantiated == null)
        {
            messageui_instantiated = Instantiate(messageui) as GameObject;
            /*
             * type 
             * 0: 회원가입 
             * 1: 로그인
             * 
             * 
             * statusindex(회원가입)
             * 0: 비어 있을 때
             * 1: 아이디 중복
             * 2: 비밀번호와 비밀번호 확인이 일치하지 않음 
             * 3: 서버 오류 
             * 
             * statusindex(로그인)
             * 0: 아이디와 비밀번호가 올바르지 않음
             * 1: 아이디와 비밀번호 둘 중 하나를 입력하지 않았음
             * 2: 서버 오류 
            */
        }
        messageui_instantiated.GetComponent<LoginMessageUIController>().SetText(_type, _statusindex);
    }
    #endregion

    //키보드 설정하는 방법: VR 컨트롤러의 조이스틱을 올렸다 내렸다 해서 텍스트 상자의 위치 조정 가능
    //                    
    #region["키보드 관련 1 (VR전용)"] 
    private void SetKeyBoardUI(int _value)
    {
        switch(status_ui)
        {
            case 0: //Login
                //조이스틱 위, 아래 값은 0과 1 사이이므로 상관없을지도? 
                vr_keyboard.outputField = logincontroller.GetComponentsInChildren<TMP_InputField>()[_value]; 
                break; 
            case 1: //Signup 
                vr_keyboard.outputField = signupcontroller.GetComponentsInChildren<TMP_InputField>()[_value]; 
                break;
            default:
                break; 
        }
    }
    #endregion

    #region["설정 버튼"]
    public void GoSettingBtn()
    {

    }
    #endregion

    #region["닫기 버튼"]
    public void CloseBtn()
    {
        Application.Quit(); 
    }
    #endregion

}
