using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barrier : MonoBehaviour, IOnDamage
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int barrierHP;
    [SerializeField] private Slider barrierGauge;
    public int BarrierHP => barrierHP;

    private void Start()
    {
        barrierHP = maxHP;
        barrierGauge.value = maxHP;
    }

    public void OnDamage(int damage, GameObject _object)
    {
        barrierHP -= damage;
        barrierGauge.value -= damage;
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
