using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class MessageUI : MonoBehaviour
{
    [SerializeField, TextArea] private string[] infotext_str = null;
    [SerializeField] private TextMeshProUGUI infotext = null;

    #region["텍스트 설정"] 
    public void SetText(int _status)
    {
        infotext.text = infotext_str[_status];    
    }
    #endregion

    #region["닫기 버튼"]
    public void CloseBtn()
    {
        Destroy(gameObject);
        Time.timeScale = 1f; 
    }
    #endregion 
}
