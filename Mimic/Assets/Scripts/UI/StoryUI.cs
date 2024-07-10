using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class StoryUI : MonoBehaviour
{

    [SerializeField] private Image story_image = null;
    [SerializeField] private Sprite[] story_image_sprite = null; 
    private int story_pagenum = 0; 


    public void BackBtn()
    {
        if(story_pagenum > 0)
        {
            --story_pagenum;
            PrintImage(); 
        }
    }

    public void ForwardBtn()
    {
        if(story_pagenum < 3)
        {
            ++story_pagenum;
            PrintImage(); 
        }
    }

    public void PrintImage()
    {
        story_image.sprite = story_image_sprite[story_pagenum]; 
    }

}
