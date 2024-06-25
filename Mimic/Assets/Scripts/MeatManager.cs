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

    #region["��� ���� ��ȯ�ϴ� �޼ҵ�"] 
    public int GetMeatNum()
    {
        return meatnum; 
    }
    #endregion

    #region["��� �޾ƿͼ� ����ϱ�"] 
    public void SetMeat(GameObject _meat)
    {
        if(meatnum < maxmeatnum)
        {
            transform.SetParent(_meat.transform); 
            ++meatnum;
        }
    }
    #endregion

    #region["�������� �÷��̾ �������� ������ �� �޼ҵ带 ������"] 
    public void LoseMeatByMonster()
    {
        //�� �������� �ִ°� �ı���. 
        if(meatnum > 0)
        {
            Destroy(GetComponentsInChildren<Meat>()[meatnum - 1].gameObject);
            --meatnum;
        }
    }
    #endregion

    #region["��⸦ ����ϴ� �޼ҵ�"]
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
