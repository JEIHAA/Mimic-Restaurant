using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

//�׽�Ʈ�� 
public class TestDayManager : MonoBehaviour
{
    [SerializeField] private int day = 1; //1,2,3,4,5,6 

    public void AddDay()
    {
        ++day; 
        if(day == 6)
        {
            //Game Finished(�׽�Ʈ���̹Ƿ� ���� �ڵ�� �ۼ����� �ʰ���) 
        }
    }
    
}
