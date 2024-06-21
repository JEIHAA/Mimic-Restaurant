using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class GameManager_StartScene : MonoBehaviour
{
    private LoginController logincontroller = null;
    private SignupController signupcontroller = null;
    private WelcomeController welcomecontroller = null;

    [SerializeField] private GameObject messageui = null;
    private GameObject messageui_instantiated = null; 

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
            messageui_instantiated.GetComponent<LoginMessageUIController>().SetText(_type, _statusindex); 
        }
    }
    #endregion
}
