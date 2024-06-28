using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class LoadingController : MonoBehaviour
{
    [Header("�ε� �̹���")]
    [SerializeField] private Image loadingimage = null;
    [Header("�ε� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI loadingtext = null;
    [Header("�ε� �̹��� �� �ؽ�Ʈ ����(��)")]
    [SerializeField, Range(0f, 0.1f)] private float WaitSecond_Image = 0.08f; 
    [SerializeField, Range(0f, 1f)] private float WaitSecond_Text = 0.25f;
    [Header("VR����: ĵ����")]
    [SerializeField] private Canvas canvas = null;
    [Header("VR����: VR���� ������Ʈ")]
    [SerializeField] private GameObject vr = null;
    [Header("VR����: PCī�޶�")]
    [SerializeField] private Camera pccamera = null;

    [Header("���� Scene")]
    [SerializeField] private string mainscene = string.Empty;

    private AsyncOperation op = null;

    private void Start()
    {
        if(XRSettings.enabled)
        {
            //VR�϶� 
            pccamera.gameObject.SetActive(false); 
        }
        else
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            vr.SetActive(false); 
        }
        op = SceneManager.LoadSceneAsync(mainscene);
        StartCoroutine(LoadingSprite());
        StartCoroutine(LoadingText()); 
    }


    private IEnumerator LoadingSprite()
    {
        int i = 0;

        while (!op.isDone)
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
        while (!op.isDone)
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
