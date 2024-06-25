using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int baseDamage = 100;
    private int currentDamage;
    [HideInInspector] public float activationTime;

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Monster"))
        {
            _collider.GetComponent<IOnDamage>().OnDamage(currentDamage);
        }
    }

    private void OnEnable()
    {
        activationTime = Time.time;
    }

    private void Start()
    {
        ResetDamage();
    }

    public void ResetDamage()
    {
        currentDamage = baseDamage;
    }

    public void UpgradeBullet()
    {
        currentDamage += 100;
    }

    public int GetCurrentDamage()
    {
        return currentDamage;
    }
}
