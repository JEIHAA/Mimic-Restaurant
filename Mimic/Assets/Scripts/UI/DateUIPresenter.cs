using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class DateUIPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI day_text = null;
    [SerializeField] private TextMeshProUGUI second_text = null;
    [SerializeField] private TextMeshProUGUI breaktime_text = null;

    private void Start()
    {
        breaktime_text.transform.parent.gameObject.SetActive(false); 
    }

    public void SetDay(int _day)
    {
        day_text.text = _day + "일";
    }

    public void SetSecond(float _second)
    {
        if(!second_text.transform.parent.gameObject.activeSelf)
        {
            second_text.transform.parent.gameObject.SetActive(true);
            breaktime_text.transform.parent.gameObject.SetActive(false); 
        }
        if(_second < 60f) //1분 미만 
        {
            if(_second < 10f)
            {
                second_text.text = "00: 0" + _second; 
            }
            else
            {
                second_text.text = "00: " + _second;
            }
        }
        else //1분 이상 
        {
            if((_second - 60f) < 10f)
            {
                int minute = (int)(_second / 60f); 
                second_text.text = "0" + (int)(_second / 60f) + ":0" + (_second - (60f * minute)); 
            }
            else
            {
                int minute = (int)(_second / 60f); 
                second_text.text = "0" + (int)(_second / 60f) + ":" + (_second - (60f * minute));
            }
        }
    }

    public void SetBreakTime(float _breaktime)
    {
        if (!breaktime_text.transform.parent.gameObject.activeSelf)
        {
            breaktime_text.transform.parent.gameObject.SetActive(true);
            second_text.transform.parent.gameObject.SetActive(false); 
        }
        if (_breaktime < 10f)
        {
            breaktime_text.text = "00: 0" + _breaktime;
        }
        else
        {
            breaktime_text.text = "00: " + _breaktime;
        }
    }
}
