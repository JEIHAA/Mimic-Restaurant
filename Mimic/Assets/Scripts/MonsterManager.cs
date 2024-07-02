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
    [SerializeField] private MeatManager meatmanager = null; 

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
            Destroy(monster.gameObject);     
        }
    }
    #endregion

    #region["���� ���� ��ȭ"] 
    public void StrengthMonster(int _round, int _wave)
    {
        monsterData.monsterHealth += 50;
        if(_round % 2 == 0 && _wave == 1)
        {
            monsterData.monsterDamage += 3; 
        }
        if(_round % 2 != 0 && _wave == 2)
        {
            monsterData.monsterDamage += 2; 
        }
        if(_wave == 2)
        {
            monsterData.meatnum += 1; 
        }
    }
    #endregion

    #region["���Ͱ� ������ �۵��ϴ� �ݹ� �޼ҵ�"] 
    public void MonsterDeathOnClick(GameObject _steak, int _meat_num)
    {
        //��� �Ŵ����� ����ϱ� 
        for (int i = 0; i < _meat_num; ++i)
        {
            meatmanager.SetMeat(_steak);
        }
    }
    #endregion

}
