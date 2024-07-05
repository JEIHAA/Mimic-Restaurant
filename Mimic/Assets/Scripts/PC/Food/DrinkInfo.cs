using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkInfo : FoodInfo, IGetFoodInfo
{
    public void GetFoodInfo()
    {
        throw new System.NotImplementedException();
    }

    //음료수
    public enum Drink
    {
        ColaDrink,        //콜라
        /*
        CiderDrink,       //사이다(사과술 아님) 
        EnergyDrink,      //에너지 음료 
        WaterMelonDrink   //수박 음료 
        */ 
    }
}
