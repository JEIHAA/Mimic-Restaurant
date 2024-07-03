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

    [SerializeField] private TextMeshProUGUI[] AddPriceTexts = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI[] MachineLevelTexts = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI[] MachineMoneyTexts = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI Increase_Sales_Money_Lv_Text = null;
    [SerializeField] private TextMeshProUGUI IncreaseSalesMoney_Text = null;
    [SerializeField] private TextMeshProUGUI Increase_Food_Hunger_Lv_Text = null;
    [SerializeField] private TextMeshProUGUI Increase_Food_Hunger_Level_Text = null;

    private int TotalMoney = 0;
    private int[] AddPrices = new int[3];
    [SerializeField] private int[] MachineLevels = new int[3];
    [SerializeField] private int[] MachineMoneys = new int[3];
    private int Increase_Sales_Money_Lv = 0;
    private int IncreaseSalesMoney = 0;
    private int Increase_Food_Hunger_Lv = 0;
    private int Increase_Food_Hunger_Level = 0;
    #endregion

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
        MachineLevelTexts[index].text = MachineLevels[index].ToString();
    }

    public void PrintMachineMoney(int index)
    {
        MachineMoneyTexts[index].text = MachineMoneys[index].ToString();
    }

    public void PrintIncrease_Sales_Money_Lv()
    {
        Increase_Sales_Money_Lv_Text.text = Increase_Sales_Money_Lv.ToString();
    }

    public void PrintIncreaseSalesMoney()
    {
        IncreaseSalesMoney_Text.text = IncreaseSalesMoney.ToString();
    }

    public void PrintIncrease_Food_Hunger_Lv()
    {
        Increase_Food_Hunger_Lv_Text.text = Increase_Food_Hunger_Lv.ToString();
    }

    public void PrintIncrease_Food_Hunger_Level()
    {
        Increase_Food_Hunger_Level_Text.text = Increase_Food_Hunger_Level.ToString();
    }

    #endregion

    #region 각 기능

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
                //그릴 
                MachineSpeedIncrease(120, 5, index);
                break;
            case 1:
                //탄산음료 
                MachineSpeedIncrease(140, 7, index);
                break;
            case 2:
                //음료 
                MachineSpeedIncrease(150, 8, index);
                break;
        }
    }

    private void HungerIncrease()
    {
        string message = "공복도 증가:\n";
        int initialHunger = 10;
        for (int i = 0; i < 5; i++)
        {
            int cost = 100 * (i + 1);
            int hungerValue = initialHunger + (10 * i);
            message += $"단계 {i + 1}: 비용 {cost}, 공복도 {hungerValue}\n";
        }
        Debug.Log(message);
        Increase_Food_Hunger_Lv++;
        PrintIncrease_Food_Hunger_Lv();
    }

    private void MoneyIncrease()
    {
        string message = "돈 증가:\n";
        for (int i = 0; i < 5; i++)
        {
            int cost = 150 + 100 * i;
            message += $"단계 {i + 1}: 비용 {cost}\n";
        }
        Debug.Log(message);
        IncreaseSalesMoney++;
        PrintIncreaseSalesMoney();
    }

    private void MachineSpeedIncrease(int baseCost, int rateIncrement, int index)
    {
        /*
        for (int i = 0; i < 5; i++)
        {
            int cost = baseCost + 100 * i;
            int rate = rateIncrement * (i + 1);
            message += $"단계 {i + 1}: 비용 {cost}, 증가율 {rate}%\n";
        }
        */
        MachineLevels[index]++;
        PrintMachineLevel(index);
    }

    #endregion
}
