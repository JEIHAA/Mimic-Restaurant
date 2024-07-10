using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VREatFood : MonoBehaviour
{
    public VRPlayer vrPlayer;
    public HandCtrl leftHand;
    public HandCtrl righthand;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            vrPlayer.IncreaseHungry(vrPlayer.increasehungry);
            other.gameObject.SetActive(false);
            leftHand.isColliding = false;
            righthand.isColliding = false;
        }
    }
}
