using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ingredients;

public class SteakCooed : Ingredients, ICooking
{
    private void Start()
    {
        ingredient = Ingredient.Steak;
        isCooked = true;
    }

    public void Cooking()
    {
        Debug.Log("Cooking...");
    }
}
