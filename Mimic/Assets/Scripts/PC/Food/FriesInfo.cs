using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriesInfo : FoodInfo, IGetFoodInfo
{
    public void GetFoodInfo()
    {
        throw new System.NotImplementedException();
    }

    //°¨ÀÚÆ¢±è
    public enum Fries
    {
        FrenchFries,     //°¨ÀÚÆ¢±è
        CheeseFries,     //Ä¡Áî °¨ÀÚÆ¢±è 
        VolcanoFries     //º¼ÄÉÀÌ³ë °¨ÀÚÆ¢±è 
    }
}
