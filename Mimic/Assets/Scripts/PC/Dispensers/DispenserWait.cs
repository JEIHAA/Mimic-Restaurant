using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
using static UnityEditor.Rendering.CameraUI;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class DispenserWait : MonoBehaviour
{
    [Header("요리기계 타이머 게이지")]
    [SerializeField] private GameObject timer_ui = null; 
    [SerializeField] protected Image timergauge = null;
    
    private Image burnimage = null;

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

    #region["탔을때 전용"] 
    protected IEnumerator WaitTimer(float _timer, GameObject _cookedmeat) 
    {
        timer_ui.SetActive(true);
        burnimage.gameObject.SetActive(true);
        timergauge.color = new Color(1f, 0f, 0f);
        float timer = _timer;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            timergauge.fillAmount = timer / _timer;
            EffectAudioManager.instance.PlayEffect("FoodBurnWarning", true);
            yield return new WaitForEndOfFrame();
            if(_cookedmeat.transform.parent != null && _cookedmeat.transform.parent.name.Equals("FoodBound"))
            {
                //요리하고 있는 고기를 집으면 부모가 생긴다. => 이때는 태워서는 안된다. 
                EffectAudioManager.instance.PlayEffect("FoodBurnWarning", false);
                timer_ui.SetActive(false);
                timergauge.color = new Color(1f, 1f, 1f);
                burnimage.gameObject.SetActive(false); 
                yield break; 
            }
        }
        EffectAudioManager.instance.PlayEffect("FoodBurnWarning", false);
        EffectAudioManager.instance.PlayEffect("FoodBurn", true); 
        _cookedmeat.GetComponent<Ingredients>().State = CookState.Burn;
        _cookedmeat.GetComponent<ICooking>()?.Burning();
        timer_ui.SetActive(false);
        burnimage.gameObject.SetActive(false); 
        timergauge.color = new Color(1f, 1f, 1f);
        yield break;
    }
    #endregion

    protected virtual void Awake()
    {
        timer_ui.SetActive(false);
        burnimage = timergauge.GetComponentsInChildren<Image>()[1];
        burnimage.gameObject.SetActive(false); 
    }
}
