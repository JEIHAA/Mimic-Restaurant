using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR;

//2024-05-22: CUSTOM UNITY TEMPLATE 
/*
 2024-06-18 작성자 : 고영석
 수정 내용 : 

 */
public class SpawnManager : MonoBehaviour
{
    private IObjectPool<Monster> monsterpool = null;

    private bool collectionCheck = true;
    [Header("스폰 몬스터 기본 마리수")]
    //변경
    [SerializeField] private int defaultnum = 10;
    //변경  
    [Header("스폰 몬스터 최대 마리수")]
    [SerializeField] private int maxnum = 50;
    [Header("스폰 몬스터 Prefab")]
    [SerializeField] private Monster monsterPrefab = null;
    [Header("스폰 포인트 Prefab")]
    [SerializeField] private GameObject[] spawnpoint = null;
    [Header("스폰 포인트 순서")]
    //1일차: 3, 2일차: 5, 3일차 이후: 7
    [SerializeField, Range(3, 7)] private int maxorder = 3;
    [Header("몬스터 스폰 주기(초)")]
    [SerializeField, Range(0f, 5f)] private float spawn_second = 0.5f;
    [SerializeField] private MonsterManager monstermanager = null;

    public static SpawnManager instance = null; //Singleton 

    #region["Awake is called when enable scriptable instance is loaded."] 
    private void Awake()
    {
        if(XRSettings.enabled)
        {
            InitMonster();
            instance = this;
        } 
    }
    #endregion

    public void InitMonster()
    {
        monsterpool = new ObjectPool<Monster>(CreateMonster, SpawnMonster, FadeMonster, DestroyMonster, collectionCheck, defaultnum, maxnum);
        PreloadMonster(defaultnum);
        StartCoroutine(SpawnMonsterCoroutine());
    }

    #region["몬스터 인스턴스화 하기"] 
    private Monster CreateMonster()
    {
        Monster instance = (Instantiate(monsterPrefab.gameObject) as GameObject).GetComponent<Monster>();
        instance.OnDeathCallBack = monstermanager.MonsterDeathOnClick; 
        return instance;
    }
    #endregion

    #region["몬스터 미리 로드하기"] 
    private void PreloadMonster(int _num)
    {
        for (int i = 0; i < _num; ++i)
        {
            monsterpool.Release(CreateMonster());
        }
    }
    #endregion


    #region["1초마다 몬스터 스폰하기"] 
    private IEnumerator SpawnMonsterCoroutine()
    {
        int spawnpoint_index = 0;
        for (int i = 0; i < defaultnum; ++i)
        {
            if (spawnpoint_index < maxorder)
            {
                Monster monster = monsterpool.Get();
                SetMonsterPosandRot(monster, spawnpoint[spawnpoint_index]);
                SpawnMonster(monster);
            }
            else
            {
                spawnpoint_index = 0;
                Monster monster = monsterpool.Get();
                SetMonsterPosandRot(monster, spawnpoint[spawnpoint_index]);
                SpawnMonster(monster);
            }
            ++spawnpoint_index;
            yield return new WaitForSeconds(spawn_second);
        }
        yield break;
    }
    #endregion

    #region["몬스터의 스폰 위치 및 회전 정도 설정하기"]
    private void SetMonsterPosandRot(Monster _monster, GameObject _spawnpoint)
    {
        _monster.transform.position = _spawnpoint.transform.position;
        //_monster.transform.rotation = _spawnpoint.transform.rotation;
        _monster.transform.SetParent(monstermanager.transform);
    }
    #endregion

    #region["몬스터 활성화"] 
    public void SpawnMonster(Monster _pooledObject)
    {
        _pooledObject.gameObject.SetActive(true);
    }
    #endregion

    #region["몬스터 비활성화"] 
    public void FadeMonster(Monster _pooledObject)
    {
        _pooledObject.gameObject.SetActive(false);
    }
    #endregion

    #region["최대 개수를 초과했을 경우 몬스터를 파괴함"] 
    private void DestroyMonster(Monster _pooledObject)
    {
        Destroy(_pooledObject.gameObject);
    }
    #endregion

    #region["몬스터 풀 초기화"] 
    private void ClearMonsterPool()
    {
        monsterpool.Clear();
    }
    #endregion

    #region["다음 웨이브로 이동"] 
    public void GoNextWave()
    {
        ClearMonsterPool();
        //몬스터 리스트 삭제 
        monstermanager.DestroyMonsterList();
        //몬스터 능력치 강화 
        monstermanager.StrengthMonster(); 
        defaultnum += 5;
        if (maxorder < 7) 
        {
            maxorder += 2;
        }
        InitMonster(); 
    }
    #endregion

}
