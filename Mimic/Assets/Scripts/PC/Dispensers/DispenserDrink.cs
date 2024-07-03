using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserDrink : MonoBehaviour, IDispenser
{
    public void OperateDispenser()
    {
        Debug.Log("This is Coke");
    }
}
