using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryUI : MonoBehaviour
{
    [SerializeField] private Image storyimage = null;
    [SerializeField] private Sprite[] storysprite_arr = null;

    private int count = 0; 
    public void BackBtn()
    {
        if(count > 0)
        {
            --count; 
            storyimage.sprite = storysprite_arr[count]; 
        }
    }

    public void NextBtn()
    {
        if(count < storysprite_arr.Length)
        {
            ++count;
            storyimage.sprite = storysprite_arr[count]; 
        }
    }
}
