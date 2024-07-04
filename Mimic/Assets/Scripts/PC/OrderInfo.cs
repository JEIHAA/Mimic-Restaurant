using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


internal class OrderInfo 
{
    private Dictionary<string, float> timer_list = null;
    //전체 음식 

    public OrderInfo()
    {
        if(timer_list == null)
        {
            timer_list = SetTimerList();
        }
    }

    #region["랜덤으로 음식 이름 불러오기"] 
/*    public string GetRandomFoodName() 
    {
        string enumstr = string.Empty;
        switch(Random.Range(0, Enum.GetValues(typeof(FoodInfo.Food)).Length))
        {
            case (int)FoodInfo.Food.Hamburger: 
                enumstr = GetRandomFoodName(typeof(FoodInfo.Hamburger)); 
                break;
            case (int)FoodInfo.Food.FrenchFries: 
                enumstr = GetRandomFoodName(typeof(FoodInfo.FrenchFries)); 
                break;
            case (int)FoodInfo.Food.Drink: 
                enumstr = GetRandomFoodName(typeof(FoodInfo.Drink)); 
                break;
            default:
                break; 
        }
        return enumstr; 
    }*/
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
        Type foodtype = typeof(FoodInfo.Food);

        timer_list_.Add(Enum.GetName(foodtype, (int)FoodInfo.Food.Hamburger), 15f); //Hamburger 
        timer_list_.Add(Enum.GetName(foodtype, (int)FoodInfo.Food.FrenchFries), 20f); //FrenchFries 
        timer_list_.Add(Enum.GetName(foodtype, (int)FoodInfo.Food.Drink), 10f); //Drink 
        return timer_list_; 
    }
    #endregion

    #region["음식 이름에 맞춰서 타이머 가져오기"] 
    public float GetFoodTimer(string _foodname)
    {
        float timer = 0f; 
        if(_foodname.Contains("burger"))
        {
            timer = timer_list[Enum.GetName(typeof(FoodInfo.Food), 0)];  //Hamburger 
        }
        if(_foodname.Contains("Fries"))
        {
            timer = timer_list[Enum.GetName(typeof(FoodInfo.Food), 1)];  //FrenchFries 
        }
        if(_foodname.Contains("Drink"))
        {
            timer = timer_list[Enum.GetName(typeof(FoodInfo.Food), 2)];  //Drink 
        }
        return timer; 
    }
    #endregion  

}
