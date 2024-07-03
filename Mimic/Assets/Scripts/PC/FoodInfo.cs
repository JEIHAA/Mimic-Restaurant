using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


internal class FoodInfo 
{
    private Dictionary<string, float> timer_list = null;
    //전체 음식 

    public FoodInfo()
    {
        if(timer_list == null)
        {
            timer_list = SetTimerList();
        }
    }

    #region["랜덤으로 음식 이름 불러오기"] 
    public string GetRandomFoodName() 
    {
        string enumstr = string.Empty;
        switch(Random.Range(0, Enum.GetValues(typeof(FoodEnumInfo.Food)).Length))
        {
            case (int)FoodEnumInfo.Food.Hamburger: 
                enumstr = GetRandomFoodName(typeof(FoodEnumInfo.Hamburger)); 
                break;
            case (int)FoodEnumInfo.Food.FrenchFries: 
                enumstr = GetRandomFoodName(typeof(FoodEnumInfo.FrenchFries)); 
                break;
            case (int)FoodEnumInfo.Food.Drink: 
                enumstr = GetRandomFoodName(typeof(FoodEnumInfo.Drink)); 
                break;
            default:
                break; 
        }
        return enumstr; 
    }
    #endregion

    //Overload 
    private string GetRandomFoodName(Type _enumtype)
    {
        return Enum.GetName(_enumtype, Random.Range(0, Enum.GetValues(_enumtype).Length)); 
    }

    #region["음식 타이머 리스트 설정하기"] 
    private Dictionary<string, float> SetTimerList()
    {
        Dictionary<string, float> timer_list_ = new Dictionary<string, float>();
        Type foodtype = typeof(FoodEnumInfo.Food);

        timer_list_.Add(Enum.GetName(foodtype, (int)FoodEnumInfo.Food.Hamburger), 15f); //Hamburger 
        timer_list_.Add(Enum.GetName(foodtype, (int)FoodEnumInfo.Food.FrenchFries), 20f); //FrenchFries 
        timer_list_.Add(Enum.GetName(foodtype, (int)FoodEnumInfo.Food.Drink), 10f); //Drink 
        return timer_list_; 
    }
    #endregion

    #region["음식 이름에 맞춰서 타이머 가져오기"] 
    public float GetFoodTimer(string _foodname)
    {
        float timer = 0f; 
        if(_foodname.Contains("burger"))
        {
            timer = timer_list[Enum.GetName(typeof(FoodEnumInfo.Food), 0)];  //Hamburger 
        }
        if(_foodname.Contains("Fries"))
        {
            timer = timer_list[Enum.GetName(typeof(FoodEnumInfo.Food), 1)];  //FrenchFries 
        }
        if(_foodname.Contains("Drink"))
        {
            timer = timer_list[Enum.GetName(typeof(FoodEnumInfo.Food), 2)];  //Drink 
        }
        return timer; 
    }
    #endregion  

}
