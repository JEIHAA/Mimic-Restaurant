using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    //음식 
    public enum Food
    {
        Hamburger,
        FrenchFries,
        Drink 
    }

    //햄버거
    public enum Hamburger
    {
        Hamburger,        //햄버거 
        Cheeseburger,     //치즈햄버거 
        Shrimpburger,     //새우햄버거 
        Octopusburger     //문어햄버거 
    }

    //감자튀김
    public enum FrenchFries
    {
        FrenchFries,     //감자튀김
        CheeseFries,     //치즈 감자튀김 
        VolcanoFries     //볼케이노 감자튀김 
    }

    //음료수
    public enum Drink
    {
        Cola,             //콜라
        CiderDrink,       //사이다(사과술 아님) 
        EnergyDrink,      //에너지 음료 
        WaterMelonDrink   //수박 음료 
    }

    
    private NavMeshAgent agent = null;
    private Animator animator = null; 
    private int animation_status = 0;
    private bool IsNotArrivedToFood = true;
    private bool IsNotArrivedToStartpoint = true; 
    private GameObject foodtable = null; //자기 자신의 식판
    private Collider collider = null;
    private Transform foodtable_trigger_g = null;

    //원하는 음식 리스트 
    private List<int> wantedfood_list = new List<int>();

    //음식
    private int food = 0;

    //햄버거 
    private int hamburger = 0;
    //감자튀김 
    private int frenchfries = 0;
    //콜라 
    private int drink = 0;

    //테스트용
    [SerializeField] private float radius = 1f;
    [SerializeField] int resolution = 10;
    [SerializeField] float maxDistance = 100f;


    private void OnEnable()
    { 
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();

        SetWantedFoodList(); 
    }


    #region["Start is called before the first frame update"] 
    private void Start()
    {
        //StartCoroutine(MoveCustomerAI()); 
         
    }
   #endregion


    #region["Update is called once per frame"] 
    private void Update()
    {
        
    }
    #endregion

    public void SetWantedFoodList()
    {
        int foodnum = Random.Range(0, 2);
        //0, 1, 2 => 1, 2, 3 

        for(int i=0; i<foodnum; ++i)
        {
            
        }
    }

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
            
            Debug.Log("name: " + name + "_foodtable_trigger: " + foodtable_trigger.gameObject.name);
            yield return new WaitForEndOfFrame(); 
        }
        Debug.Log("Break..."); 
        yield break; 
    }

    /*
    private GameObject PerformAllDirectionsRaycast()
    {
        //출처: ChatGPT 
        // resolution^2 만큼의 방향을 샘플링
        GameObject foodtable = null; 
        for (int thetaIndex = 0; thetaIndex < resolution; thetaIndex++)
        {
            for (int phiIndex = 0; phiIndex < resolution; phiIndex++)
            {
                float theta = Mathf.PI * thetaIndex / resolution;
                float phi = 2 * Mathf.PI * phiIndex / resolution;

                Vector3 direction = new Vector3(
                    Mathf.Sin(theta) * Mathf.Cos(phi),
                    Mathf.Sin(theta) * Mathf.Sin(phi),
                    Mathf.Cos(theta)
                );

                Ray ray = new Ray(transform.position, direction);
                RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);
                foreach (RaycastHit hit in hits)
                {
                    Debug.DrawRay(transform.position, direction * 15f, Color.yellow);
                    if (hit.collider.CompareTag("FoodTable"))
                    {
                        foodtable = hit.collider.gameObject;
                        break; 
                    }
                }
            }
        }
        return foodtable; 
    }
    */ 

    private void OnTriggerEnter(Collider _collider)
    {
        Debug.Log("Triggered: " + _collider.gameObject.name); 

        if(_collider.name.Equals(foodtable_trigger_g.name))
        {
            collider.enabled = false;
            IsNotArrivedToFood = false; 
            Debug.Log("Stop!!!");
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            foodtable = _collider.gameObject;
            if (animation_status == (int)Status.Walking)
            {
                animator.SetTrigger("Idle"); 
            }
            //foodtable.transform.SetParent(transform); //자신의 식판을 자식으로 설정. 
        }
        if(_collider.name.Equals("Startpoint"))
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
        while(IsNotArrivedToStartpoint)
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
        if(_isNotMonsterArrived)
        {
            //여기서는 그냥 음식만 받고 나가는 거니까 다시 손님이 들어와야 한다.
            CustomerSpawnManager.instance.EnterAgain(); 
        }
        yield break; 
    }
    #endregion

}
