using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class PCUIManager : MonoBehaviour
{
    [Header("UI 상자들. ")]
    [SerializeField] private GameObject uiboxholder = null;
    [Header("아래로 내리는 버튼: UI 출력")]
    [SerializeField] private Button upbutton = null;
    [Header("위로 올리는 버튼: UI 숨기기")]
    [SerializeField] private Button downbutton = null;
    #region["Awake is called when enable scriptable instance is loaded."] 
    private void Awake()
    {
        
    }
    #endregion


    #region["Start is called before the first frame update"] 
    private void Start()
    {
        
    }
   #endregion


    #region["Update is called once per frame"] 
    private void Update()
    {
        
    }
    #endregion

    #region["UI 위로 숨기기"] 
    public void SetUpButton()
    {
        uiboxholder.SetActive(false);
        upbutton.interactable = false;
        downbutton.interactable = true; 
    }
    #endregion


    #region["UI 아래로 내리기"] 

    public void SetDownButton()
    {
        uiboxholder.SetActive(true);
        downbutton.interactable = false;
        upbutton.interactable = true; 
    }
    #endregion

}
