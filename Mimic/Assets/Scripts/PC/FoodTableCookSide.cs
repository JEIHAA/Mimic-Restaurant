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
        if (_player.GetComponentInChildren<FoodInfo>().gameObject != null)
        {
            food = _player.GetComponentInChildren<FoodInfo>().gameObject;
            //가지고 있는 게 음식이어야 줄 수 있음. 
            if (GetComponentInChildren<FoodTable>() != null)
            {
                food.transform.position = GetComponentInChildren<FoodTable>().gameObject.transform.position;
                food.transform.SetParent(transform);
            }
            else
            {
                Debug.Log("This is not Food.");
            }
        }
        else
        {

        }
    }

    private void OnTriggerStay(Collider _collider)
    {
        if(_collider.name.Equals("PCPlayer"))
        {
            
            food = _collider.GetComponentInChildren<FoodInfo>().gameObject; 
        }
    }

}
