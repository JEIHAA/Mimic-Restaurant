using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRPlayer : MonoBehaviour, IOnDamage
{
    [Header("최대 공복도")]
    [SerializeField] private int maxHungry = 100;
    [Header("현재 공복도")]
    [SerializeField] private int hungry;
    [Header("초당 줄어드는 공복도")]
    [SerializeField] private int decreaseHungry = 1;
    [Header("플레이어 체력")]
    [SerializeField] private int playerHP = 10;
    [Header("햄버거 공복도 회복량")]
    [SerializeField] private int increaseHungry = 50;
    [SerializeField] private Slider hungryGauge;

    private void Start()
    {
        hungry = maxHungry;
        hungryGauge.value = maxHungry;
        StartCoroutine(HungerDecreaseRoutine());
    }

    private IEnumerator HungerDecreaseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            DecreaseHungry(decreaseHungry);
        }
    }

    public void DecreaseHungry(int amount)
    {
        hungry -= amount;
        hungryGauge.value -= amount;
        if (hungry <= 0)
        {
            hungry = 0;
            hungryGauge.value = 0;
        }
    }


    private void IncreaseHungry(int amount)
    {
        hungry += amount;
        hungryGauge.value += amount;
        if (hungry > maxHungry)
        {
            hungry = maxHungry;
            hungryGauge.value = maxHungry;
        }
        //Debug.Log("Hungry increased by: " + amount + ". Current hungry: " + hungry);
    }


    public void OnDamage(int damage)
    {
        //플레이어는 고기를 잃어버린다. 
        MeatManager.instance.LoseMeatByMonster(); 
        playerHP -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hamburger"))
        {
            IncreaseHungry(increaseHungry);
           // Debug.Log("hamburger +20 Current hungry: " + hungry);
            other.gameObject.SetActive(false);
        }
    }

    public void UpgardeMaxHungry()
    {
        maxHungry += 100;
        hungryGauge.maxValue = maxHungry;
    }
}
