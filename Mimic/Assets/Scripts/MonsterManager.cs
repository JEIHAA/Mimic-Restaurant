using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 
/*
 2024-06-20 �ۼ��� : ���� 
 ���� ���� : 
*/

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private MonsterStat monsterData = null;

    #region["��ü �� �����̱�"] 
    public void MoveAll()
    {
        foreach(Monster monster in GetComponentsInChildren<Monster>())
        {
            if(monster.gameObject.activeSelf)
            {
                monster.Move(); 
            }
        }
    }
    #endregion

    #region["���� ���� ��ȭ"] 
    public void StrengthMonster()
    {
        monsterData.monsterHealth += 100;
        monsterData.monsterDamage += 10; 
    }
    #endregion
}
