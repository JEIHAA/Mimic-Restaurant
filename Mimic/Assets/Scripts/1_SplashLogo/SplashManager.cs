using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

//2024-05-22: CUSTOM UNITY TEMPLATE 
/*
 2024-06-19 작성자 : 고영석
 수정 내용 : 
 
 
 */
public class SplashManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private GameObject vrobject = null;
    [SerializeField] private Camera pccamera = null; 

    #region["Start is called before the first frame update"] 
    private void Start()
    {
        StartCoroutine(SplashLogoCoroutine()); 
        if(XRSettings.enabled)
        {
            pccamera.gameObject.SetActive(false);
            canvas.renderMode = RenderMode.WorldSpace;
        }
        else
        {
            pccamera.gameObject.SetActive(true);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay; 
        }
    }
    #endregion

    #region["Logo Splash Loading Coroutine"]
    private IEnumerator SplashLogoCoroutine()
    {
        yield return new WaitForSeconds(3.4f);
        string scenename = "2_Login_StartGame"; 
        if(XRSettings.enabled)
        {
            scenename += "_VR"; 
        }
        SceneManager.LoadScene(scenename); 
    }
    #endregion

}
