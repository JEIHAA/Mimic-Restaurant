using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class OrderInfo : MonoBehaviour
{
    private Dictionary<string, float> timer_list = null;

    [Header("햄버거")]
    [SerializeField] private Sprite[] hamburger = null;
    [Header("감자튀김")]
    [SerializeField] private Sprite[] fries = null;
    [Header("음료")]
    [SerializeField] private Sprite[] drink = null;

    public static OrderInfo instance = null; //Another Singleton.. (Too many Singletons!) 

    private void Awake()
    {
        if(timer_list == null)
        {
            timer_list = SetTimerList();
        }
        instance = this; 
    }

    #region["랜덤으로 음식 이름과 사진 불러오기"] 
    public Dictionary<string, Sprite> GetRandomFoodInfo() 
    {
        string enumstr = string.Empty;
        Sprite sprite = null;
        Dictionary<string, Sprite> foodinfolist = new Dictionary<string, Sprite>();

        int foodnum = 0; 
        switch(Random.Range(0, Enum.GetValues(typeof(FoodEnumInfo.Food)).Length))
        {
            case (int)FoodEnumInfo.Food.Hamburger:
                foodnum = GetRandomFoodNum(typeof(FoodEnumInfo.Hamburger));
                enumstr = GetFoodNameByTypeandNumber(typeof(FoodEnumInfo.Hamburger), foodnum); 
                sprite = hamburger[foodnum];     
                break;
            case (int)FoodEnumInfo.Food.FrenchFries:
                foodnum = GetRandomFoodNum(typeof(FoodEnumInfo.FrenchFries));
                enumstr = GetFoodNameByTypeandNumber(typeof(FoodEnumInfo.FrenchFries), foodnum);
                sprite = fries[foodnum]; 
                break;
            case (int)FoodEnumInfo.Food.Drink:
                foodnum = GetRandomFoodNum(typeof(FoodEnumInfo.Drink)); 
                enumstr = GetFoodNameByTypeandNumber(typeof(FoodEnumInfo.Drink), foodnum); 
                sprite = drink[foodnum]; 
                break;
            default:
                break; 
        }
        foodinfolist.Add(enumstr, sprite);
        return foodinfolist; 
    }
    #endregion


    private int GetRandomFoodNum(Type _enumtype)
    {
        return Random.Range(0, Enum.GetValues(_enumtype).Length);
    }


    private string GetFoodNameByTypeandNumber(Type _enumtype, int _num)
    {
        return Enum.GetName(_enumtype, _num); 
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
