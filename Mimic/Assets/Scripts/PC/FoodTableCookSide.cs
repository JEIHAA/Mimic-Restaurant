using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class FoodTableCookSide : MonoBehaviour, IDispenser
{
    public IEnumerator GenerateFood(GameObject _food)
    {
        yield return new WaitForSeconds(0f);
        throw new System.NotImplementedException();
    }

    public bool GetIsGenerate()
    {
        throw new System.NotImplementedException();
    }

    public void OperateDispenser(GameObject _go)
    {
        Debug.Log("You need Food..."); 
    }

    private void OnTriggerStay()
    {
        Debug.Log("collider.gameObject.name: " + GetComponent<Collider>().gameObject.name); 
    }
}
