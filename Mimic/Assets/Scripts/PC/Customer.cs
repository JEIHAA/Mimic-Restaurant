using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using Image = UnityEngine.UI.Image;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
    private bool isNotMonsterArrived = false; 

    private GameObject foodtable = null; //자기 자신의 식판
    
    private GameObject food_getted = null; //받은 음식 
    private Transform foodtable_trigger_g = null; //자기 자신의 식판 트리거
    private Collider collider = null;

    //손님 1명이 낸 돈 총합 
    private int money_per_customer = 0;

    //원하는 음식 리스트 
    private List<String> wantedfood = new List<string>();
    private Dictionary<int, Sprite> wantedfood_sprite = new Dictionary<int, Sprite>();

    [SerializeField, Range(1, 5)] private int max_foodnum = 3;

    //말풍선 UI 요소 
    [SerializeField] private Image speechbubble = null; //말풍선 메인 이미지 
    [SerializeField] private Image timerimage = null;   //타이머 이미지 
    [SerializeField] private Image[] foodimage = null; //음식 이미지 
    
    private Transform endpoint = null;
    private OrderInfo orderinfo = null;

    #region["활성화되었을때 실행됨"] 
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>(); 

        //초기화 
        IsNotArrivedToFood = true;
        IsNotArrivedToStartpoint = true;
        isNotMonsterArrived = false; 
        TotallyFoodGetted = false;
        IstimeOver = false; 
        foodtable_trigger_g = null;
        foodtable = null;
        endpoint = null; 
        wantedfood.Clear();
        wantedfood_sprite.Clear(); 
        money_per_customer = 0; 
        collider.enabled = true;
        timerimage.fillAmount = 1;
        timerimage.color = new Color(178f/255f, 234f/255f, 112f/255f);
        SetFoodImageDisable(); 
        speechbubble.gameObject.SetActive(false); 
    }
    #endregion

    private void OnDisable()
    {
        if (isNotMonsterArrived) 
        {
            //여기서는 그냥 음식만 받고 나가는 거니까 다시 손님이 들어와야 한다.
            CustomerSpawnManager.instance.EnterAgain();
        }
    }

    public void SetOrderInfo(OrderInfo _orderinfo)
    {
        orderinfo = _orderinfo; 
    }

    #region["음식 이미지 끄기: 초기화"] 
    private void SetFoodImageDisable()
    {
        for(int i=0; i<foodimage.Length; ++i)
        {
            foodimage[i].sprite = null; 
            foodimage[i].gameObject.SetActive(false); 
        }
    }
    #endregion

    public void SetEndPoint(Transform _endpoint)
    {
        endpoint = _endpoint;
    }

    #region["원하는 음식 정하기"] 
    private void SetWantedFoodList()
    {
        int foodnum = Random.Range(1, max_foodnum + 1);

        for (int i = 0; i < foodnum; ++i)
        {
            //음식 이름을 랜덤으로 가져와서 추가 
            Dictionary<string, Sprite> foodinfo = orderinfo.GetRandomFoodInfo();
            string foodname = foodinfo.Keys.ToList<String>()[0];
            wantedfood.Add(foodname);
            wantedfood_sprite.Add(i, foodinfo[foodname]); 
        }
    }
    #endregion


    #region["말풍선 출력"]
    private void SpeechBubble(Vector3 _newpos) 
    {
        speechbubble.gameObject.SetActive(true);
        SetWantedFoodList();

        for (int i = 0; i < wantedfood.Count; ++i)
        {
            foodimage[i].gameObject.SetActive(true);
            foodimage[i].sprite = wantedfood_sprite[i];
        }
        if (wantedfood.Count == 1)
        {
            foodimage[0].GetComponent<RectTransform>().localPosition = new Vector3(-1.3f, 5.8f, 0f);
        }
        else
        {
            foodimage[0].GetComponent<RectTransform>().localPosition = new Vector3(30.8f, 5.8f, 0f);
            foodimage[1].GetComponent<RectTransform>().localPosition = new Vector3(-31.9f, 5.8f, 0f); 
        }
    }        
    #endregion

    private IEnumerator SpeechBubbleCoroutine()
    {
        while(speechbubble.gameObject.activeSelf)
        {
            yield return new WaitForEndOfFrame(); 
        }
        yield break; 
    }

    #region["음식 받기"] 
    public void GetFood(GameObject _food, GameObject _player)
    {

        for(int i=0; i<wantedfood.Count; ++i)
        {
            if (_food.name.Contains(wantedfood[i]))
            {
                EffectAudioManager.instance.PlayEffect("GiveFood", true); 
                food_getted = _food;
                wantedfood.Remove(wantedfood[i]);
                wantedfood_sprite.Remove(i); 
                foodimage[i].sprite = null;
                foodimage[i].gameObject.SetActive(false);
                switch(i)
                {
                    case 0:
                        foodimage[1].GetComponent<RectTransform>().localPosition = new Vector3(-1.3f, 5.8f, 0f);
                        break; 
                    case 1:
                        foodimage[0].GetComponent<RectTransform>().localPosition = new Vector3(-1.3f, 5.8f, 0f);
                        break;
                    default:
                        break; 
                }
                break; 
            }
            else
            {
                _food.GetComponent<Collider>().enabled = false;
                _food.transform.position = _player.GetComponentInChildren<BindFood>().transform.position;
                _food.transform.SetParent(_player.GetComponentInChildren<BindFood>().transform); 
            }
        }
    }
    #endregion  


    private IEnumerator GetFoodCoroutine()
    {
        int count = 0;
        int wantedfood_num = wantedfood.Count; 
        while(IstimeOver == false)
        {
            if(food_getted != null)
            {
                ++count;
                GiveMoney(food_getted);
                Destroy(food_getted.gameObject);
                food_getted = null; 
                //Photon Maybe? 
            }
            if(count >= wantedfood_num)
            {
                TotallyFoodGetted = true;
                count = 0; 
            }
            yield return new WaitForEndOfFrame(); 
        }
        yield break; 
    }

    #region["음식 기다리는 코루틴"] 
    private IEnumerator WaitFoodCoroutine()
    {
        float timer = 0f; 
        float duration = 0f; 
        for (int i = 0; i < wantedfood.Count; ++i)
        {
            timer += orderinfo.GetFoodTimer(wantedfood[i]);
        }

        //타이머 설정: 주문한 음식의 평균 + 5초 
        timer = timer / wantedfood.Count;
        timer += 5f;

        duration = timer;
        //타이머 비율 설정: 3으로 나눈다. 
        float timer_rate = timer / 3f;

        while(timer >= 0f)
        {
            timer -= Time.deltaTime;
            timerimage.fillAmount = timer / duration; 
            if(timer_rate * 2f > timer)
            {
                timerimage.color = new Color(251f/255f, 209f/255f, 72f/255f); 
            }
            if(timer_rate > timer)
            {
                timerimage.color = new Color(200f / 255f, 92f / 255f, 92f / 255f); 
            }
            if (TotallyFoodGetted)
            {   
                break; 
            }
            yield return new WaitForEndOfFrame(); 
        }
        if(timer < 0f)
        {
            //시간 초과 
            Debug.Log("Too Late");
            //화내는 효과음 재생 
            if(name.Contains("Dog") || name.Contains("Alien"))
            {
                //강아지, 외계인: 흐음 
                EffectAudioManager.instance.PlayEffect("EmotionChange", true); 
            }
            else
            {
                EffectAudioManager.instance.PlayEffect("AngryCustomer", true); 
            }
            CustomerSpawnManager.instance.IncreaseCustomerNotGet(); //받지 못한 손님 명수 증가시키기 
            speechbubble.gameObject.SetActive(false); 
        }
        if(TotallyFoodGetted)
        {
            //제대로 받았음. 
            Debug.Log("Thank you");
            CustomerSpawnManager.instance.IncreaseCustomer(); //받은 손님 명수 증가시키기 
            CustomerSpawnManager.instance.IncreaseMoney(money_per_customer); 
            MoneyManager.instance.AddMoney(money_per_customer); 
            speechbubble.gameObject.SetActive(false); 
        }
        GoAway(endpoint.gameObject, true); 
        yield break; 
    }
    #endregion

    #region["돈 받아서 더하기"] 
    private void GiveMoney(GameObject _food)
    {
        //아마 음식 클래스는 Food 클래스를 부모 클래스로 가져아 할것이라고 봄. 
        int money = _food.GetComponent<FoodInfo>().GetMoney(); 
        money_per_customer += money; 
    }
    #endregion  

    public void Move(Transform foodtable_trigger)
    {
        EffectAudioManager.instance.PlayEffect("BellSound", true);
        StartCoroutine(MoveCustomerAI(foodtable_trigger));
        foodtable_trigger_g = foodtable_trigger;
    }

    private IEnumerator MoveCustomerAI(Transform foodtable_trigger)
    {
        while (IsNotArrivedToFood)
        {
            animation_status = (int)Status.Walking;
            animator.SetTrigger("Walking");
            transform.LookAt(foodtable_trigger);
            agent.SetDestination(foodtable_trigger.position);
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.name.Equals(foodtable_trigger_g.name)) //지정된 카운터에 가까이 갔을때 
        { 
            IsNotArrivedToFood = false;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            Vector3 newpos = foodtable_trigger_g.transform.position;
            newpos.z += Random.Range(0f, 1f); 
            transform.position = newpos; 
            foodtable = _collider.gameObject;
            if (animation_status == (int)Status.Walking)
            {
                animator.SetTrigger("Idle");
            }
            foodtable.GetComponentInParent<FoodTableCookSide>().IsUsedByCustomer = true; 
            foodtable.GetComponentInParent<FoodTableCookSide>().OnGetFoodOnClick = GetFood; 
            collider.enabled = false;
            SpeechBubble(foodtable_trigger_g.transform.position); //말풍선 출력 
            StartCoroutine(SpeechBubbleCoroutine()); //말풍선 관련 COROUTINE 
            StartCoroutine(WaitFoodCoroutine()); //손님은 일정한 시간동안 음식을 기다린다. 
            StartCoroutine(GetFoodCoroutine());  //음식 받기 코루틴 
        }
        if (_collider.name.Equals("EndPoint"))
        {
            IsNotArrivedToStartpoint = false;
        }
    }

    #region["다시 돌아가기"] 
    public void GoAway(GameObject _startpoint, Boolean _isNotMonsterArrived)
    {
        StartCoroutine(GoBackCoroutine(_startpoint, _isNotMonsterArrived));
        isNotMonsterArrived = _isNotMonsterArrived;
    }
    #endregion

    #region["다시 돌아가는 코루틴"] 
    private IEnumerator GoBackCoroutine(GameObject _startpoint, Boolean _isNotMonsterArrived)
    {
        while (IsNotArrivedToStartpoint)
        {
            animation_status = (int)Status.Walking;
            animator.SetTrigger("Walking");
            float distance = (transform.position - _startpoint.transform.position).magnitude;
            if(distance < 4f)
            {
                collider.enabled = true;
            }
            foodtable.GetComponentInParent<FoodTableCookSide>().IsUsedByCustomer = false; 
            agent.SetDestination(_startpoint.transform.position);
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false); 
        yield break;
    }
    #endregion
    
}
