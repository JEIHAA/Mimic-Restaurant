using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class LoadingController : MonoBehaviour
{

    [SerializeField] private Image loadingimage = null;
    [SerializeField] private TextMeshProUGUI loadingtext = null;

    private bool isLoading = true;

    private void Start()
    {
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        int i = 0;
        float t = 0f; 
        loadingtext.text = "Loading";
        
        while (isLoading)
        {
            if (i > 95)
            {
                i = 0;
            }
            loadingtext.text += "."; 
            loadingimage.sprite = Resources.Load<Sprite>("Images\\Sprites\\Loading_Sprites\\" + i + "");
            ++i;
            ++t;
            yield return new WaitForSeconds(0.5f); 
            if(t >= 3f)
            {
                loadingtext.text = "Loading";
                t = 0f; 
            }
        }
        yield break;
    }

}
