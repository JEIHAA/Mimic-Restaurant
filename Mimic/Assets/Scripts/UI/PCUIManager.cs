using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class PCUIManager : MonoBehaviour
{
    [Header("UI 상자들. ")]
    [SerializeField] private GameObject uiboxholder = null;
    [Header("위로 올리는 버튼: UI 숨기기")]
    [SerializeField] private Button upbutton = null;
    [Header("아래로 내리는 버튼: UI 보여주기")]
    [SerializeField] private Button downbutton = null;
    [Header("설정 UI")]
    [SerializeField] private GameObject settingui = null; 


    private Vector3 originaluibox_transform = Vector3.zero;
    private Vector3 newuibox_transform = Vector3.zero;
    private GameObject settingui_instantiate = null;

    #region["Awake is called when enable scriptable instance is loaded."] 

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

    #region["설정화면 들어가기"]
    public void SettingButton()
    { 
        if(settingui_instantiate == null)
        {
            settingui_instantiate = Instantiate(settingui);
            settingui_instantiate.GetComponentInChildren<GameController_Setting>().CloseSettingsOnClick = ReplayBGM;
            Time.timeScale = 0f; 
        }
    }
    #endregion

    #region["브금 재설정"]
    public void ReplayBGM()
    {
        AudioManager.instance.PlayBGM();
        Destroy(settingui_instantiate);
        Time.timeScale = 1f; 
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


    public void UpgradeGrillButton()
    {
        //MoneyManager.instance.ShowGrillSpeedIncrease(); 
    }
}
