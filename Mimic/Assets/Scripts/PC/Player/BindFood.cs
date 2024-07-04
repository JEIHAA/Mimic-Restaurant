using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BindFood : MonoBehaviour
{
    [SerializeField] private bool hasFood = false;
    public bool HasFood => hasFood;
    [SerializeField] private GameObject food = null;
    public GameObject Food { get => food; set => food = value; }

    private void OnTriggerStay(Collider _other)
    {
        if (hasFood)
        {
            return;
        }
        if (_other.gameObject.layer == LayerMask.NameToLayer("Food") || _other.gameObject.layer == LayerMask.NameToLayer("Ingredient") && !_other.GetComponent<Ingredients>().IsCooking)
        {
            CatchFood(_other);
        }
    }

    private void CatchFood(Collider _other) 
    {
        if (Input.GetKeyDown("f")) 
        {
            //if (_other.GetComponent<Ingredients>().IsCooking) return;
            Debug.Log("GetFood");
            if (_other.gameObject.transform.parent != null)
            {
                food = _other.gameObject.transform.parent.gameObject;
            }
            else
            { 
                food = _other.gameObject;
            }
            SetFoodPos();
        }
    }

    public void DropFood() 
    {
        if (hasFood)
        {
            if (Input.GetKeyDown("e"))
            {
                food.transform.parent = null;
                foreach (Rigidbody rb in food.GetComponentsInChildren<Rigidbody>())
                {
                    rb.isKinematic = false;
                }
                food = null;
                hasFood = false;
            }
        }
    }

    private void SetFoodPos() 
    {
        food.transform.parent = this.transform;
        food.gameObject.transform.localPosition = Vector3.zero;
        foreach (Rigidbody rb in food.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
            rb.gameObject.transform.localPosition = Vector3.zero;
        }
        hasFood = true;
    }

    public void BindCheck() 
    {
        if (food == null)
        {
            hasFood = false;
        }
        if (food != null && food.transform.parent == null) 
        {
            SetFoodPos();
        }
    }

}
