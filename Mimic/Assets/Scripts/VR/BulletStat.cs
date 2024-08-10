using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BulletStat", menuName = "ScriptableObjects/BulletName", order = 1)]
public class BulletStat : ScriptableObject
{
    public int bulletDamage = 150;
    public int bulletSpeed = 20;

    private void OnDisable()
    {
        bulletDamage = 150;
        bulletSpeed = 20;
    }
}
