using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class PCUIManager : MonoBehaviour
{
    public enum Machine{
        Grill, Drink, Fries
    }

    public int Increase_Sales_Money;
    public int Increase_Food_Hunger_Level;

    private GameObject settingui_instantiate = null;
    private GameObject tutorialui_instantiate = null;

    [Header("UI 상자들. ")]
    [SerializeField] private GameObject uiboxholder = null;
    [Header("정산 UI")]
    [SerializeField] private GameObject adjustui = null;
    [Header("설정 UI")]
    [SerializeField] private GameObject settingui = null;

    [Header("상자")]
    [SerializeField] private GameObject StoreBox = null;
    [SerializeField] private GameObject HamBurgerBox = null; 
    [SerializeField] private GameObject SodaBox = null;
    [SerializeField] private GameObject FrencFriesBox = null; 

    [Header("튜토리얼 UI")]
    [SerializeField] private GameObject tutorial_ui = null;

    #region["Start is called before the first frame update"] 
    private void Start()
    {
        HamBurgerBox.SetActive(false);
        SodaBox.SetActive(false);
        FrencFriesBox.SetActive(false);

        uiboxholder.SetActive(false); 
        adjustui.SetActive(false);

        MoneyManager.instance.Print_SalesAmount_Money(); 
        MoneyManager.instance.PrintFoodMoney_Each();
    }
    #endregion

    #region UI 기능
    #region 머신 업그레이드 기능
    public void GrillMachine_LvMoney()
    {
        MoneyManager.instance.ShowMachineSpeedIncrease((int)Machine.Grill);
    }

    public void DrinkMachine_LvMoney()
    {
        MoneyManager.instance.ShowMachineSpeedIncrease((int)Machine.Drink);
    }

    public void FryingMachine_LvMoney()
    {
        MoneyManager.instance.ShowMachineSpeedIncrease((int)Machine.Fries);
    }
    #endregion

    #region 판매 및 배고픔 기능
    public void Increase_Sales_Money_Lv()
    {
        MoneyManager.instance.ShowMoneyIncrease();
    }

    public void Increase_Food_Hunger_Lv()
    {
        MoneyManager.instance.ShowHungerIncrease(); 
    }
    #endregion

    #region 레시피 들어가기 및 뒤로가기 버튼 구현
    public void HambugerBtn()
    {
        StoreBox.SetActive(false);
        HamBurgerBox.SetActive(true);
    }

    public void SodaBtn()
    {
        StoreBox.SetActive(false);
        SodaBox.SetActive(true);    
    }

    public void FrencFriesBtn()
    {
        StoreBox.SetActive(false);
        FrencFriesBox.SetActive(true);
    }

    public void BackBtn_In_HambugerBox()
    {
        HamBurgerBox.SetActive(false);
        StoreBox.SetActive(true);
    }

    public void BackBtn_In_SodaBox()
    {
        SodaBox.SetActive(false);
        StoreBox.SetActive(true);
    }

    public void BackBtn_In_FrencFriesBox()
    {
        FrencFriesBox.SetActive(false);
        StoreBox.SetActive(true);
    }

    #region["설정 버튼"] 
    public void SettingBtn()
    {
        if(settingui_instantiate == null)
        {
            settingui_instantiate = Instantiate(settingui);
            settingui_instantiate.GetComponentInChildren<GameController_Setting>().CloseSettingsOnClick = ExitSetting;
            Time.timeScale = 0f; 
        }
    }
    #endregion

    #region["뒤로가기 버튼 => 정산 UI로 이동"] 
    public void BackBtn()
    {
        uiboxholder.SetActive(false);
        adjustui.SetActive(true); 
    }
    #endregion

    public void ExitSetting()
    {
        Destroy(settingui_instantiate);
        AudioManager.instance.PlayBGM();
        Time.timeScale = 1f; 
    }

    #endregion
    #endregion

    #region["정산 UI 출력"]
    public void AdjustUI()
    {
        adjustui.SetActive(true);
        adjustui.GetComponent<AdjustUIManager>().RunAdjustUI(); 
    }
    #endregion

    #region["튜토리얼 버튼"] 
    public void TutorialBtn()
    {
        if(tutorialui_instantiate == null)
        {
            tutorialui_instantiate = Instantiate(tutorial_ui);
            tutorialui_instantiate.GetComponentsInChildren<Button>()[0].onClick.AddListener(CloseTutorialBtn); 
            gameObject.SetActive(false);
            Time.timeScale = 0f; 
        }
    }
    #endregion

    #region["튜토리얼 닫기 버튼"]
    public void CloseTutorialBtn()
    {
        Destroy(tutorialui_instantiate);
        gameObject.SetActive(true); 
        Time.timeScale = 1f; 
    }
    #endregion

}
