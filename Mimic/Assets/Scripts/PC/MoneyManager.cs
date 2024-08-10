using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
public class MoneyManager : MonoBehaviourPun
{
    //Singleton
    public static MoneyManager instance = null;
    //기계
    public enum Machine
    {
        Grill, //그릴
        Drink, //음료
        Fryer  //튀김
    }
    #region 변수
    [SerializeField] private TextMeshProUGUI TotalMoneyText = null;
    [SerializeField] private TextMeshProUGUI[] AddPriceTexts = new TextMeshProUGUI[3]; //추가금액
    [SerializeField] private TextMeshProUGUI[] MachineLevelTexts = new TextMeshProUGUI[3]; //머신 레벨
    [SerializeField] private TextMeshProUGUI[] MachineMoneyTexts = new TextMeshProUGUI[3]; //머신 돈
    [SerializeField] private TextMeshProUGUI Increase_Sales_Money_Lv_Text = null;  //스킬-돈 증가 텍스트
    [SerializeField] private TextMeshProUGUI IncreaseSalesMoney_Text = null; //스킬-돈 증가 텍스트
    [SerializeField] private TextMeshProUGUI Increase_Food_Hunger_Lv_Text = null; //스킬-증가 텍스트
    [SerializeField] private TextMeshProUGUI Increase_Food_Hunger_Text = null; //스킬-
    [Header("전체 돈")]
    [SerializeField] private int TotalMoney = 0; //전체 돈
    [SerializeField] private int[] MachineLevels = new int[3];
    [SerializeField] private int[] MachineMoneys = new int[3];
    [SerializeField] private int[] foodMoneys = new int[3];
    [SerializeField] private TextMeshProUGUI[] foodMoney_text = null;
    [Header("초기 판매 돈 레벨")]
    private int Increase_Sales_Money_Lv = 1;
    [Header("초기 음식 공복도")]
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
    [Header("기계")]
    [SerializeField] private DispenserHamburger[] hamburgerdispenser = null;
    [SerializeField] private DispenserStove[] stovedispenser = null;
    [SerializeField] private DispenserDrink[] drinkdispenser = null;
    [SerializeField] private DispenserFried[] frydispenser = null;
    [Header("음식 Scriptable Object")]
    [SerializeField] private FoodStat[] foodstat = null;
    [Header("VR UI")]
    [SerializeField] private VRUI vrui = null;
    [Header("안내 메시지")]
    [SerializeField] private GameObject nomoney_msg = null;
    private GameObject nomoney_msg_instantiate = null;
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
    private void Start()
    {
        Print_SalesAmount_Money();
    }
    #region 각 추가금액,레벨,머니 표시
    public void Print_SalesAmount_Money()
    {
        TotalMoneyText.text = TotalMoney.ToString();
        if (!XRSettings.enabled && PhotonNetwork.IsConnected)
        {
            //PC -> VR
            photonView.RPC("SendMoneyToVR", RpcTarget.OthersBuffered, TotalMoney);
        }
    }
    public void PrintMachineLevel(int index)
    {
        MachineLevelTexts[index].text = "Lv." + MachineLevels[index];
        if (MachineLevels[index] == 5)
        {
            MachineLevelTexts[index].text = "Lv.Max";
            //MachineLevelTexts[index].GetComponent<RectTransform>().position += new Vector3(-25f, 0f, 0f);
            RectTransform rectTransform = MachineLevelTexts[index].GetComponent<RectTransform>();
            // 텍스트의 위치를 고정합니다.
            rectTransform.anchoredPosition = new Vector3(+40f, rectTransform.anchoredPosition.y, 0);
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
            RectTransform rectTransform = Increase_Sales_Money_Lv_Text.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(+40f, rectTransform.anchoredPosition.y, 0);
        }
    }
    public void PrintFoodMoney_Each()
    {
        //햄버거, 감자튀김, 콜라
        for (int i = 0; i < foodMoneys.Length; ++i)
        {
            foodMoney_text[i].text = foodMoneys[i].ToString();
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
        Debug.Log("Money: " + _Money);
        TotalMoney += _Money;
        Print_SalesAmount_Money();
    }
    #region["돈 쓰는 메소드"]
    public void MinusMoney(int _Money)
    {
        TotalMoney -= _Money;
        Print_SalesAmount_Money();
        if(XRSettings.enabled)
        {
            photonView.RPC("SendToPC", RpcTarget.OthersBuffered, TotalMoney); 
        }
    }
    #endregion

    [PunRPC]
    public void SendToPC(int _money)
    {
        TotalMoney = _money;
        Print_SalesAmount_Money(); 
    }
    #region["현재 돈을 쓸 수 있는지 체크: 폭탄 같은 경우는 이 체크를 무시하고 바로 MinusMoney 하면 됨."]
    public bool MoneyCheck(int _Money)
    {
        if (TotalMoney > 0)
        {
            if (TotalMoney > _Money)
            {
                //돈을 사용할 수 있음.
                return true;
            }
        }
        //돈을 사용할 수 없음.
        return false;
    }
    #endregion
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
            case (int)Machine.Grill: //Grill
                MachineSpeedIncrease(5, index);
                break;
            case (int)Machine.Drink: //Drink
                MachineSpeedIncrease(7, index);
                break;
            case (int)Machine.Fryer: //Fries
                MachineSpeedIncrease(6, index);
                break;
        }
    }
    private void HungerIncrease()
    {
        if (TotalMoney > 0)
        {
            if (Increase_Food_Hunger_Lv < 5)
            {
                if (MoneyCheck(Food_Hunger_Money))
                {
                    MinusMoney(Food_Hunger_Money); //초기 비용만큼 돈을 뺀다.
                    for (int i = 0; i < foodstat.Length; ++i)
                    {
                        float rate = foodstat[i].hungerrestore * (Food_Hunger_Rate / 100f);
                        foodstat[i].hungerrestore += (int)rate;
                    }
                    Food_Hunger_Money += 100; //비용 증가(간격: 100)
                    ++Increase_Food_Hunger_Lv; //레벨 증가
                    Food_Hunger_Rate += 10; //비율 증가
                }
                else
                {
                    NoMoneyMessage();
                }
            }
            PrintIncrease_Food_Hunger_Lv();
            PrintIncrease_Food_Hunger();
        }
    }
    private void MoneyIncrease()
    {
        if (TotalMoney > 0)
        {
            if (Increase_Sales_Money_Lv < 5)
            {
                if (MoneyCheck(Sale_Money_Cost))
                {
                    MinusMoney(Sale_Money_Cost);
                    for (int i = 0; i < foodMoneys.Length; ++i)
                    {
                        foodMoneys[i] += Sale_Money;
                        foodstat[i].money += Sale_Money;
                    }
                    PrintFoodMoney_Each();
                    Sale_Money_Cost += 100;
                    ++Increase_Sales_Money_Lv;
                    Sale_Money += 50;
                }
                else
                {
                    NoMoneyMessage();
                }
            }
            PrintIncrease_Sales_Money_Lv();
            PrintIncreaseSalesMoney();
        }
    }
    private void MachineSpeedIncrease(int _rateIncrement, int _index)
    {
        if (TotalMoney > 0)
        {
            if (MachineLevels[_index] < 5)
            {
                if (MoneyCheck(MachineMoneys[_index]))
                {
                    MinusMoney(MachineMoneys[_index]); //쓴 만큼 뺀다.
                    float newrate_int = _rateIncrement * MachineLevels[_index];
                    float minusrate = newrate_int / 100f;
                    Debug.Log("minusrate: " + minusrate);
                    switch (_index)
                    {
                        case (int)Machine.Grill:
                            for (int i = 0; i < stovedispenser.Length; ++i)
                            {
                                stovedispenser[i].UpgradeTimer(minusrate);
                            }
                            for (int i = 0; i < hamburgerdispenser.Length; ++i)
                            {
                                hamburgerdispenser[i].UpgradeTimer(minusrate);
                            }
                            break;
                        case (int)Machine.Drink:
                            for (int i = 0; i < drinkdispenser.Length; ++i)
                            {
                                drinkdispenser[i].UpgradeTimer(minusrate);
                            }
                            break;
                        case (int)Machine.Fryer:
                            for (int i = 0; i < frydispenser.Length; ++i)
                            {
                                frydispenser[i].UpgradeTimer(minusrate);
                            }
                            break;
                    }
                    ++MachineLevels[_index];
                    MachineMoneys[_index] += 100;
                }
                else
                {
                    NoMoneyMessage();
                }
            }
            PrintMachineLevel(_index);
            PrintMachineMoney(_index);
        }
    }
    #endregion
    #region["VR 쪽에 돈정보 전달"]
    [PunRPC]
    public void SendMoneyToVR(int _totalMoney)
    {
        vrui.GetTotalMoneyFromPC(_totalMoney);
    }
    #endregion

    public void SetTotalMoney(int _money)
    {
        TotalMoney = _money;
        Print_SalesAmount_Money();
    }

    public int GetTotalMoney()
    {
        return TotalMoney;
    }
    #region["돈 없을때 안내 메시지 출력: VR 플레이어가 죽었을때 폭탄 터지는건 상관없음"]
    public void NoMoneyMessage()
    {
        if (nomoney_msg_instantiate == null)
        {
            nomoney_msg_instantiate = Instantiate(nomoney_msg);
            nomoney_msg_instantiate.GetComponent<MessageUI>().SetText(0);
        }
    }
    #endregion
}