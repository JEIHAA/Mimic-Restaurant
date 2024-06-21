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
    public void MoveAll(Transform _vrplayer_position)
    {
        foreach(Monster monster in GetComponentsInChildren<Monster>())
        {
            if(monster.gameObject.activeSelf)
            {
                monster.Move(_vrplayer_position); 
            }
        }
    }
    #endregion

    #region["�ڽ� ���� ������Ʈ �����"] 
    public void DestroyMonsterList()
    {
        foreach(Transform monster in transform)
        {
            Debug.Log("Monster List: " + monster.gameObject.name); 
            Destroy(monster.gameObject);     
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
