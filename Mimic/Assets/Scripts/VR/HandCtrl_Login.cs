using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class HandCtrl_Login : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerActionProperty;
    [SerializeField] private InputActionProperty gripActionProperty;
    private Animator handAnim;
    private bool triggerPressed = false;

    private void Awake()
    {
        handAnim = GetComponent<Animator>();
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
    }

    private void TriggerButton()
    {
        float tvalue = triggerActionProperty.action.ReadValue<float>();
        if (tvalue > 0.9f)
        {
         
        }
        else
        {
            triggerPressed = false;
        }
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
