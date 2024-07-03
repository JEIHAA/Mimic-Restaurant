using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryDispenser : MonoBehaviour
{
    private enum DispenserType
    {
        None, Hamburger, Fried, Drink
    }

    [SerializeField] private DispenserType dispenserType;
    private IDispenser dispenser;


    private void Awake()
    {
        switch (dispenserType)
        {
            case DispenserType.Hamburger:
                dispenser = this.gameObject.AddComponent<DispenserHamburger>();
                break;
            case DispenserType.Fried:
                dispenser = this.gameObject.AddComponent<DispenserFried>();
                break;
            case DispenserType.Drink:
                dispenser = this.gameObject.AddComponent<DispenserDrink>();
                break;
            default: Debug.LogError("Need dispenserType Setting");
                break;
        }        
    }
}
