using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

[CreateAssetMenu(fileName = "MonsterStat", menuName = "ScriptableObjects/MonsterStat", order = 1)]
public class MonsterStat : ScriptableObject
{
    public int monsterHealth = 500; 
    public int monsterDamage = 10;
    public int meatnum = 3; 

    private void OnDisable()
    {
        monsterHealth = 500; 
        monsterDamage = 10;
        meatnum = 3; 
    }
}
