using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : MonoBehaviour, IOnDamage
{
    [SerializeField] private int maxHungry = 100;
    [SerializeField] private int hungry;
    [SerializeField] private int playerHP = 10;

    private void Start()
    {
        hungry = maxHungry;
        StartCoroutine(HungerDecreaseRoutine());
    }

    private IEnumerator HungerDecreaseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            DecreaseHungry(5);
        }
    }

    private void DecreaseHungry(int amount)
    {
        hungry -= amount;
        if (hungry <= 0)
        {
            hungry = 0;
       // Debug.Log("Hungry decreased by: " + amount + ". Current hungry: " + hungry);
        }
    }

    private void IncreaseHungry(int amount)
    {
        hungry += amount;
        if (hungry > maxHungry)
        {
            hungry = maxHungry;
        }
        Debug.Log("Hungry increased by: " + amount + ". Current hungry: " + hungry);
    }

    public void OnDamage(int damage)
    {
        playerHP -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hamburger"))
        {
            IncreaseHungry(20);
            Debug.Log("hamburger +20 Current hungry: " + hungry);
            Destroy(other.gameObject);
        }
    }
}
