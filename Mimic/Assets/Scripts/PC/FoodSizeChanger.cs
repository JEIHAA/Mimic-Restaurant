using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSizeChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Ingredient"))
        {
            //재료
            if (_collider.GetComponent<Ingredients>().IsGoPC == false)
            {
                _collider.gameObject.transform.localScale += new Vector3(50f, 50f, 30f);
                _collider.GetComponent<Ingredients>().IsGoPC = true;
                MeatManager.instance.UseMeat(_collider.gameObject);
            }
        }
        if (_collider.CompareTag("Food"))
        {
            //음식
            if(_collider.GetComponent<FoodInfo>().IsGoVR == false)
            {
                _collider.gameObject.transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
                _collider.GetComponent<FoodInfo>().IsGoVR = true; 
            }
        }
    }
}
