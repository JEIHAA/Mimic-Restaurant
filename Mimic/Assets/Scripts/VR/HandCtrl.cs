using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Rendering.DebugUI;

public class HandCtrl : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerActionProperty;
    [SerializeField] private InputActionProperty leftgripActionProperty;
    [SerializeField] private InputActionProperty rightgripActionProperty;
    [SerializeField] private GameObject gun;
    [SerializeField] private Gun gunComponent;
    [SerializeField] private Animator handAnim;
    private bool isColliding = false;
    private bool triggerPressed = false;
    private bool pistolOn = false;

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
        float leftValue = leftgripActionProperty.action.ReadValue<float>();
        float rightValue = rightgripActionProperty.action.ReadValue<float>();

        if ((pistolOn && leftValue > 0.8f && !isColliding) || (rightValue > 0.8f && !isColliding))
        {
            gun.SetActive(true);
        }
        else
        {
            gun.SetActive(false);
        }
    }

   /* private void LeftGripButton()
    {
        float leftValue = leftgripActionProperty.action.ReadValue<float>();
        if (leftValue > 0.8f)
        {
            gun.SetActive(true);
        }
        else
        {
            gun.SetActive(false);
        }
    }
     private void RightGripButton()
     {
         float rightValue = rightgripActionProperty.action.ReadValue<float>();
         if (rightValue > 0.8f && isColliding == false)
         {
             gun.SetActive(true);
         }
         else
         {
             gun.SetActive(false);
         }
     }*/

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
        float leftValue = leftgripActionProperty.action.ReadValue<float>();
        float rightValue = rightgripActionProperty.action.ReadValue<float>();
        handAnim.SetFloat("LeftGrip", leftValue);
        handAnim.SetFloat("RightGrip", rightValue);
    }

    public void DoubleGun()
    {
        pistolOn = true;
    }

}
