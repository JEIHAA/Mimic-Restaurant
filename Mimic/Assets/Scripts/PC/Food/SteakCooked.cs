using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ingredients;

public class SteakCooked : Ingredients, ICooking
{
    [SerializeField] Material[] materials;
    [SerializeField] MeshRenderer renderer;

    private void Start()
    {
        ingredient = Ingredient.Steak;
        state = CookState.Cooked;
        renderer= GetComponent<MeshRenderer>();
    }

    public void Burning()
    {
        renderer.materials = materials;
        Debug.Log("Burned out...");
    }
}
