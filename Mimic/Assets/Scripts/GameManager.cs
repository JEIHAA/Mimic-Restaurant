using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 
/*
 2024-06-20 �ۼ��� : ���� 
 ���� ���� : 
*/
public class GameManager : MonoBehaviour
{
    [Header("���� �Ŵ���")]
    [SerializeField] private MonsterManager monstermanager = null;
    [SerializeField] private SpawnManager spawnmanager = null; 
    [SerializeField] private TestDayManager daymanager = null; 

    #region["Awake is called when enable scriptable instance is loaded."] 
    private void Awake()
    {
        
    }
    #endregion


    #region["Start is called before the first frame update"] 
    private void Start()
    {
        
    }
    #endregion


    #region["Update is called once per frame"] 
    private void Update()
    {
        monstermanager.MoveAll(); 

    }
    #endregion

    #region["�׽�Ʈ��"] 
    public void AddDay()
    {
        daymanager.AddDay(); 

    }
    #endregion

}
