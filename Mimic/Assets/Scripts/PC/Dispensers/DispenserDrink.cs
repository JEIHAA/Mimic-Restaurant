using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DispenserDrink : DispenserWait, IDispenser 
{
    [SerializeField] private Transform foodGenerator;
    [SerializeField] private GameObject outputPrefab;
    [SerializeField] private GameObject output;
    [SerializeField] private bool isGenerate = false;

    private float timer_wait = 3f;

    public static DispenserDrink instance = null; 
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

        if (food != null)
        {
            Debug.Log("손이 비어있어야합니다");
            return;
        }

        if (output != null)
        {
            Debug.Log("음식 가져감");
            _player.GetComponentInChildren<BindFood>().Food = output;
            output = null;
            return;
        }
        else if (isGenerate)
        {
            Debug.Log("이미 사용중입니다!");
            return;
        }
        else if (output == null)
        {
            Debug.Log("조리 시작");
            isGenerate = true;
            StartCoroutine(GenerateFood(outputPrefab));
        }
    }

    public IEnumerator GenerateFood(GameObject _outputPrefab)
    {
        Debug.Log("음료 뽑는 중...");
        StartCoroutine(WaitTimer(timer_wait)); 
        yield return new WaitForSeconds(timer_wait);

        output = Instantiate(_outputPrefab, foodGenerator.position, Quaternion.identity);

        isGenerate = false;
    }

}
