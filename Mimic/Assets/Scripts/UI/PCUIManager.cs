using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class PCUIManager : MonoBehaviour
{
    [Header("UI ���ڵ�. ")]
    [SerializeField] private GameObject uiboxholder = null;
    [Header("�Ʒ��� ������ ��ư: UI ���")]
    [SerializeField] private Button upbutton = null;
    [Header("���� �ø��� ��ư: UI �����")]
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

    #region["UI ���� �����"] 
    public void SetUpButton()
    {
        uiboxholder.SetActive(false);
        upbutton.interactable = false;
        downbutton.interactable = true; 
    }
    #endregion


    #region["UI �Ʒ��� ������"] 

    public void SetDownButton()
    {
        uiboxholder.SetActive(true);
        downbutton.interactable = false;
        upbutton.interactable = true; 
    }
    #endregion

}
