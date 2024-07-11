using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int bulletDamage = 70;
    [SerializeField] private int bulletSpeed = 20;
    [SerializeField] private BulletStat bulletData = null;
    [HideInInspector] public float activationTime;
    [SerializeField] new private ParticleSystem particleSystem;

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Monster"))
        {
            _collider.GetComponent<IOnDamage>().OnDamage(bulletDamage, null);
            particleSystem.Play();
        }
    }

    private void OnEnable()
    {
        activationTime = Time.time;
        SetBulletStat();
    }

    private void SetBulletStat()
    {
        bulletDamage = bulletData.bulletDamage;
        bulletSpeed = bulletData.bulletSpeed;
       // Debug.Log("CurrentDamage: " + bulletDamage);
    }

    public void UpgradeDamage()
    {
       // Debug.Log("UpgradeDamage");
        bulletData.bulletDamage += 20;
    }

    public void UpgradeSpeed()
    {
        bulletData.bulletSpeed += 2;
    }
}
