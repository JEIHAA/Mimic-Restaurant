using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class Customer : MonoBehaviour
{
    //애니메이션 상태값 
    public enum Status
    {
        Idle,
        Walking
    }

    private NavMeshAgent agent = null;
    private Animator animator = null;
    private int animation_status = 0;
    private bool IsNotArrivedToFood = true;
    private bool IsNotArrivedToStartpoint = true;
    private bool TotallyFoodGetted = false; //음식을 다 받았을때 
    private bool IstimeOver = false; //타임 오버 

    private GameObject foodtable = null; //자기 자신의 식판
    
    private GameObject food_getted = null; //받은 음식 
    private FoodInfo foodinfo = new FoodInfo(); //음식 정보 
    private Transform foodtable_trigger_g = null; //자기 자신의 식판 트리거

    //손님 1명이 낸 돈 총합 
    private int money_per_customer = 0;

    //원하는 음식 리스트 
    private List<String> wantedfood = new List<String>();

    [SerializeField, Range(1, 5)] private int max_foodnum = 3;
    [SerializeField] private GameObject speechbubble = null;
    [SerializeField] private TextMeshProUGUI speechbubble_text = null; 

    #region["활성화되었을때 실행됨"] 
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //초기화 
        IsNotArrivedToFood = true;
        IsNotArrivedToStartpoint = true;
        TotallyFoodGetted = false;
        IstimeOver = false; 
        foodtable_trigger_g = null;
        foodtable = null;
        wantedfood.Clear(); 
        money_per_customer = 0; 
        animator.SetTrigger("Walking");
        animation_status = (int)Status.Walking;

        SetWantedFoodList();
        speechbubble.SetActive(false); 
    }
    #endregion
    

    #region["원하는 음식 정하기"] 
    private void SetWantedFoodList()
    {
        int foodnum = Random.Range(1, max_foodnum + 1);

        for (int i = 0; i < foodnum; ++i)
        {
            //음식 이름을 랜덤으로 가져와서 추가 
            wantedfood.Add(foodinfo.GetRandomFoodName());
        }
    }
    #endregion


    #region["말풍선 출력"]
    private void SpeechBubble()
    {
        speechbubble.SetActive(true);
        Vector3 transform_screenpos = Camera.main.WorldToViewportPoint(transform.position);
        transform_screenpos.z = 0f; 
        transform_screenpos.y += 2f;
        speechbubble.GetComponentInChildren<RectTransform>().position = transform_screenpos;
        //테스트용 
        speechbubble_text.text = "Food Count: " + wantedfood.Count + "\r\n"; 
        for (int i = 0; i < wantedfood.Count; ++i)
        {
            speechbubble_text.text += "Food: " + wantedfood[i] + "\r\n";  
        }
    }
    #endregion

    #region["음식 받기"] 
    public void GetFood(GameObject _food)
    {
        food_getted = _food;
    }
    #endregion  

    private IEnumerator GetFoodCoroutine()
    {
        int count = 0; 
        while(IstimeOver == false)
        {
            if(food_getted != null)
            {
                ++count;
                GiveMoney(food_getted); 
            }
            if(count >= wantedfood.Count)
            {
                TotallyFoodGetted = true; 
                break; 
            }
            yield return new WaitForEndOfFrame(); 
        }
        yield break; 
    }

    #region["음식 기다리는 코루틴"] 
    private IEnumerator WaitFoodCoroutine()
    {
        float timer = 0;

        for (int i = 0; i < wantedfood.Count; ++i)
        {
            timer += foodinfo.GetFoodTimer(wantedfood[i]);
        }

        timer = timer / wantedfood.Count;
        timer += 5f; 

        while(timer >= 0f)
        {
            timer -= Time.deltaTime; 
            if(TotallyFoodGetted)
            {
                break; 
            }
            yield return new WaitForEndOfFrame(); 
        }
        if(timer < 0f)
        {
            //시간 초과 
            Debug.Log("Too Late");  
        }
        if(TotallyFoodGetted)
        {
            //제대로 받았음. 
            Debug.Log("Thank you"); 
            //MoneyManager.instance.AddMoney(money_per_customer); 
        }
        yield break; 
    }
    #endregion

    #region["돈 받아서 더하기"] 
    private void GiveMoney(GameObject _food)
    {
        //아마 음식 클래스는 Food 클래스를 부모 클래스로 가져아 할것이라고 봄. 
        //int money = _food.GetComponent<Food>().GetMoney(); 
        //money_per_customer += money; 
    }
    #endregion  

    public void Move(Transform foodtable_trigger)
    {
        StartCoroutine(MoveCustomerAI(foodtable_trigger));
        foodtable_trigger_g = foodtable_trigger;
    }

    private IEnumerator MoveCustomerAI(Transform foodtable_trigger)
    {
        while (IsNotArrivedToFood)
        {
            if (animation_status == (int)Status.Idle)
            {
                Debug.Log("Walking...");
                animation_status = (int)Status.Walking;
                animator.SetTrigger("Walking");
            }
            transform.LookAt(foodtable_trigger);
            agent.SetDestination(foodtable_trigger.position);
            //transform.position = Vector3.MoveTowards(transform.position, foodtable_trigger.position, 5f * Time.deltaTime); 

            yield return new WaitForEndOfFrame();
        }
        yield break;
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.name.Equals(foodtable_trigger_g.name))
        {
            agent.isStopped = true;
            IsNotArrivedToFood = false;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            Vector3 newpos = foodtable_trigger_g.transform.position;
            newpos.z += 3f; 
            transform.position = newpos; 
            foodtable = _collider.gameObject;
            if (animation_status == (int)Status.Walking)
            {
                animator.SetTrigger("Idle");
            }
            SpeechBubble(); //말풍선 출력 
            StartCoroutine(WaitFoodCoroutine()); //손님은 일정한 시간동안 음식을 기다린다. 
            StartCoroutine(GetFoodCoroutine());  //음식 받기 코루틴 
            foodtable.GetComponentInChildren<FoodTable>().OnGetFoodOnClick = GetFood;
        }
        if (_collider.name.Equals("Startpoint"))
        {
            IsNotArrivedToStartpoint = false;
            gameObject.SetActive(false);
        }
    }



    #region["다시 돌아가기"] 
    public void GoAway(GameObject _startpoint, Boolean _isNotMonsterArrived)
    {
        StartCoroutine(GoBackCoroutine(_startpoint, _isNotMonsterArrived));
    }
    #endregion

    #region["다시 돌아가는 코루틴"] 
    private IEnumerator GoBackCoroutine(GameObject _startpoint, Boolean _isNotMonsterArrived)
    {
        agent.isStopped = false; 
        while (IsNotArrivedToStartpoint)
        {
            if (animation_status == (int)Status.Idle)
            {
                Debug.Log("Walking...");
                animation_status = (int)Status.Walking;
                animator.SetTrigger("Walking");
            }
            agent.SetDestination(_startpoint.transform.position);
            yield return new WaitForEndOfFrame();
        }
        if (_isNotMonsterArrived)
        {
            //여기서는 그냥 음식만 받고 나가는 거니까 다시 손님이 들어와야 한다.
            CustomerSpawnManager.instance.EnterAgain();
        }
        yield break;
    }
    #endregion

}
