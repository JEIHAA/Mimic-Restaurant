using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookState
{
    None,
    Raw,
    Cooking,
    Cooked,
    Burn
}

public class Ingredients: MonoBehaviour
{
    public enum Ingredient
    {
        None,
        Steak,
        Cheese
    }

    [SerializeField] protected Ingredient ingredient;
    [SerializeField] protected GameObject nextLevel = null;
    public GameObject NextLevel => nextLevel;

    protected CookState state = CookState.None;
    public CookState State { get => state; set => state = value; }

    protected bool isGoPC = false; 

    public bool IsGoPC
    {
        set { isGoPC = value; }
        get { return isGoPC; }
    }

}
