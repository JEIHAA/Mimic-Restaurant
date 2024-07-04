using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance = null;

    #region 변수
    [SerializeField] private TextMeshProUGUI TotalMoneyText = null;

    [SerializeField] private TextMeshProUGUI[] AddPriceTexts = new TextMeshProUGUI[3]; //추가금액
    [SerializeField] private TextMeshProUGUI[] MachineLevelTexts = new TextMeshProUGUI[3]; //머신 레벨
    [SerializeField] private TextMeshProUGUI[] MachineMoneyTexts = new TextMeshProUGUI[3]; //머신 돈
    [SerializeField] private TextMeshProUGUI Increase_Sales_Money_Lv_Text = null;  //스킬-돈 증가 텍스트
    [SerializeField] private TextMeshProUGUI IncreaseSalesMoney_Text = null; //스킬-돈 증가 텍스트 
    [SerializeField] private TextMeshProUGUI Increase_Food_Hunger_Lv_Text = null; //스킬-증가 텍스트
    [SerializeField] private TextMeshProUGUI Increase_Food_Hunger_Text = null; //스킬- 

    private int TotalMoney = 0;
    private int[] AddPrices = new int[3];
    [SerializeField] private int[] MachineLevels = new int[3];
    [SerializeField] private int[] MachineMoneys = new int[3];
    [SerializeField] private int[] rates = new int[3]; //비율 증가

    private int Increase_Sales_Money_Lv = 1;
    private int Increase_Food_Hunger_Lv = 1;

    [Header("초기 공복도 비용")] 
    [SerializeField] private int Food_Hunger_Money = 100;
    [Header("초기 공복도 비율")]
    [SerializeField] private int Food_Hunger_Rate = 10;
    #endregion

    [Header("초기 판매가격 증가할때 드는 비용")]
    [SerializeField] private int Sale_Money_Cost = 150;
    [Header("판매가격 증가")]
    [SerializeField] private int Sale_Money = 50; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #region 각 추가금액,레벨,머니 표시

    public void Print_SalesAmount_Money()
    {
        TotalMoneyText.text = TotalMoney.ToString();
    }

    public void PrintAddPrice(int index)
    {
        AddPriceTexts[index].text = AddPrices[index].ToString();
    }

    public void PrintMachineLevel(int index)
    {
        MachineLevelTexts[index].text = "Lv." + MachineLevels[index];
        if (MachineLevels[index] == 5)
        {
            MachineLevelTexts[index].text = "Lv.Max" ;
            //MachineLevelTexts[index].GetComponent<RectTransform>().position += new Vector3(-25f, 0f, 0f);
            RectTransform rectTransform = MachineLevelTexts[index].GetComponent<RectTransform>();
            // 텍스트의 위치를 고정합니다.
            rectTransform.anchoredPosition = new Vector3(+40f, rectTransform.anchoredPosition.y,0);
        }
    }

    public void PrintMachineMoney(int index)
    {
        MachineMoneyTexts[index].text = MachineMoneys[index].ToString();
    }

    public void PrintIncrease_Sales_Money_Lv()
    {
        Increase_Sales_Money_Lv_Text.text = "Lv." + Increase_Sales_Money_Lv.ToString();
        if (Increase_Sales_Money_Lv == 5) 
        {
            Increase_Sales_Money_Lv_Text.text = "Lv.Max";
            //Increase_Sales_Money_Lv_Text.GetComponent<RectTransform>().position += new Vector3(-25f, 0f, 0f);
            RectTransform rectTransform = Increase_Sales_Money_Lv_Text.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(+40f, rectTransform.anchoredPosition.y, 0);
        }
    }

    public void PrintIncreaseSalesMoney()
    {
        IncreaseSalesMoney_Text.text = Sale_Money_Cost.ToString(); 
    }

    public void PrintIncrease_Food_Hunger_Lv()
    {
        Increase_Food_Hunger_Lv_Text.text = "Lv. " + Increase_Food_Hunger_Lv.ToString();
        if (Increase_Food_Hunger_Lv == 5) 
        {
            Increase_Food_Hunger_Lv_Text.text = "Lv.Max";
            //Increase_Food_Hunger_Lv_Text.GetComponent<RectTransform>().position += new Vector3(-25f, 0f, 0f);
            RectTransform rectTransform = Increase_Food_Hunger_Lv_Text.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(+40f, rectTransform.anchoredPosition.y, 0);
        }
    }

    public void PrintIncrease_Food_Hunger()
    {
        Increase_Food_Hunger_Text.text = Food_Hunger_Money.ToString(); 
    }

    #endregion

    #region 돈 관리 기능

    public void AddMoney(int _Money)
    {
        TotalMoney += _Money;
        Print_SalesAmount_Money();
    }

    public void MinusMoney(int _Money)
    {
        TotalMoney -= _Money;
        Print_SalesAmount_Money();
    }

    #endregion

    #region 업그레이드 및 표시 기능
    public void ShowHungerIncrease()
    {
        HungerIncrease();
    }

    public void ShowMoneyIncrease()
    {
        MoneyIncrease();
    }

    public void ShowMachineSpeedIncrease(int index)
    {
        switch (index)
        {
            case 0:
                MachineSpeedIncrease(5, index);
                break;
            case 1:
                MachineSpeedIncrease(7, index);
                break;
            case 2:
                MachineSpeedIncrease(8, index);
                break;
        }
    }

    private void HungerIncrease()
    {
        if(Increase_Food_Hunger_Lv < 5)
        {
            MinusMoney(Food_Hunger_Money); //초기 비용만큼 돈을 뺀다. 
            Food_Hunger_Money += 100; //비용 증가(간격: 100) 
            ++Increase_Food_Hunger_Lv; //레벨 증가 
            Food_Hunger_Rate += 10; //비율 증가 
        }
        PrintIncrease_Food_Hunger_Lv();
        PrintIncrease_Food_Hunger(); 
    }

    private void MoneyIncrease()
    { 
        if(Increase_Sales_Money_Lv < 5)
        {
            MinusMoney(Sale_Money_Cost);
            Sale_Money_Cost += 100;
            ++Increase_Sales_Money_Lv;
            Sale_Money += 50; 
        }
        PrintIncrease_Sales_Money_Lv(); 
        PrintIncreaseSalesMoney();

    }
    private void MachineSpeedIncrease(int _rateIncrement, int _index)
    {
        if (MachineLevels[_index] < 5)
        {
            MinusMoney(MachineMoneys[_index]); //쓴 만큼 뺀다. 
            ++MachineLevels[_index];
            MachineMoneys[_index] += 100;
            _rateIncrement *= MachineLevels[_index];
        }
        PrintMachineLevel(_index);
        PrintMachineMoney(_index);
    }
  
    #endregion
}
