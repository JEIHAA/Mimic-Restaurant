using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Steak : Ingredients, ICooking
{
    private void Start()
    {
        ingredient = Ingredient.Steak;
        state = CookState.Raw;
    }

    public void Burning()
    {
        Debug.Log("Burnd out...");
    }

    
}
