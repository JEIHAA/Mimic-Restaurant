using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour, IOnDamage
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int barrierHP;
    public int BarrierHP => barrierHP;

    private void Start()
    {
        barrierHP = maxHP;
    }

    public void OnDamage(int damage)
    {
        barrierHP -= damage;
        if (barrierHP <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void RepairingBarrier(int value) 
    {
        barrierHP += value;
        this.gameObject.SetActive(true);
        if (barrierHP > maxHP)
        {
            barrierHP = maxHP;
        }
    }

    
}
