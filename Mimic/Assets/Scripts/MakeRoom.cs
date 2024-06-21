using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/* 2024-06-21 강민기
 
 */

public class MakeRoom : MonoBehaviour
{
    [SerializeField] private string GameVersion = "1.0";
    [SerializeField] private string nickname = string.Empty;
    [SerializeField] private string nextScene = string.Empty;

    /*호출 시점: 게임 오브젝트가 활성화되었을 때, 즉 스크립트나 객체가 처음 로드될 때 호출됩니다.
    주 용도: 스크립트 초기화, 게임 오브젝트나 컴포넌트의 레퍼런스 설정 등.
    특징: 모든 Awake 메서드는 씬이 로드될 때 바로 호출됩니다.
     
     
     */

    // 객체가 로드될 때 가장 먼저 실행됨
    private void Awake()
    {
        
    }

    /* Start 메서드
     * 호출 시점: 첫 프레임이 시작되기 직전에 호출됩니다. 즉, 모든 Awake 메서드가 호출된 후에 호출됩니다.
     * 주 용도: 게임 시작 시 설정해야 할 것들, 게임 오브젝트가 활성화된 후에 초기화가 필요한 것들.
     * 게임 시작 시 설정해야 할 것들, 게임 오브젝트가 활성화된 후에 초기화가 필요한 것들.
    */

    // 첫 프레임 전에 실행됨
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        
    }
}
