using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserHamburger : MonoBehaviour, IDispenser
{
    public void OperateDispenser(GameObject _player)
    {
        Debug.Log("This is Ham");
    }
}
