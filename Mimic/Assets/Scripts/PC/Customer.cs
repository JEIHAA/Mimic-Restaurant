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
    //�ִϸ��̼� ���°� 
    public enum Status
    {
        Idle,
        Walking
    }

    //���� 
    public enum Food
    {
        Hamburger,
        FrenchFries,
        Drink 
    }

    //�ܹ���
    public enum Hamburger
    {
        Hamburger,        //�ܹ��� 
        Cheeseburger,     //ġ���ܹ��� 
        Shrimpburger,     //�����ܹ��� 
        Octopusburger     //�����ܹ��� 
    }

    //����Ƣ��
    public enum FrenchFries
    {
        FrenchFries,     //����Ƣ��
        CheeseFries,     //ġ�� ����Ƣ�� 
        VolcanoFries     //�����̳� ����Ƣ�� 
    }

    //�����
    public enum Drink
    {
        Cola,             //�ݶ�
        CiderDrink,       //���̴�(����� �ƴ�) 
        EnergyDrink,      //������ ���� 
        WaterMelonDrink   //���� ���� 
    }

    
    private NavMeshAgent agent = null;
    private Animator animator = null; 
    private int animation_status = 0;
    private bool IsNotArrivedToFood = true;
    private bool IsNotArrivedToStartpoint = true; 
    private GameObject foodtable = null; //�ڱ� �ڽ��� ����
    private Collider collider = null;
    private Transform foodtable_trigger_g = null;

    //���ϴ� ���� ����Ʈ 
    private List<int> wantedfood_list = new List<int>();

    //����
    private int food = 0;

    //�ܹ��� 
    private int hamburger = 0;
    //����Ƣ�� 
    private int frenchfries = 0;
    //�ݶ� 
    private int drink = 0;

    //�׽�Ʈ��
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
        //��ó: ChatGPT 
        // resolution^2 ��ŭ�� ������ ���ø�
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
            //foodtable.transform.SetParent(transform); //�ڽ��� ������ �ڽ����� ����. 
        }
        if(_collider.name.Equals("Startpoint"))
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
            //���⼭�� �׳� ���ĸ� �ް� ������ �Ŵϱ� �ٽ� �մ��� ���;� �Ѵ�.
            CustomerSpawnManager.instance.EnterAgain(); 
        }
        yield break; 
    }
    #endregion

}
