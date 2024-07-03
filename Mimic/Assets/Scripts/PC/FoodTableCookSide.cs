using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class FoodTableCookSide : MonoBehaviour, IDispenser
{

    public void OperateDispenser()
    {
        Debug.Log("You need Food..."); 
    }

    private void OnTriggerStay()
    {
        Debug.Log("collider.gameObject.name: " + GetComponent<Collider>().gameObject.name); 
    }
}
