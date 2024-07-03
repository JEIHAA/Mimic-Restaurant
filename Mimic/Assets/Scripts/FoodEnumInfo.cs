using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class FoodEnumInfo 
{
    public enum Food
    {
        Hamburger,       //햄버거 
        FrenchFries,     //감자튀김 
        Drink            //음료수 
    }

    //햄버거
    public enum Hamburger
    {
        Hamburger,        //햄버거 
        Cheeseburger,     //치즈햄버거 
        Shrimpburger,     //새우햄버거 
        Octopusburger     //문어햄버거 
    }

    //감자튀김
    public enum FrenchFries
    {
        FrenchFries,     //감자튀김
        CheeseFries,     //치즈 감자튀김 
        VolcanoFries     //볼케이노 감자튀김 
    }

    //음료수
    public enum Drink
    {
        ColaDrink,        //콜라
        CiderDrink,       //사이다(사과술 아님) 
        EnergyDrink,      //에너지 음료 
        WaterMelonDrink   //수박 음료 
    }

}
