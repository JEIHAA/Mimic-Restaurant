using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 
/*
 2024-06-20 작성자 : 고영석 
 수정 내용 : 
*/
public class GameManager : MonoBehaviour
{
    [Header("몬스터 매니저")]
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

    #region["테스트용"] 
    public void AddDay()
    {
        daymanager.AddDay(); 

    }
    #endregion

}
