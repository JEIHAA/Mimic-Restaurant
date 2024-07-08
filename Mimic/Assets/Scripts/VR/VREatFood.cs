using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VREatFood : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hamburger"))
        {
           // IncreaseHungry(increaseHungry);
            // Debug.Log("hamburger +20 Current hungry: " + hungry);
            other.gameObject.SetActive(false);
        }
    }
}
