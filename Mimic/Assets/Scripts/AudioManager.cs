using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//BGM용
public class AudioManager : MonoBehaviour
{
    private AudioSource bgm_source = null;
    [SerializeField] private AudioClip bgm_pc = null;
    [SerializeField] private AudioClip bgm_vr = null;

    public static AudioManager instance = null; //This is Singleton. 

    #region["Awake is called when enable scriptable instance is loaded."] 
    private void Awake()
    {
        bgm_source = GetComponent<AudioSource>();
        instance = this; 
    }
    #endregion

    #region["PlayerPrefs 값에 맞춰서 볼륨 설정해주기"] 
    private void SetBGMVolume()
    {
        if(PlayerPrefs.HasKey("Volume_BGM"))
        {
            bgm_source.volume = PlayerPrefs.GetFloat("Volume_BGM") / 10f; 
        }
    }
    #endregion

    #region["BGM 플레이 => 메인 게임 화면에서 볼륨을 바꿨을 경우 이 메소드를 싱글톤으로 불러와 실행하는 메소드를 만든 다음 콜백으로 보내면 됨."] 
    public void PlayBGM()
    {
        if(bgm_source.isPlaying)
        {
            bgm_source.Stop(); 
        }
        SetBGMVolume(); 
        if(XRSettings.enabled)
        {
            //VR
            bgm_source.clip = bgm_vr; 
        }
        else
        {
            //PC 
            bgm_source.clip = bgm_pc; 
        }
        bgm_source.Play();
        bgm_source.loop = true; 
    }
    #endregion 
}
