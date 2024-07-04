using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected bool isCooked = false;
    public bool IsCooked => isCooked;
    protected bool isCooking = false;
    public bool IsCooking { get => isCooking; set => isCooking = value; }

}
