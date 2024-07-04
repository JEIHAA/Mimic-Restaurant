using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class PCUIManager : MonoBehaviour
{
    enum Machine{
        Grill, Drink, Fries
    }
    [Header("UI 상자들. ")]
    [SerializeField] private GameObject uiboxholder = null;
    [Header("위로 올리는 버튼: UI 숨기기")]
    [SerializeField] private Button upbutton = null;
    [Header("아래로 내리는 버튼: UI 보여주기")]
    [SerializeField] private Button downbutton = null;
    #region["Awake is called when enable scriptable instance is loaded."] 
    [Header("설정 UI")]
    [SerializeField] private GameObject settingsui  = null;

    private Vector3 originaluibox_transform = Vector3.zero;
    private Vector3 newuibox_transform = Vector3.zero;
    private GameObject settingsui_instantiate = null;

    private void Awake()
    {
        originaluibox_transform = uiboxholder.GetComponent<RectTransform>().position; 
        uiboxholder.GetComponent<RectTransform>().position += new Vector3(0f, 1000f, 0f);
        newuibox_transform = uiboxholder.GetComponent<RectTransform>().position; 
    }
    #endregion

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

    #region 각 기능들

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


    public void Increase_Sales_Money_Lv()
    {

    }

    public void IncreaseSalesMoney()
    {

    }
    public void Increase_Food_Hunger_Lv()
    {

    }

    public void Increase_Food_Hunger()
    {

    }


    public void SettingsButton()
    {
        if(settingsui_instantiate == null)
        {
            settingsui_instantiate = Instantiate(settingsui);
            settingsui_instantiate.GetComponentInChildren<GameController_Setting>().CloseSettingsOnClick = ReplayBGM;
            Time.timeScale = 0f; 
        }
    }

    public void ReplayBGM()
    {
        Destroy(settingsui_instantiate);
        Time.timeScale = 1f;
        AudioManager.instance.PlayBGM();
    }
    #endregion
}
