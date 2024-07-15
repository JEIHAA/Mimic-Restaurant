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
        if (timer_list == null)
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
        switch (Random.Range(0, Enum.GetValues(typeof(FoodInfo.Food)).Length))
        {
            case (int)FoodInfo.Food.Hamburger:
                //foodnum = GetRandomFoodNum(typeof(BurgerInfo.Hamburger));
                if (DayManager.instance.GetSeconds() < 60f)
                {
                    foodnum = 0;
                    enumstr = GetFoodNameByTypeandNumber(typeof(FriesInfo.Fries), foodnum);
                    Debug.Log("enumstr: " + enumstr);
                    sprite = fries[foodnum];
                }
                else
                {
                    foodnum = 0;
                    enumstr = GetFoodNameByTypeandNumber(typeof(BurgerInfo.Hamburger), foodnum);
                    Debug.Log("enumstr: " + enumstr);
                    sprite = hamburger[foodnum];
                }
                break;
            case (int)FoodInfo.Food.FrenchFries:
                //foodnum = GetRandomFoodNum(typeof(FriesInfo.Fries));
                foodnum = 0;
                enumstr = GetFoodNameByTypeandNumber(typeof(FriesInfo.Fries), foodnum);
                Debug.Log("enumstr: " + enumstr);
                sprite = fries[foodnum];
                break;
            case (int)FoodInfo.Food.Drink:
                //foodnum = GetRandomFoodNum(typeof(DrinkInfo.Drink)); 
                foodnum = 0;
                enumstr = GetFoodNameByTypeandNumber(typeof(DrinkInfo.Drink), foodnum);
                Debug.Log("enumstr: " + enumstr);
                sprite = drink[foodnum];
                break;
            default:
                //foodnum = GetRandomFoodNum(typeof(BurgerInfo.Hamburger));
                if (DayManager.instance.GetSeconds() < 90f)
                {
                    foodnum = 0;
                    enumstr = GetFoodNameByTypeandNumber(typeof(FriesInfo.Fries), foodnum);
                    Debug.Log("enumstr: " + enumstr);
                    sprite = fries[foodnum];
                }
                else
                {
                    foodnum = 0;
                    enumstr = GetFoodNameByTypeandNumber(typeof(BurgerInfo.Hamburger), foodnum);
                    Debug.Log("enumstr: " + enumstr);
                    sprite = hamburger[foodnum];
                }
                break;
        }
        foodinfolist.Add(enumstr, sprite);
        return foodinfolist;
    }
    #endregion


    private int GetRandomFoodNum(Type _enumtype)
    {
        return Random.Range(0, Enum.GetValues(_enumtype).Length + 1);
    }


    public string GetFoodNameByTypeandNumber(Type _enumtype, int _num)
    {
        return Enum.GetName(_enumtype, _num);
    }

    #region["음식 타이머 리스트 설정하기"] 
    private Dictionary<string, float> SetTimerList()
    {
        Dictionary<string, float> timer_list_ = new Dictionary<string, float>();
        Type foodtype = typeof(FoodInfo.Food);

        timer_list_.Add(Enum.GetName(foodtype, (int)FoodInfo.Food.Hamburger), 25f); //Hamburger 
        timer_list_.Add(Enum.GetName(foodtype, (int)FoodInfo.Food.FrenchFries), 30f); //FrenchFries 
        timer_list_.Add(Enum.GetName(foodtype, (int)FoodInfo.Food.Drink), 20f); //Drink 
        return timer_list_;
    }
    #endregion

    #region["음식 이름에 맞춰서 타이머 가져오기"] 
    public float GetFoodTimer(string _foodname)
    {
        float timer = 0f;
        if (_foodname.Contains("burger"))
        {
            timer = timer_list[Enum.GetName(typeof(FoodInfo.Food), (int)FoodInfo.Food.Hamburger)];  //Hamburger 
        }
        if (_foodname.Contains("Fries"))
        {
            timer = timer_list[Enum.GetName(typeof(FoodInfo.Food), (int)FoodInfo.Food.FrenchFries)];  //FrenchFries 
        }
        if (_foodname.Contains("Drink"))
        {
            timer = timer_list[Enum.GetName(typeof(FoodInfo.Food), (int)FoodInfo.Food.Drink)];  //Drink 
        }
        return timer;
    }
    #endregion  

}
