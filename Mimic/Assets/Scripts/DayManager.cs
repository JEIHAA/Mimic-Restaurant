using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DayManager : MonoBehaviourPun 
{
    [SerializeField] private int day = 1; //1,2,3,4,5,6 
    [SerializeField] private TextMeshProUGUI daytext = null;

    private Boolean status = false;

    private void Awake()
    {
        
    }

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
