using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Events;
public class VRBombBtn : MonoBehaviour
{
    [SerializeField] private int price = 1000;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float radius = 5f;
    [SerializeField] private Collider[] monsters;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private GameObject boomUI;
    public GameObject button;
    GameObject presser;
    bool isPressed;


    private void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.004f, 0);
            presser = other.gameObject;
            isPressed = true;
               // MegaMegaBomb();
/*            if (MoneyManager.instance.MoneyCheck(price)) 
            {
            }*/
        }       
        if(isPressed)
        {
            boomUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            isPressed = false;
            boomUI.SetActive(false);
        }
    }

    public void MegaMegaBomb() 
    {
        monsters = Physics.OverlapSphere(transform.position, radius, layer);
        foreach (Collider monster in monsters) 
        {
            monster.GetComponent<IOnDamage>()?.OnDamage(9999, monster.gameObject);
        }
        MoneyManager.instance.MinusMoney(price);
        particle.Play();
    }

}
