using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class LoginController : MonoBehaviour
{
    //전체 게임 컨트롤러에 CallBack으로 전달 
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

    [Header("아이디 텍스트 상자")]
    [SerializeField] private TMP_InputField idtextbox = null;
    [Header("비밀번호 텍스트 상자")]
    [SerializeField] private TMP_InputField pwtextbox = null;

    private string id = string.Empty;
    private string pw = string.Empty;

    #region["아이디 입력"] 
    public void IDTextBox_OnValueChanged()
    {
        id = idtextbox.text; 
    }
    #endregion

    #region["비밀번호 입력"] 
    public void PWTextBox_OnValueChanged()
    {
        pw = pwtextbox.text; 
    }
    #endregion

    #region["로그인 버튼"] 
    public void LoginBtn()
    {
        if(!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pw))
        {
            int result = DBManager.instance.Login(id, pw);
            onloginonclick?.Invoke(result, id); 
            //0: 서버 오류, 1: 성공!, 2: 아이디와 비밀번호가 올바르지 않음.. 
        }
        else
        {
            //아이디와 비밀번호를 모두 입력하지 않았을때
            onloginonclick?.Invoke(3, id); 
        }
    }
    #endregion

    #region["회원가입 버튼"] 
    public void SignupBtn()
    {
        onsignuponclick?.Invoke(); 
    }
    #endregion

}
