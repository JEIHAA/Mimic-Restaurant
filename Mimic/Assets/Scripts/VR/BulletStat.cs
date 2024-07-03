using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BulletStat", menuName = "ScriptableObjects/BulletName", order = 1)]
public class BulletStat : ScriptableObject
{
    public int bulletDamage = 100;
    public int bulletSpeed = 10;

    private void OnDisable()
    {
        bulletDamage = 100;
        bulletSpeed = 10;
    }
}
