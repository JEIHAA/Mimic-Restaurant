using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class LoginMessageUIController : MonoBehaviour
{
    [Header("�α��� ���� �޽���")]
    [SerializeField, TextArea] private string[] statustext_login = null;
    [Header("ȸ������ ���� �޽���")]
    [SerializeField, TextArea] private string[] statustext_signup = null;
    [SerializeField] private TextMeshProUGUI statustext = null; 

    public void SetText(int _type, int _statusindex)
    {
        if(_type == 0) //ȸ������ 
        {
            statustext.text = statustext_signup[_statusindex]; 
        }
        if(_type == 1) //�α���
        {
            statustext.text = statustext_login[_statusindex]; 
        }
        if(_type == 2) //VR���� 
        {
            statustext.text = "�α������ּ���."; 
        }
        if(_type == 3) //VR���� 
        {
            statustext.text = "�α��� ����!"; 
        }
    }

    public void CloseBtn()
    {
        Destroy(gameObject); 
    }
}
