using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class FoodTableCookSide : MonoBehaviour, IDispenser
{
    private GameObject food = null;

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
        if (_player.GetComponentInChildren<FoodTest>().gameObject != null)
        {
            food = _player.GetComponentInChildren<FoodTest>().gameObject;
            if(GetComponentInChildren<FoodTable>() != null)
            {
                food.transform.position = GetComponentInChildren<FoodTable>().gameObject.transform.position;
                food.transform.SetParent(transform);
            }
        }
    }
}
