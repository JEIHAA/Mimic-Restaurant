using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class DateUIPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI day_text = null;
    [SerializeField] private TextMeshProUGUI second_text = null;
    public void SetDay(int _day)
    {
        day_text.text = _day + "¿œ";
    }

    public void SetSecond(float _second)
    {
        second_text.text = _second + ":00";
    }
}
