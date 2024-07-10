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
    public int Hungry { get { return hungry; } set { hungry = value; } }
    [Header("총알 소모 에너지양")]
    [SerializeField] private int decreaseHungry = 5;
    public int decreasehungry => decreaseHungry;
    [Header("플레이어 체력, 최대 체력")]
    [SerializeField] private int playerHP = 10;
    [SerializeField] private int playerMaxHP = 200;
    [Header("햄버거 공복도 회복량")]
    [SerializeField] private int increaseHungry = 50;
    public int increasehungry => increaseHungry;
    [SerializeField] private Slider hungryGauge;
    [SerializeField] private VRBombBtn bomb;
    [SerializeField] private int medicalPrice = 500;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private Barrier barrier;

    private void Start()
    {
        hungry = maxHungry;
        hungryGauge.value = maxHungry;
        //StartCoroutine(HungerDecreaseRoutine());
        playerHP = playerMaxHP;
    }

/*    private IEnumerator HungerDecreaseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            DecreaseHungry(decreaseHungry);
        }
    }*/

    public void DecreaseHungry(int amount)
    {
        hungry -= amount;
        hungryGauge.value -= amount;
        if (hungry <= 0)
        {
            hungry = 0;
            hungryGauge.value = 0;
            // Debug.Log("Hungry decreased by: " + amount + ". Current hungry: " + hungry);
        }
    }

    public void IncreaseHungry(int amount)
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

    public void OnDamage(int damage, GameObject _monster)
    {
         playerHP -= damage;
        //StartCoroutine(HitEffect());
        if (playerHP <= 0)
        {
            PlayerDead();
        }
    }

    private void PlayerDead() 
    {
        bomb.MegaMegaBomb();
        MoneyManager.instance.MinusMoney(medicalPrice);
        playerHP = playerMaxHP;
        barrier.BarrierHP = barrier.MaxHP;
    }

    private IEnumerator HitEffect()
    {
        hitEffect.SetActive(false);
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitEffect.SetActive(false);
    }

    public void GunEnergyUpgrade()
    {
        if (decreaseHungry > 1)
        {
            decreaseHungry -= 1;
        }
    }
}
