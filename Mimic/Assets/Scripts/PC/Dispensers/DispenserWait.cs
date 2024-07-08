using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class DispenserWait : MonoBehaviour
{
    [Header("요리기계 타이머 게이지")]
    [SerializeField] private GameObject timer_ui = null; 
    [SerializeField] protected Image timergauge = null;


    protected IEnumerator WaitTimer(float _timer)
    {
        timer_ui.SetActive(true); 
        float timer = _timer;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            timergauge.fillAmount = timer / _timer;
            yield return new WaitForEndOfFrame();
        }
        timer_ui.SetActive(false); 
        yield break; 
    }

    protected virtual void Awake()
    {
        timer_ui.SetActive(false); 
    }
}
