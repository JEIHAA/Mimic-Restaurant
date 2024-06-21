using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAttack : MonoBehaviour
{
    private int damage = 10;
    private void OnTriggerEnter(Collider other)
    {
        IOnDamage onDamage = other.GetComponent<IOnDamage>();
        onDamage?.OnDamage(damage);
        Debug.Log("-"+damage);
    }
}
