using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Barrier : MonoBehaviour, IOnDamage
{
    [SerializeField] private int maxHP = 1500;
    public int MaxHP => maxHP;
    [SerializeField] private int barrierHP;
    public int BarrierHP { get { return barrierHP; } set { barrierHP = value; } }
    [SerializeField] private Slider barrierGauge;
    public Slider BarrierGauge => barrierGauge;

    private void Start()
    {
        barrierHP = maxHP;
        if(XRSettings.enabled)
        {
            barrierGauge.value = maxHP;
        }
    }

    public void OnDamage(int damage, GameObject _object)
    {
        barrierHP -= damage;
        if(XRSettings.enabled)
        {
            barrierGauge.value -= damage;
        }
        if (barrierHP <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void RepairingBarrier(int value) 
    {
        barrierHP += value;
        barrierGauge.value += value;
        this.gameObject.SetActive(true);
        if (barrierHP > maxHP)
        {
            barrierHP = maxHP;
            barrierGauge.value = maxHP;
        }
    }

    
}
