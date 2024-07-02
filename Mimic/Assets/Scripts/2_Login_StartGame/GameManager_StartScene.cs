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
    [SerializeField] private GameObject settingui = null;
    private GameObject settingui_instantiate = null;
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


    #region["설정 버튼"]
    public void GoSettingBtn()
    {
        if (!XRSettings.enabled && settingui_instantiate == null)
        {
            settingui_instantiate = Instantiate(settingui);
            settingui_instantiate.GetComponentInChildren<GameController_Setting>().CloseSettingsOnClick = ExitSettingsOnClick;
            logincontroller.gameObject.SetActive(false);
        }
        if(XRSettings.enabled)
        {
            CreateErrorMessageUI(4, 0); 
        }
    }
    #endregion

    #region["설정 닫기 버튼"]
    public void ExitSettingsOnClick()
    {
        Destroy(settingui_instantiate);
        logincontroller.gameObject.SetActive(true); 
    }
    #endregion

    #region["닫기 버튼"]
    public void CloseBtn()
    {
        Application.Quit(); 
    }
    #endregion

}
