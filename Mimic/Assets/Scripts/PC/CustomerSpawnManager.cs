using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class CustomerSpawnManager : MonoBehaviour
{
    [Header("손님 배열: 외계인, 외계인2, 강아지, 강아지2")]
    [SerializeField] private GameObject[] customer = null;
    [Header("손님 명수")]
    [SerializeField, Range(6, 10)] private int customer_num = 8;
    [Header("식당에 들어올 수 있는 손님 명수")]
    [SerializeField, Range(4, 10)] private int customer_num_restaurant = 6;
    [Header("손님 시작점")]
    [SerializeField] private Transform customer_startpoint = null;
    [Header("손님 나가는 지점")]
    [SerializeField] private Transform customer_endpoint = null;
    [Header("식판 트리거(1~5)")]
    [SerializeField] private Transform[] foodtable_trigger = null;
    [SerializeField] private OrderInfo orderinfo = null;

    private List<GameObject> customer_list_pools = new List<GameObject>();
    private List<int> foodtable_list = new List<int>();
    private List<int> customer_list = new List<int>();
    private Boolean isBreakTime = false;

    public static CustomerSpawnManager instance = null; //Singleton 

    //나중에 정산 UI에 출력할 정보들임. 
    private int customer_getted_num = 0;
    private int customer_notgetted_num = 0;
    private int money_getted = 0;

    #region["받은 손님 명수 증가시키기"] 
    public void IncreaseCustomer()
    {
        ++customer_getted_num; 
    }
    #endregion

    #region["받지 못한 손님 명수 증가시키기"]
    public void IncreaseCustomerNotGet()
    {
        ++customer_notgetted_num; 
    }
    #endregion

    #region["돈을 받을 때마다 이 메소드가 실행"]
    public void IncreaseMoney(int _money)
    {
        money_getted += _money; 
    }
    #endregion

    #region["받은 손님 명수 돌려주기"] 
    public int GetCustomerNum()
    {
        return customer_getted_num; 
    }
    #endregion

    #region["받지 못한 손님 명수 돌려주기"]
    public int NotGetCustomerNum()
    {
        return customer_notgetted_num; 
    }
    #endregion

    #region["돈 돌려주기"]
    public int GetMoney()
    {
        return money_getted; 
    }
    #endregion

    #region["PC 정산 정보 초기화하기"] 
    public void ClearCustomer()
    {
        customer_getted_num = 0;
        customer_notgetted_num = 0;
        money_getted = 0; 
    }
    #endregion

    private void Awake()
    {
        instance = this;
    }

    public void StartSpawnCustomer()
    {
        //초기 손님 오브젝트 풀 생성 
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


    #region["초기 손님 오브젝트 풀 생성"] 
    private GameObject CreateCustomer(int _i)
    {
        GameObject customer_object = Instantiate(customer[_i]);
        customer_object.transform.SetParent(transform);
        customer_object.transform.position = customer_startpoint.position;
        customer_object.gameObject.SetActive(false);
        return customer_object;
    }
    #endregion

    #region["손님 데려오기"] 
    public GameObject GetCustomer(int _i)
    {
        GameObject customer_object = null;
        if (isBreakTime == false)
        {
            customer_object = customer_list_pools[customer_list[_i]];
            if (!customer_object.activeSelf) //비활성화된 손님만 활성화할 것. 
            {
                customer_object.transform.position = customer_startpoint.position;
                customer_object.SetActive(true);
                customer_object.GetComponent<Customer>().SetEndPoint(customer_endpoint);
                customer_object.GetComponent<Customer>().Move(foodtable_trigger[_i]);
                customer_object.GetComponent<Customer>().SetOrderInfo(orderinfo); 
            }
            else
            {
                if (_i < customer_num_restaurant - 1)
                {
                    GetCustomer(_i + 1);
                }
            }
        }
        return customer_object;
    }
    #endregion

    #region["손님 내보내기"]
    public void FadeCustomer(GameObject _customer, Boolean _isNotMonsterArrived)
    {
        _customer.GetComponentInChildren<Customer>().GoAway(_customer, _isNotMonsterArrived);
    }
    #endregion  

    #region["중복을 배제한 랜덤 생성"]
    //음식 테이블 
    private void CreateUnDulpicateRandomFood(int _min, int _max)
    {
        foodtable_list.Clear();
        int currentNumber = Random.Range(_min, _max);
        for (int i = 0; i < _max;)
        {
            if (foodtable_list.Contains(currentNumber))
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

    //손님 
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

    #region["손님 스폰하는 코루틴"] 
    private IEnumerator SpawnCustomer()
    {
        //랜덤 리스트를 미리 생성해 놓는다 => 중복값 생성 방지를 위함. 
        //손님 오브젝트 또한 중복해서 생성되면 겹쳐져서 한쪽이 사라짐... 
        CreateUnDulpicateRandomFood(0, customer_num_restaurant - 1);
        CreateUnDulipicateRandomCustomer(0, customer_num - 1);
        for (int i = 0; i < customer_num_restaurant; ++i)
        {
            GetCustomer(i);
           
            yield return new WaitForSeconds(Random.Range(4f, 8f));
        }
        yield break;
    }
    #endregion

    /*
    #region["모든 손님들을 다 내보냄"] 
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
    */

    /*
    #region["적이 식당 근처에 왔을때"]
    public void IsMonsterArrivedToRestaurant()
    {
        StartCoroutine(FadeCustomerCoroutine()); 
    }
    #endregion
    */

    #region["나간 손님 다시 불러오기"]
    public void EnterAgain()
    {
        if (isBreakTime == false)
        {
            StartCoroutine(EnterAgainCoroutine());
        }
    }
    #endregion

    #region["손님 다시 돌아오는 코루틴"]
    public IEnumerator EnterAgainCoroutine()
    {
        yield return new WaitForEndOfFrame();
        GetCustomer(Random.Range(0, 5));
        yield break;
    }
    #endregion

    public void BreakTime()
    {
        //쉬는 시간에는 새로운 손님을 받지 않음. 
        isBreakTime = true;
    }

    public void Restart()
    {
        //쉬는시간 끝
        isBreakTime = false;
        StartCoroutine(SpawnCustomer()); 
    }
}
