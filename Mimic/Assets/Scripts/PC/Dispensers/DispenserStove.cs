using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static FoodInfo;

public class DispenserStove : DispenserWait, IDispenser
{
    [SerializeField] private Transform foodGenerator;
    [SerializeField] private GameObject output;
    [SerializeField] private bool isGenerate = false;

    private float timer_wait = 3f;

    [HideInInspector] public static DispenserStove instance = null; 
    protected override void Awake()
    {
        base.Awake();
        instance = this; 
    }

    public void UpgradeTimer(float _timer)
    {
        if(timer_wait > 0)
        {
            timer_wait -= (_timer * timer_wait); 
        }
    }

    public bool GetIsGenerate() 
    {
        return isGenerate;
    }

    public void OperateDispenser(GameObject _player)
    {
        Debug.Log(this.name + "사용");
        GameObject food = _player.GetComponentInChildren<BindFood>().Food;

        if (food == null && output != null)
        {
            Debug.Log("음식 가져감");
            _player.GetComponentInChildren<BindFood>().Food = output;
            output = null;
            return;
        }
        else if (food == null) 
        {
            Debug.Log("재료가 필요합니다!");
            return;
        }
        else if (isGenerate)
        {
            Debug.Log("이미 사용중입니다!");
            return;
        }
        else if (food.GetComponent<Ingredients>() == null || food.GetComponent<Ingredients>().State != CookState.Raw)
        {
            Debug.Log("구울 수 없습니다!");
            return;
        }
        else if (output == null)
        {
            Debug.Log("조리 시작");
            isGenerate = true;
            _player.GetComponentInChildren<BindFood>().Food = null;
            StartCoroutine(GenerateFood(food));
        }
    }

    public IEnumerator GenerateFood(GameObject _food)
    {
        Debug.Log("굽기");
        if(_food.GetComponent<Ingredients>().State == CookState.Raw)
        {
            EffectAudioManager.instance.PlayEffect("RoastingMeat", true);
            _food.GetComponent<Ingredients>().State = CookState.Cooking;
            _food.transform.transform.parent = null;
            _food.transform.transform.position = foodGenerator.position;
            Debug.Log("음식 내려놓음");

            StartCoroutine(WaitTimer(timer_wait)); 
            yield return new WaitForSeconds(timer_wait);

            _food.GetComponent<Ingredients>().State = CookState.Cooked;
            Destroy(_food);
            output = Instantiate(_food.GetComponent<Ingredients>().NextLevel, foodGenerator.position, Quaternion.Euler(-90f, 0, 0) );
            isGenerate = false;

            StartCoroutine(WaitTimer(timer_wait * 2f, output));
            //yield return new WaitForSeconds(timer_wait*2f);
            EffectAudioManager.instance.PlayEffect("RoastingMeat", false); 
        }
    }

    
}
