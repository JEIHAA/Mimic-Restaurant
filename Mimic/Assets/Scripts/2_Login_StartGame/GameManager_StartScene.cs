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
    //VR����
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

    //VR����
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
            //VR�϶�
            CreateErrorMessageUI(2, 0);
            status_ui = (int)UIStatus.Login; 
        }
    }
    #endregion

    #region["�α��� ��ư�� �������� ����Ǵ� �ݹ� �Լ�"] 
    public void OnLoginOnClick(int _result, string _id)
    {
        switch(_result)
        {
            case 0:
                //���� ���� 
                CreateErrorMessageUI(1, 2); 
                break;
            case 1:
                //�α��� ����
                logincontroller.gameObject.SetActive(false);
                welcomecontroller.gameObject.SetActive(true);
                welcomecontroller.SetText(_id);
                status_ui = (int)UIStatus.Welcome; 
                break;
            case 2:
                //���̵�� ��й�ȣ�� �ùٸ��� ����. 
                CreateErrorMessageUI(1, 0); 
                break;
            case 3:
                //���̵�� ��й�ȣ �� �� �ϳ��� �Է����� �ʾ���. 
                CreateErrorMessageUI(1, 1); 
                break;
            default:
                break; 
        }
    }
    #endregion

    #region["ȸ������ ��ư�� �������� ����Ǵ� �ݹ� �Լ�"]
    public void OnSignupOnClick()
    {
        status_ui = (int)UIStatus.Signup; 
        logincontroller.gameObject.SetActive(false);
        signupcontroller.gameObject.SetActive(true); 
    }
    #endregion

    #region["ȸ������ �޴����� ȸ������ ��ư�� �������� ����Ǵ� �ݹ� �Լ�"]
    public void OnClickInSignupOnClick(int _result)
    {
        switch (_result)
        {
            case 0:
                //���� ���� 
                CreateErrorMessageUI(0, 3); 
                break;
            case 1:
                //ȸ������ ����(�Ǵ� �׳� �ݱ�) 
                signupcontroller.gameObject.SetActive(false);
                logincontroller.gameObject.SetActive(true);
                if(XRSettings.enabled)
                {
                    CreateErrorMessageUI(3, 0); 
                }
                break;
            case 2:
                //���̵� �ߺ�  
                CreateErrorMessageUI(0, 1); 
                break;
            case 3:
                //��й�ȣ�� ��й�ȣ Ȯ���� ��ġ���� ���� 
                CreateErrorMessageUI(0, 2); 
                break;
            case 4:
                //��� ���� �� 
                CreateErrorMessageUI(0, 0); 
                break;
            default:
                break;
        }
    }
    #endregion

    #region["�޽��� UI ����"] 
    public void CreateErrorMessageUI(int _type, int _statusindex)
    { 
        if(messageui_instantiated == null)
        {
            messageui_instantiated = Instantiate(messageui) as GameObject;
            /*
             * type 
             * 0: ȸ������ 
             * 1: �α���
             * 
             * 
             * statusindex(ȸ������)
             * 0: ��� ���� ��
             * 1: ���̵� �ߺ�
             * 2: ��й�ȣ�� ��й�ȣ Ȯ���� ��ġ���� ���� 
             * 3: ���� ���� 
             * 
             * statusindex(�α���)
             * 0: ���̵�� ��й�ȣ�� �ùٸ��� ����
             * 1: ���̵�� ��й�ȣ �� �� �ϳ��� �Է����� �ʾ���
             * 2: ���� ���� 
            */
        }
        messageui_instantiated.GetComponent<LoginMessageUIController>().SetText(_type, _statusindex);
    }
    #endregion


    #region["���� ��ư"]
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

    #region["���� �ݱ� ��ư"]
    public void ExitSettingsOnClick()
    {
        Destroy(settingui_instantiate);
        logincontroller.gameObject.SetActive(true); 
    }
    #endregion

    #region["�ݱ� ��ư"]
    public void CloseBtn()
    {
        Application.Quit(); 
    }
    #endregion

}
