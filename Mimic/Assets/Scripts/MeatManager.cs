using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class MeatManager : MonoBehaviour
{    
    private int meatnum = 0;

    [SerializeField] private int maxmeatnum = 10;

    public static MeatManager instance = null; //Singleton 

    private void Awake()
    {
        instance = this; 
    }

    #region["고기 개수 반환하는 메소드"] 
    public int GetMeatNum()
    {
        return meatnum; 
    }
    #endregion

    #region["고기 받아와서 등록하기"] 
    public void SetMeat(GameObject _meat)
    {
        if(meatnum < maxmeatnum)
        {
            transform.SetParent(_meat.transform); 
            ++meatnum;
        }
    }
    #endregion

    #region["몬스터한테 플레이어가 데미지를 입으면 이 메소드를 실행함"] 
    public void LoseMeatByMonster()
    {
        //맨 마지막에 있는거 파괴함. 
        if(meatnum > 0)
        {
            Destroy(GetComponentsInChildren<Meat>()[meatnum - 1].gameObject);
            --meatnum;
        }
    }
    #endregion

    #region["고기를 사용하는 메소드"]
    public void UseMeat(GameObject _meat)
    {
        if(meatnum > 0)
        {
            Destroy(_meat);
            --meatnum; 
        }
    }
    #endregion

}
