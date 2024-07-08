using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class PCUIManager : MonoBehaviour
{
    enum Machine{
        Grill, Drink, Fries
    }
    public int Increase_Sales_Money;
    public int Increase_Food_Hunger_Level;
    
    [Header("UI 상자들. ")]
    [SerializeField] private GameObject uiboxholder = null;
    [Header("위로 올리는 버튼: UI 숨기기")]
    [SerializeField] private Button upbutton = null;
    [Header("아래로 내리는 버튼: UI 보여주기")]
    [SerializeField] private Button downbutton = null;
   
    public GameObject StoreBox; 
    public GameObject HamBurgerBox;
    public GameObject SodaBox; 
    public GameObject FrencFriesBox;

    public Button GrillMachineBtn;
    public Button DrinkMachineBtn;
    public Button FriesMachineBtn;
    public Button Increase_Sales_MoneyBtn;
    public Button Increase_Food_Hunger_LevelBtn;


    #region["Awake is called when enable scriptable instance is loaded."] 

    private Vector3 originaluibox_transform = Vector3.zero;
    private Vector3 newuibox_transform = Vector3.zero; 
    private void Awake()
    {
        originaluibox_transform = uiboxholder.GetComponent<RectTransform>().position; 
        uiboxholder.GetComponent<RectTransform>().position += new Vector3(0f, 1000f, 0f);
        newuibox_transform = uiboxholder.GetComponent<RectTransform>().position; 
    }
    #endregion

    #region["Start is called before the first frame update"] 
    private void Start()
    {
        HamBurgerBox.SetActive(false);
        SodaBox.SetActive(false);
        FrencFriesBox.SetActive(false);
    }
    #endregion

    #region UI 기능 
    #region["UI 위로 숨기기"] 
    public void SetUpButton()
    {
        //uiboxholder.GetComponent<RectTransform>().position += new Vector3(0f, 1000f, 0f); 
        StartCoroutine(UpUICoroutine()); 
    }
    #endregion

    #region["UI 아래로 내리기"] 

    public void SetDownButton()
    {
        //uiboxholder.GetComponent<RectTransform>().position = originaluibox_transform; 
        StartCoroutine(DownUICoroutine()); 
    }
    #endregion

    #region["위로 올리는 코루틴"] 
    private IEnumerator UpUICoroutine()
    {
        while(uiboxholder.GetComponent<RectTransform>().position.y < newuibox_transform.y)
        {
            uiboxholder.GetComponent<RectTransform>().position += new Vector3(0f, 4000f * Time.deltaTime, 0f);
            yield return new WaitForEndOfFrame(); 
        }
        uiboxholder.GetComponent<RectTransform>().position = newuibox_transform; 
        yield break; 
    }
    #endregion

    #region["아래로 내리는 코루틴"]
    private IEnumerator DownUICoroutine()
    {
        while(uiboxholder.GetComponent<RectTransform>().position.y > originaluibox_transform.y)
        {
            uiboxholder.GetComponent<RectTransform>().position -= new Vector3(0f, 4000f * Time.deltaTime, 0f);
            yield return new WaitForEndOfFrame(); 
        }
        uiboxholder.GetComponent<RectTransform>().position = originaluibox_transform; 
        yield break; 
    }
    #endregion

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

    #endregion
    #endregion
}
