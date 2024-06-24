using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public int damage = 100;

    private void OnTriggerEnter(Collider _collider)
    {
        Debug.Log("collider.name: " + _collider.name);
        if (_collider.CompareTag("Monster"))
        {
            _collider.GetComponent<IOnDamage>().OnDamage(damage);
        }
    }
       
}
