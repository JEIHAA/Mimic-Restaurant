using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerInfo : FoodInfo, IGetFoodInfo
{
    public void GetFoodInfo()
    {
        throw new System.NotImplementedException();
    }

    //ÇÜ¹ö°Å
    public enum Hamburger
    {
        Hamburger,        //ÇÜ¹ö°Å 
        /*
        Cheeseburger,     //Ä¡ÁîÇÜ¹ö°Å 
        Shrimpburger,     //»õ¿ìÇÜ¹ö°Å 
        Octopusburger     //¹®¾îÇÜ¹ö°Å 
        */ 
    }
}
