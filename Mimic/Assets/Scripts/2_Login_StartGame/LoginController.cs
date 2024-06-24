using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class LoginController : MonoBehaviour
{
    //��ü ���� ��Ʈ�ѷ��� CallBack���� ���� 
    public delegate void OnLoginDelegate(int _result, string _id);
    public delegate void OnSignupDelegate();
    private OnLoginDelegate onloginonclick = null;
    private OnSignupDelegate onsignuponclick = null;
    public OnLoginDelegate OnLoginOnClick
    {
        set { onloginonclick = value;  }
    }
    
    public OnSignupDelegate OnSignupOnClick
    {
        set { onsignuponclick = value;  }
    }

    [Header("���̵� �ؽ�Ʈ ����")]
    [SerializeField] private TMP_InputField idtextbox = null;
    [Header("��й�ȣ �ؽ�Ʈ ����")]
    [SerializeField] private TMP_InputField pwtextbox = null;

    private string id = string.Empty;
    private string pw = string.Empty;

    #region["���̵� �Է�"] 
    public void IDTextBox_OnValueChanged()
    {
        id = idtextbox.text; 
    }
    #endregion

    #region["��й�ȣ �Է�"] 
    public void PWTextBox_OnValueChanged()
    {
        pw = pwtextbox.text; 
    }
    #endregion

    #region["�α��� ��ư"] 
    public void LoginBtn()
    {
        if(!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pw))
        {
            int result = DBManager.instance.Login(id, pw);
            onloginonclick?.Invoke(result, id); 
            //0: ���� ����, 1: ����!, 2: ���̵�� ��й�ȣ�� �ùٸ��� ����.. 
        }
        else
        {
            //���̵�� ��й�ȣ�� ��� �Է����� �ʾ�����
            onloginonclick?.Invoke(3, id); 
        }
    }
    #endregion

    #region["ȸ������ ��ư"] 
    public void SignupBtn()
    {
        onsignuponclick?.Invoke(); 
    }
    #endregion

}
