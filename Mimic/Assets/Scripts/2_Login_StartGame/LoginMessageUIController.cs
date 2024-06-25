using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class LoginMessageUIController : MonoBehaviour
{
    [Header("로그인 오류 메시지")]
    [SerializeField, TextArea] private string[] statustext_login = null;
    [Header("회원가입 오류 메시지")]
    [SerializeField, TextArea] private string[] statustext_signup = null;
    [SerializeField] private TextMeshProUGUI statustext = null; 

    public void SetText(int _type, int _statusindex)
    {
        if(_type == 0) //회원가입 
        {
            statustext.text = statustext_signup[_statusindex]; 
        }
        if(_type == 1) //로그인
        {
            statustext.text = statustext_login[_statusindex]; 
        }
    }

    public void CloseBtn()
    {
        Destroy(gameObject); 
    }
}
