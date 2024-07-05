using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class ConveyorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider _collider)
    {
        if(_collider.CompareTag("Ingredient"))
        {
            //재료 
            _collider.gameObject.transform.localScale += new Vector3(1f, 1f, 1f); 
        }
        if(_collider.CompareTag("Food"))
        {
            //음식 
            _collider.gameObject.transform.localScale -= new Vector3(1f, 1f, 1f); 
        }
    }

}
