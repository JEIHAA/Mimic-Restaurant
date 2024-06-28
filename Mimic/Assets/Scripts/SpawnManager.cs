using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR;

//2024-05-22: CUSTOM UNITY TEMPLATE 
/*
 2024-06-18 �ۼ��� : ����
 ���� ���� : 

 */
public class SpawnManager : MonoBehaviour
{
    private IObjectPool<Monster> monsterpool = null;

    private bool collectionCheck = true;
    [Header("���� ���� �⺻ ������")]
    //����
    [SerializeField] private int defaultnum = 10;
    //����  
    [Header("���� ���� �ִ� ������")]
    [SerializeField] private int maxnum = 50;
    [Header("���� ���� Prefab")]
    [SerializeField] private Monster monsterPrefab = null;
    [Header("���� ����Ʈ Prefab")]
    [SerializeField] private GameObject[] spawnpoint = null;
    [Header("���� ����Ʈ ����")]
    //1����: 3, 2����: 5, 3���� ����: 7
    [SerializeField, Range(3, 7)] private int maxorder = 3;
    [Header("���� ���� �ֱ�(��)")]
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

    #region["���� �ν��Ͻ�ȭ �ϱ�"] 
    private Monster CreateMonster()
    {
        Monster instance = (Instantiate(monsterPrefab.gameObject) as GameObject).GetComponent<Monster>();
        instance.OnDeathCallBack = monstermanager.MonsterDeathOnClick; 
        return instance;
    }
    #endregion

    #region["���� �̸� �ε��ϱ�"] 
    private void PreloadMonster(int _num)
    {
        for (int i = 0; i < _num; ++i)
        {
            monsterpool.Release(CreateMonster());
        }
    }
    #endregion


    #region["1�ʸ��� ���� �����ϱ�"] 
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

    #region["������ ���� ��ġ �� ȸ�� ���� �����ϱ�"]
    private void SetMonsterPosandRot(Monster _monster, GameObject _spawnpoint)
    {
        _monster.transform.position = _spawnpoint.transform.position;
        //_monster.transform.rotation = _spawnpoint.transform.rotation;
        _monster.transform.SetParent(monstermanager.transform);
    }
    #endregion

    #region["���� Ȱ��ȭ"] 
    public void SpawnMonster(Monster _pooledObject)
    {
        _pooledObject.gameObject.SetActive(true);
    }
    #endregion

    #region["���� ��Ȱ��ȭ"] 
    public void FadeMonster(Monster _pooledObject)
    {
        _pooledObject.gameObject.SetActive(false);
    }
    #endregion

    #region["�ִ� ������ �ʰ����� ��� ���͸� �ı���"] 
    private void DestroyMonster(Monster _pooledObject)
    {
        Destroy(_pooledObject.gameObject);
    }
    #endregion

    #region["���� Ǯ �ʱ�ȭ"] 
    private void ClearMonsterPool()
    {
        monsterpool.Clear();
    }
    #endregion

    #region["���� ���̺�� �̵�"] 
    public void GoNextWave()
    {
        ClearMonsterPool();
        //���� ����Ʈ ���� 
        monstermanager.DestroyMonsterList();
        //���� �ɷ�ġ ��ȭ 
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
