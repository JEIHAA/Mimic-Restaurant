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
   // private bool triggerPressed = false;  
    private bool pistolOn = false;
    private Coroutine shootingCoroutine;
    [SerializeField] private float shootingSpeed = 1f;
    public VRPlayer vrPlayer;

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

        if((leftValue > 0.8f && !isColliding) || (rightValue > 0.8f && !isColliding))
        {
            gun.SetActive(true);
        }
        else
        {
            gun.SetActive(false);
        }
    }


    /* private void TriggerButton()
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
     }*/

    private void TriggerButton()
    {
        float tvalue = triggerActionProperty.action.ReadValue<float>();
        if (tvalue > 0.9f && gun.activeSelf && vrPlayer.hungry > 0)
        {
            if (shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootCoroutine());
            }
        }
        else
        {
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            gunComponent.ShootBullet();
            yield return new WaitForSeconds(1f/shootingSpeed); 
        }
    }

    public void UpgradeShootingSpeed()
    {
        shootingSpeed += 1f;
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
