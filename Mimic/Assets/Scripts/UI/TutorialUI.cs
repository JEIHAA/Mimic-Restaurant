using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites_list = null;
    [SerializeField] private Image image = null;
    [SerializeField, TextArea] private string[] textline = null;
    [SerializeField] private TextMeshProUGUI line = null;

    private int count = 0;  
  

    public void BackBtn()
    {
        if(count > 0)
        {
            --count;
            /*
            if (count == 0)
            {
                StopAllCoroutines(); 
            }
            else if (count == 1)
            {
                StopAllCoroutines();
                StartCoroutine(Coroutine_CustomerGIF()); 
            }
            else if(count == 2)
            {
                StopAllCoroutines();
            }
            */
            image.sprite = sprites_list[count];
            line.text = textline[count]; 
        }
    }

    public void NextBtn()
    {
        if(count < sprites_list.Length)
        {
            ++count;
            image.sprite = sprites_list[count];
            line.text = textline[count]; 
            /*
            if(count == 1)
            {
                //요리 
                StopAllCoroutines(); 
                StartCoroutine(Coroutine_CustomerGIF()); 
            }
            else if(count == 2)
            {
                //상점UI 
                StopAllCoroutines();
                StartCoroutine(Coroutine_ShopUI()); 
            }
            else if(count == 3)
            {
                //VR
                StopAllCoroutines();
                StartCoroutine(Coroutine_WithVR()); 
            }
            */
        }
    }
}
