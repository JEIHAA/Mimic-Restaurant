using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class CustomerSpawnManager : MonoBehaviour
{
    [Header("�մ� �迭: �ܰ���, �ܰ���2, ������, ������2")]
    [SerializeField] private GameObject[] customer = null;
    [Header("�մ� ���")]
    [SerializeField, Range(6, 10)] private int customer_num = 8;
    [Header("�Ĵ翡 ���� �� �ִ� �մ� ���")]
    [SerializeField, Range(4, 10)] private int customer_num_restaurant = 6; 
    [Header("�մ� ������")]
    [SerializeField] private Transform customer_startpoint = null;
    [Header("���� Ʈ����(1~6)")] 
    [SerializeField] private Transform[] foodtable_trigger = null;

    private List<GameObject> customer_list_pools = new List<GameObject>();
    private List<int> foodtable_list = new List<int>();
    private List<int> customer_list = new List<int>(); 

    public static CustomerSpawnManager instance = null; //Singleton 

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //PC������ �����Ѵ�: ����ȭ�� ���ؼ� PC������ �ؾ��ϴ°Ŵ� VR������ ������ ���ϵ��� �Ѵ�. 
        if (!XRSettings.enabled)
        {
            for (int i = 0; i < customer_num; ++i)
            {
                if (i >= 0 && i < 2)
                {
                    customer_list_pools.Add(CreateCustomer(0));
                }
                if (i >= 2 && i < 4)
                {
                    customer_list_pools.Add(CreateCustomer(1));
                }
                if (i >= 4 && i < 6)
                {
                    customer_list_pools.Add(CreateCustomer(2));
                }
                if (i >= 6 && i < 8)
                {
                    customer_list_pools.Add(CreateCustomer(3));
                }
            }
            StartCoroutine(SpawnCustomer());
        }
    }

    private GameObject CreateCustomer(int _i)
    {
        GameObject customer_object = Instantiate(customer[_i]);
        customer_object.transform.SetParent(transform);
        customer_object.transform.position = customer_startpoint.position;
        customer_object.gameObject.SetActive(false);  
        return customer_object;
    }

    #region["��������"] 
    public GameObject GetCustomer(int _i)
    {
        GameObject customer_object = customer_list_pools[customer_list[_i]];
        if(!customer_object.activeSelf) //��Ȱ��ȭ�� �մԸ� Ȱ��ȭ�� ��. 
        {
            customer_object.SetActive(true);
            customer_object.GetComponent<Customer>().Move(foodtable_trigger[_i]);
        }
        return customer_object;
    }
    #endregion

    #region["��������"]
    public void FadeCustomer(GameObject _customer, Boolean _isNotMonsterArrived)
    {
       _customer.GetComponentInChildren<Customer>().GoAway(_customer, _isNotMonsterArrived);        
    }
    #endregion  

    #region["�ߺ��� ������ ���� ����"]
    //���� ���̺� 
    private void CreateUnDulpicateRandomFood(int _min, int _max)
    {
        foodtable_list.Clear();
        int currentNumber = Random.Range(_min, _max);
        for(int i=0; i<_max;)
        {
            if(foodtable_list.Contains(currentNumber))
            {
                currentNumber = Random.Range(_min, _max); 
            }
            else
            {
                foodtable_list.Add(currentNumber);
                ++i; 
            }
        }
    }

    //�մ� 
    private void CreateUnDulipicateRandomCustomer(int _min, int _max)
    {
        customer_list.Clear();
        int currentNumber = Random.Range(_min, _max);
        for (int i = 0; i < _max;)
        {
            if (customer_list.Contains(currentNumber))
            {
                currentNumber = Random.Range(_min, _max);
            }
            else
            {
                customer_list.Add(currentNumber);
                ++i;
            }
        }
    }
    #endregion

    #region["����� ���� �� �ٽ� ����ֱ�"] 
    public void ReturnObject(GameObject _customer)
    {
        _customer.SetActive(false);
    }
    #endregion

    #region["�մ� �����ϴ� �ڷ�ƾ"] 
    private IEnumerator SpawnCustomer()
    {
        //���� ����Ʈ�� �̸� ������ ���´� => �ߺ��� ���� ������ ����. 
        //�մ� ������Ʈ ���� �ߺ��ؼ� �����Ǹ� �������� ������ �����... 
        CreateUnDulpicateRandomFood(0, customer_num_restaurant - 1);
        CreateUnDulipicateRandomCustomer(0, customer_num - 1); 
        for (int i = 0; i < customer_num_restaurant; ++i)
        {
            GetCustomer(i);
            yield return new WaitForSeconds(Random.Range(3f, 6f));
        }
        yield break; 
    }
    #endregion

    #region["��� �մԵ��� �� ������"] 
    private IEnumerator FadeCustomerCoroutine()
    {
        for (int i = 0; i < customer_num_restaurant; ++i)
        {
            FadeCustomer(customer_list_pools[i], false); 
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
        yield break;
    }
    #endregion  

    #region["���� �Ĵ� ��ó�� ������"]
    public void IsMonsterArrivedToRestaurant()
    {
        StartCoroutine(FadeCustomerCoroutine()); 
    }
    #endregion

    #region["���� �մ� �ٽ� �ҷ�����"]
    public void EnterAgain()
    {   
        GetCustomer(Random.Range(0, 8)); 
    }
    #endregion 
}
