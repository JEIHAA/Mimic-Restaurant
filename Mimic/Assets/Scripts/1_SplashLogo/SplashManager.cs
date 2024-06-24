using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//2024-05-22: CUSTOM UNITY TEMPLATE 
/*
 2024-06-19 작성자 : 고영석
수정 내용 : 
 
 
 */
public class SplashManager : MonoBehaviour
{

    #region["Start is called before the first frame update"] 
    private void Start()
    {
        StartCoroutine(SplashLogoCoroutine()); 
    }
    #endregion

    #region["Logo Splash Loading Coroutine"]
    private IEnumerator SplashLogoCoroutine()
    {
        yield return new WaitForSeconds(3.4f);
        SceneManager.LoadScene("2_Login_StartGame");
    }
    #endregion

}
