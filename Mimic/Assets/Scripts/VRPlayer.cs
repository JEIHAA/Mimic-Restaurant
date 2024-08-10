using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class VRPlayer : MonoBehaviour, IOnDamage
{
    [Header("최대 공복도")]
    [SerializeField] private int maxHungry = 100;
    [Header("현재 공복도")]
    [SerializeField] private int hungry;
    public int Hungry { get { return hungry; } set { hungry = value; } }
    [Header("총알 소모 에너지양")]
    [SerializeField] private int decreaseHungry = 1;
    public int decreasehungry => decreaseHungry;
    [Header("플레이어 체력, 최대 체력")]
    [SerializeField] private int playerHP = 10;
    [SerializeField] private int playerMaxHP = 200;
    //[Header("햄버거 공복도 회복량")]
    //[SerializeField] private int increaseHungry = 50;
    [SerializeField] private Slider hungryGauge;
    [SerializeField] private Slider hpGauge;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private VRBombBtn bomb;
    [SerializeField] private int medicalPrice = 500;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private Barrier barrier;
    [SerializeField] XROrigin xrorigin;

    private void Start()
    {
        hungry = maxHungry;
        hungryGauge.value = maxHungry;
        //StartCoroutine(HungerDecreaseRoutine());
        playerHP = playerMaxHP;
        hpGauge.value = playerMaxHP;
    }

    private void Update()
    {
        xrorigin.transform.position = new Vector3(0f, 2f, -0.6f);
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
        hpGauge.value -= damage;
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
        StartCoroutine(PlayerReborn());
    }
    private IEnumerator PlayerReborn()
    {
        yield return new WaitForSeconds(1f);
        playerHP = playerMaxHP;
        hpGauge.value = playerMaxHP;
        barrier.BarrierHP = barrier.MaxHP;
        barrier.BarrierGauge.value = barrier.MaxHP;
        particle.Play();
        //barrier.gameObject.SetActive(true);
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
            decreaseHungry *= 1/2;
    }
}
