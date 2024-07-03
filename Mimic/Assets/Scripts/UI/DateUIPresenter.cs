using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class DateUIPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI day_text = null;
    [SerializeField] private TextMeshProUGUI second_text = null;
    [SerializeField] private TextMeshProUGUI breaktime_text = null;

    
    public void SetDay(int _day)
    {
        day_text.text = _day + "일";
        Debug.LogError("Day: " + _day); 
    }

    public void SetSecond(float _second)
    {
        if(second_text != null)
        {
            if (_second < 60f) //1분 미만 
            {
                second_text.text = "00:" + (int)_second;
            }
            else //1분 초과 
            {
                second_text.text = "0" + (int)(_second / 60f) + ":" + (int)(_second - 60f);
            }
        }
        Debug.LogError("Second: " + (int)_second); 
    }

    public void SetBreakTime(float _breaktime)
    {
        if(breaktime_text != null)
        {
            //쉬는시간 설정 
            breaktime_text.text = "00: " + (int)_breaktime;
        }
        Debug.LogError("BreakTime: " + (int)_breaktime);  
    }
}
