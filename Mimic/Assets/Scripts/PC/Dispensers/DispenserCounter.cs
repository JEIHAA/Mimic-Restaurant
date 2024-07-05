using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class DispenserCounter : MonoBehaviour, IDispenser
{
    private GameObject output = null;

    public bool GetIsGenerate() { return false;  }
    public IEnumerator GenerateFood(GameObject _outputPrefab) { yield return null; }

    public void OperateDispenser(GameObject _player)
    {
        GameObject food = _player.GetComponentInChildren<BindFood>().Food;
        if(food == null)
        {
            Debug.Log("음식이 필요합니다.");
            return; 
        }
        else
        {
            //음식 받기 
            //GetComponentInChildren<FoodTable>().
        }
    }

}
