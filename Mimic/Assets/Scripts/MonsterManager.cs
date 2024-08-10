using JetBrains.Annotations;
using Photon.Pun;
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
    [SerializeField] private MeatManager meatmanager = null;

    private int monster_killed = 0;

    #region["죽인 몬스터 수 가져오기"] 

    public int GetMonster_Killed()
    {
        return monster_killed; 
    }
    #endregion

    #region["VR 정산: 몬스터 수 초기화하기"] 
    public void ClearMonster()
    {
        monster_killed = 0; 
    }
    #endregion

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
            Destroy(monster.gameObject);     
        }
    }
    #endregion

    #region["몬스터 스탯 강화"] 
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

    #region["몬스터가 죽을때 작동하는 콜백 메소드"]
    public void MonsterDeathOnClick(int _meatnum)
    {
        ++monster_killed;
        /*
        for(int i=0; i<_meatnum; ++i)
        {
            if(PhotonNetwork.IsConnected)
            {
                GameObject steak = PhotonNetwork.Instantiate("Prefabs\\Food\\Ingredient\\Steak", new Vector3(-1.2f, 1.5f, -1.5f), Quaternion.identity);
                meatmanager.SetMeat(steak);
            }
        }
        */
        if (PhotonNetwork.IsConnected)
        {
            GameObject steak = PhotonNetwork.Instantiate("Prefabs\\Food\\Ingredient\\Steak", new Vector3(-1.2f, 1.5f, -1.5f), Quaternion.identity);
            meatmanager.SetMeat(steak);
        }
        //고기 매니저에 등록하기
    }
    #endregion
}
