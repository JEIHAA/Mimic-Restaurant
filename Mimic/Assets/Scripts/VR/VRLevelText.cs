using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VRLevelText : MonoBehaviour
{
    public TMP_Text levelText; 

    private int currentLevel = 1;
    private int currentMoney = 100;
    [SerializeField]private int maxLevel = 5; 

    public void UpgradeLevel()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        else
        {
            currentLevel = maxLevel; 
        }

        UpgradeLevelText(); 
    }

    public void UpgradeMoney()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            currentMoney += 100;
        }
        else
        {
            currentLevel = maxLevel;
        }

        UpgradeMoneyText();
    }

    private void UpgradeLevelText()
    {
        if (currentLevel < maxLevel)
        {
            levelText.text = "LV" + currentLevel.ToString(); 
        }
        else
        {
            levelText.text = "LV.MAX"; 
        }
    }

    private void UpgradeMoneyText()
    {
        if (currentLevel < maxLevel)
        {
            levelText.text = currentMoney.ToString();
        }
        else
        {
            levelText.text = "MAX";
        }
    }
}
