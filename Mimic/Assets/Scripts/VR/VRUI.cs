using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VRUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TotalMoneyTextVR = null;
    [SerializeField] private MoneyManager moneyManager = null;
    [SerializeField] private GameObject storeMenu;
    private int totalMoney;
    private int baseMoney = 100;
    private int upgradeMoney = 100;
    private int barrierMoney = 200;
    private int clickCount = 1;

    private void Start()
    {
        UpdateTotalMoneyText();
        storeMenu.SetActive(false);
    }

    public void UpgradeUseMoney()   // 총업글시 사용머니 100원씩 증가
    {
        int moneyToUse = baseMoney + upgradeMoney * (clickCount - 1);
        
        if (totalMoney >= moneyToUse)
        {
            totalMoney -= moneyToUse;
            clickCount++;
            UpdateTotalMoneyText();
        }
    }

    public void BarrierUseMoney()       //배리어 업글시 사용머니
    {
        if (totalMoney >= barrierMoney)
        {
            totalMoney -= barrierMoney;
            UpdateTotalMoneyText();
        }
    }

    private void UpdateTotalMoneyText()
    {
        TotalMoneyTextVR.text = totalMoney.ToString();
    }

    public void AddMoney(int amount)        // AddMoney 메소드
    {
        totalMoney += amount;
        UpdateTotalMoneyText();
    }

    public void GetTotalMoneyFromPC(int _money)
    {
        totalMoney = _money;
        UpdateTotalMoneyText();
    }

    public void StoreOnButton()
    {
        storeMenu.SetActive(true);
        
    }

    public void StoreOffButton()
    {
        storeMenu.SetActive(false);
    }

}
