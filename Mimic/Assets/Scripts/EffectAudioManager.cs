using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class EffectAudioManager : MonoBehaviour
{
    public static EffectAudioManager instance = null; //Singleton 
    private AudioSource audiosource = null;

    [Header("효과음 오디오 클립")] 
    [SerializeField] private AudioClip[] effectclip = null;

    public enum EffectEnum
    {
        FryingFries,          //감자튀김 튀기는 소리 
        RoastingMeat,         //고기 굽는 소리 
        Razer,                //레이저 
        MonsterAttack,        //몬스터 공격 
        MonsterAttackShield,  //몬스터가 보호막을 공격 
        RestoreShield,        //보호막 복구 
        DestroyShield,        //보호막 파괴 
        BellSound,            //손님 벨 소리 
        EmotionChange,        //강아지, 외계인 화났을때 
        AngryCustomer,        //손님 화나는 소리 
        DrinkSound,           //음료수 소리 
        FoodBurn,             //음식 불 붙음 
        FoodBurnWarning,      //음식 타기 전 경고 
        ConveySound,          //컨베이어 벨트 소리 
        GiveFood,             //음식 줌 
        ChooseIngredient      //플레이어 요리재료 선택 
    }

    #region["Awake is called when enable scriptable instance is loaded."] 
    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
        instance = this; 
    }
    #endregion

    public void PlayEffect(string _status, Boolean _NotStop)
    {
        if(PlayerPrefs.HasKey("Volume_Effect"))
        {
            audiosource.volume = PlayerPrefs.GetFloat("Volume_Effect") / 10f;
        }
        audiosource.clip = effectclip[(int)Enum.Parse(typeof(EffectEnum), _status)];
        if(!audiosource.isPlaying && _NotStop)
        {
            audiosource.Play(); 
        }
        else if(!_NotStop)
        {
            audiosource.Stop(); 
        }
    }

}
