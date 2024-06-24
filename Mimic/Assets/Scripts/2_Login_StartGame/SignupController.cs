using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 


public class SignupController : MonoBehaviour
{
    public enum SignupInfo
    {
        nickname,  
        id,  
        password,
        passwordconfirmed 
    }
    
    public delegate void OnClickInSignupDelegate(int _result);
    private OnClickInSignupDelegate onclickinsignuponclick = null;
    public OnClickInSignupDelegate OnClickInSignupOnClick
    {
        set { onclickinsignuponclick = value;  }
    }

    [Header("회원가입 정보: 닉네임, 아이디, 비밀번호, 비밀번호 확인 순서대로 넣기")]
    [SerializeField] private TMP_InputField[] signup_info = null;

    private string id = string.Empty;
    private string pw = string.Empty;
    private string pwc = string.Empty;
    private string nickname = string.Empty;

    public void OnValueChanged_ID()
    {
        id = signup_info[(int)SignupInfo.id].text; 
    }

    public void OnValueChanged_PW()
    {
        pw = signup_info[(int)SignupInfo.password].text; 
    }

    public void OnValueChanged_PWC()
    {
        pwc = signup_info[(int)SignupInfo.passwordconfirmed].text; 
    }

    public void OnValueChanged_NickName()
    {
        nickname = signup_info[(int)SignupInfo.nickname].text; 
    }

    public void SignupBtn()
    {
        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pw) && !string.IsNullOrEmpty(pwc))
        {
            if(DBManager.instance.IDOverlapCheck(id) == 0)
            {
                if (pw.Equals(pwc))
                {
                    MemberDTO memberdto = new MemberDTO(id, pw, nickname);
                    int result = DBManager.instance.InsertMember(memberdto);
                    onclickinsignuponclick?.Invoke(result);
                    //0: 프로그램 또는 서버 오류, 1: 성공 
                }
                else
                {
                    onclickinsignuponclick?.Invoke(3); 
                    //비밀번호와 비밀번호 확인이 올바르지 않음. 
                }
            }
            else
            {
                //아이디 중복 
                onclickinsignuponclick?.Invoke(2); 
            }
        }
        else
        {
            //아이디 및 비밀번호, 비밀번호 확인란 중 하나라도 비어 있을때
            onclickinsignuponclick?.Invoke(4); 
        }
    }

    public void CloseBtn()
    {
        onclickinsignuponclick?.Invoke(1); 
    }
}
