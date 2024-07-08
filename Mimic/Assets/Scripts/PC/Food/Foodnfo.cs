using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInfo : MonoBehaviour
{
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
    protected int price;
    /*protected GameObject nextLevel;
    public GameObject NextLevel => nextLevel;*/

    protected bool isGoVR = false;

    public bool IsGoVR
    {
        set { isGoVR = value; }
        get { return isGoVR;  }
    }
}
