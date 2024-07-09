using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Pool;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class MeatManager : MonoBehaviour
{
    [SerializeField] private int meatnum = 6; 
    private int meatnum_acummulated = 0;

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

    #region["누적 고기 개수 반환: 정산 UI에 출력"]
    public int GetMeatNumAcummlated()
    {
        return meatnum_acummulated;
    }
    #endregion

    #region["고기 정산 정보 지우기"]
    public void ClearMeatAcummlated()
    {
        meatnum_acummulated = 0;
    }
    #endregion

    #region["고기 받아와서 등록하기"] 
    public void SetMeat(GameObject _meat)
    {
        //transform.SetParent(_meat.transform); 
        _meat.transform.SetParent(transform);
        ++meatnum;
        ++meatnum_acummulated;
    }
    #endregion

    #region["몬스터한테 플레이어가 데미지를 입으면 이 메소드를 실행함"] 
    public void LoseMeatByMonster(GameObject _monster)
    {
        if(meatnum >= 0)
        {
            //몬스터가 고기를 가지기 
            GameObject hand = _monster.GetComponentInChildren<MonsterHand>().gameObject;
            GameObject steak = GetComponentsInChildren<Steak>()[meatnum - 1].gameObject;
            steak.GetComponent<Collider>().enabled = false;
            steak.GetComponent<Rigidbody>().isKinematic = true;
            steak.GetComponent<Rigidbody>().useGravity = false;            
            steak.transform.position = hand.transform.position;
            steak.transform.SetParent(hand.transform);
            --meatnum;
        }
    }
    #endregion

    #region["고기를 사용하는 메소드"]
    public void UseMeat(GameObject _meat)
    {
        if (meatnum > 0)
        {
            _meat.transform.SetParent(null);
            --meatnum;
        }
    }
    #endregion
}
