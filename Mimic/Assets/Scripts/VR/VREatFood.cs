using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VREatFood : MonoBehaviour
{
    public VRPlayer vrPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hamburger"))
        {
            vrPlayer.IncreaseHungry(vrPlayer.increaseHungry);
            other.gameObject.SetActive(false);
        }
    }
}
