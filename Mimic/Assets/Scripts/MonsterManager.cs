using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 
/*
 2024-06-20 작성자 : 고영석 
 수정 내용 : 
*/

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private MonsterStat monsterData = null;

    #region["전체 다 움직이기"] 
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

    #region["자식 몬스터 오브젝트 지우기"] 
    public void DestroyMonsterList()
    {
        foreach(Transform monster in transform)
        {
            Debug.Log("Monster List: " + monster.gameObject.name); 
            Destroy(monster.gameObject);     
        }
    }
    #endregion

    #region["몬스터 스탯 강화"] 
    public void StrengthMonster()
    {
        monsterData.monsterHealth += 100;
        monsterData.monsterDamage += 10; 
    }
    #endregion
}
