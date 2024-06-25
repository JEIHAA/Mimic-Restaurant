using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//2024-05-22: CUSTOM UNITY TEMPLATE 

//테스트용 TEST ONLY 
public class TestDayManager : MonoBehaviour
{
    [SerializeField] private int day = 1; //1,2,3,4,5,6 
    [SerializeField] private TextMeshProUGUI daytext = null; 

    public void AddDay()
    {
        ++day;
        daytext.text = "CURRENT DAY: " + day;
        if (day == 6)
        {
            //Game Finished(테스트용이므로 따로 코드는 작성하지 않겠음) 
        }
    }
    
}
