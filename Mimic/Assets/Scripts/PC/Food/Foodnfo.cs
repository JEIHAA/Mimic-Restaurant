using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInfo : MonoBehaviour
{
    [SerializeField] protected FoodStat foodstat = null; 
    public enum Food
    {
        None,
        Hamburger,       //ÇÜ¹ö°Å 
        FrenchFries,     //°¨ÀÚÆ¢±è 
        Drink,           //À½·á¼ö 
        ingredient
    }
;
    protected int fillHunger;
    public int GetFillHunger()
    {
        return fillHunger; 
    }
    protected int price;

    protected bool isGoVR = false;

    public bool IsGoVR
    {
        set { isGoVR = value; }
        get { return isGoVR;  }
    }

    public int GetMoney()
    {
        return price; 
    }


}
