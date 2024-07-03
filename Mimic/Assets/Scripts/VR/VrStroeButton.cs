using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Events;

public class VrStroeButton : MonoBehaviour
{
    [SerializeField] private GameObject storeMenu;
    public GameObject button;
    GameObject presser;
    bool isPressed;

    private void Start()
    {
        storeMenu.SetActive(false);
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.004f, 0);
            presser = other.gameObject;
            isPressed = true;
        }
        if(!storeMenu.activeSelf)
        {
            VRStoreOn();
        }
        else if(storeMenu.activeSelf)
        {
            VRStoreOff();
        }
                
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            isPressed = false;
        }
    }

    public void VRStoreOn()
    {
        storeMenu.SetActive(true);
    }

    public void VRStoreOff()
    {
        storeMenu.SetActive(false);
    }

}
