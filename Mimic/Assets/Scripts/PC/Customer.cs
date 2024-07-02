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
    //�ִϸ��̼� ���°� 
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
    private bool TotallyFoodGetted = false; //������ �� �޾����� 
    private bool IstimeOver = false; //Ÿ�� ���� 

    private GameObject foodtable = null; //�ڱ� �ڽ��� ����
    
    private GameObject food_getted = null; //���� ���� 
    private FoodInfo foodinfo = new FoodInfo(); //���� ���� 
    private Transform foodtable_trigger_g = null; //�ڱ� �ڽ��� ���� Ʈ����

    //�մ� 1���� �� �� ���� 
    private int money_per_customer = 0;

    //���ϴ� ���� ����Ʈ 
    private List<String> wantedfood = new List<String>();

    [SerializeField, Range(1, 5)] private int max_foodnum = 3;
    [SerializeField] private GameObject speechbubble = null;
    [SerializeField] private TextMeshProUGUI speechbubble_text = null; 

    #region["Ȱ��ȭ�Ǿ����� �����"] 
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //�ʱ�ȭ 
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
    

    #region["���ϴ� ���� ���ϱ�"] 
    private void SetWantedFoodList()
    {
        int foodnum = Random.Range(1, max_foodnum + 1);

        for (int i = 0; i < foodnum; ++i)
        {
            //���� �̸��� �������� �����ͼ� �߰� 
            wantedfood.Add(foodinfo.GetRandomFoodName());
        }
    }
    #endregion


    #region["��ǳ�� ���"]
    private void SpeechBubble()
    {
        speechbubble.SetActive(true);
        Vector3 transform_screenpos = Camera.main.WorldToViewportPoint(transform.position);
        transform_screenpos.z = 0f; 
        transform_screenpos.y += 2f;
        speechbubble.GetComponentInChildren<RectTransform>().position = transform_screenpos;
        //�׽�Ʈ�� 
        speechbubble_text.text = "Food Count: " + wantedfood.Count + "\r\n"; 
        for (int i = 0; i < wantedfood.Count; ++i)
        {
            speechbubble_text.text += "Food: " + wantedfood[i] + "\r\n";  
        }
    }
    #endregion

    #region["���� �ޱ�"] 
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

    #region["���� ��ٸ��� �ڷ�ƾ"] 
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
            //�ð� �ʰ� 
            Debug.Log("Too Late");  
        }
        if(TotallyFoodGetted)
        {
            //����� �޾���. 
            Debug.Log("Thank you"); 
            //MoneyManager.instance.AddMoney(money_per_customer); 
        }
        yield break; 
    }
    #endregion

    #region["�� �޾Ƽ� ���ϱ�"] 
    private void GiveMoney(GameObject _food)
    {
        //�Ƹ� ���� Ŭ������ Food Ŭ������ �θ� Ŭ������ ������ �Ұ��̶�� ��. 
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
            SpeechBubble(); //��ǳ�� ��� 
            StartCoroutine(WaitFoodCoroutine()); //�մ��� ������ �ð����� ������ ��ٸ���. 
            StartCoroutine(GetFoodCoroutine());  //���� �ޱ� �ڷ�ƾ 
            foodtable.GetComponentInChildren<FoodTable>().OnGetFoodOnClick = GetFood;
        }
        if (_collider.name.Equals("Startpoint"))
        {
            IsNotArrivedToStartpoint = false;
            gameObject.SetActive(false);
        }
    }



    #region["�ٽ� ���ư���"] 
    public void GoAway(GameObject _startpoint, Boolean _isNotMonsterArrived)
    {
        StartCoroutine(GoBackCoroutine(_startpoint, _isNotMonsterArrived));
    }
    #endregion

    #region["�ٽ� ���ư��� �ڷ�ƾ"] 
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
            //���⼭�� �׳� ���ĸ� �ް� ������ �Ŵϱ� �ٽ� �մ��� ���;� �Ѵ�.
            CustomerSpawnManager.instance.EnterAgain();
        }
        yield break;
    }
    #endregion

}
