using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

[CreateAssetMenu(fileName = "MonsterStat", menuName = "ScriptableObjects/MonsterStat", order = 1)]
public class MonsterStat : ScriptableObject
{
    public int monsterHealth = 300;
    public int monsterDamage = 10;
}
