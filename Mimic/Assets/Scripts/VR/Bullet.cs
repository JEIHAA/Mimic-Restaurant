using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public int damage = 100;
    public float activationTime;

    private void OnTriggerEnter(Collider _collider)
    {
        Debug.Log("collider.name: " + _collider.name);
        if (_collider.CompareTag("Monster"))
        {
            _collider.GetComponent<IOnDamage>().OnDamage(damage);
        }
    }

    private void OnEnable()
    {
        activationTime = Time.time; 
    }

    private void Start()
    {
        damage = 100;
    }

    public void UpgradeBullet()
    {
        damage += 100;
    }
}
