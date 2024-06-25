using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class HandCtrl : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerActionProperty;
    [SerializeField] private InputActionProperty gripActionProperty;
    [SerializeField] private GameObject gun;
    [SerializeField] private Gun gunComponent;
    private Animator handAnim;
    private bool isColliding = false;
    private bool triggerPressed = false;

    private void Awake()
    {
        handAnim = GetComponent<Animator>();
        gun.SetActive(false);
    }

    private void Update()
    {
        GripButton();
        TriggerButton();

        GripAnim();
        TriggerAnim();
    }

    private void GripButton()
    {
        float value = gripActionProperty.action.ReadValue<float>();
        Debug.Log("grip value: " + value); 
        if (value > 0.8f && isColliding == false)
        {
            gun.SetActive(true);
        }
        else
        {
            gun.SetActive(false);
        }
    }

    private void TriggerButton()
    {
        float tvalue = triggerActionProperty.action.ReadValue<float>();
        if (tvalue > 0.9f)
        {
            if (gun.activeSelf && !triggerPressed)
            {
                gunComponent.ShootBullet();
                triggerPressed = true ; 
            }
        }
        else
        {
            triggerPressed = false; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }

    private void TriggerAnim()
    {
        float value = triggerActionProperty.action.ReadValue<float>();
        handAnim.SetFloat("Trigger", value);
    }

    private void GripAnim()
    {
        float value = gripActionProperty.action.ReadValue<float>();
        handAnim.SetFloat("Grip", value);
    }
}
