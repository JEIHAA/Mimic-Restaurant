using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//2024-05-22: CUSTOM UNITY TEMPLATE 

//�׽�Ʈ�� TEST ONLY 
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
            //Game Finished(�׽�Ʈ���̹Ƿ� ���� �ڵ�� �ۼ����� �ʰ���) 
        }
    }
    
}
