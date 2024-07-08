using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Steak : Ingredients, ICooking
{
    private void Start()
    {
        ingredient = Ingredient.Steak;
        isCooked = false;
    }

    public void Cooking()
    {
        Debug.Log("ÆÄ±«!!!!!!!!!!!!!!!!!!!!");
        //this.gameObject.SetActive(false);
    }

    
}
