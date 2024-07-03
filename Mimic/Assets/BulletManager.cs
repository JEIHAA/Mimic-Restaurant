using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private BulletStat bulletData = null;

    public void UpgradeDamage()
    {
        bulletData.bulletDamage += 100;
    }
    public void UpgradeSpeed()
    {
        bulletData.bulletSpeed += 1;
    }
}
