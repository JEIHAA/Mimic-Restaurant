using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class FoodTableCookSide : MonoBehaviour, IDispenser
{
    private GameObject food = null;
    public delegate void OnGetFoodDelegate(GameObject _food, GameObject _player);
    private OnGetFoodDelegate ongetfoodonclick = null;
    private Boolean isUsedByCustomer = false; 

    public OnGetFoodDelegate OnGetFoodOnClick
    {
        set { ongetfoodonclick = value; }
    }

    public Boolean IsUsedByCustomer
    {
        set { isUsedByCustomer = value;  }
        //get { return isUsedByCustomer; }
    }

    public IEnumerator GenerateFood(GameObject _food)
    {
        throw new System.NotImplementedException();
    }

    public bool GetIsGenerate()
    {
        throw new System.NotImplementedException();
    }

    public void OperateDispenser(GameObject _player)
    {
        if (_player.GetComponentInChildren<FoodInfo>().gameObject != null)
        {
            food = _player.GetComponentInChildren<FoodInfo>().gameObject;
            BindFood bindfood = _player.GetComponentInChildren<BindFood>(); 
            //가지고 있는 게 음식이어야 줄 수 있음. 
            if (GetComponentInChildren<FoodTable>() != null && food != null)
            {
                if(isUsedByCustomer)
                {
                    food.GetComponent<Collider>().enabled = true;
                    food.transform.position = GetComponentInChildren<FoodTable>().gameObject.transform.position;
                    food.transform.SetParent(transform);
                    ongetfoodonclick?.Invoke(food, _player);
                }
            }
            else
            {
                Debug.Log("This is not Food.");
            }
        }
    }

}
