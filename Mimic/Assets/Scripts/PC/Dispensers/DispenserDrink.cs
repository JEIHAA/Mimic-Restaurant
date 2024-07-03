using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserDrink : MonoBehaviour, IDispenser
{
    public void OperateDispenser(GameObject _player)
    {
        Debug.Log("This is Coke");
    }
}
