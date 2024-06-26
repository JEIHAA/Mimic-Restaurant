using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class LoadingController : MonoBehaviour
{

    [SerializeField] private Image loadingimage = null;
    [SerializeField] private TextMeshProUGUI loadingtext = null;
    [SerializeField, Range(0f, 0.1f)] private float WaitSecond_Image = 0.08f; 
    [SerializeField, Range(0f, 1f)] private float WaitSecond_Text = 0.25f;
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private GameObject vr = null;
    [SerializeField] private Camera pccamera = null;

    private bool isLoading = true;

    private AsyncOperation op = null;

    private void Awake()
    {
        
    }

    private void Start()
    {
        if(XRSettings.enabled)
        {
            //VRÀÏ¶§ 
        }
        else
        {

        }
        StartCoroutine(LoadingSprite());
        StartCoroutine(LoadingText()); 
    }


    private IEnumerator LoadingSprite()
    {
        int i = 0;

        while (isLoading)
        {
            if (i > 95)
            {
                i = 0;
            }
            loadingimage.sprite = Resources.Load<Sprite>("Images\\Sprites\\Loading_Sprites\\" + i);
            ++i;
            yield return new WaitForSeconds(WaitSecond_Image); 
        }
        yield break;
    }

    private IEnumerator LoadingText()
    {
        loadingtext.text = "Loading";
        float t = 0f;
        while (isLoading)
        {
            loadingtext.text += ".";
            ++t;
            yield return new WaitForSeconds(WaitSecond_Text);
            if (t >= 3f)
            {
                loadingtext.text = "Loading";
                t = 0f;
            }
        }
        yield break;
    }


}
