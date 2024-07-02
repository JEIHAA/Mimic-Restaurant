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
        //나중에 음식 오브젝트에 음식 태그 추가해야함. 
        if(_collider.CompareTag("Food"))
        {
            ongetfoodonclick?.Invoke(_collider.gameObject); 
        }
    }

}
