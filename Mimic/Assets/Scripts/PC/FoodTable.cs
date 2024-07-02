using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class FoodTable : MonoBehaviour
{
    public delegate void OnGetFoodDelegate(GameObject _food);
    private OnGetFoodDelegate ongetfoodonclick = null;
    public OnGetFoodDelegate OnGetFoodOnClick
    {
        set { ongetfoodonclick = value;  }
    }
    
    private void OnTriggerEnter(Collider _collider)
    {
        //���߿� ���� ������Ʈ�� ���� �±� �߰��ؾ���. 
        if(_collider.CompareTag("Food"))
        {
            ongetfoodonclick?.Invoke(_collider.gameObject); 
        }
    }

}
