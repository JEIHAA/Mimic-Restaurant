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
            _collider.gameObject.transform.localScale += new Vector3(70f, 70f, 70f);
        }
        if (_collider.CompareTag("Food"))
        {
            //음식
            _collider.gameObject.transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
        }
    }
}
