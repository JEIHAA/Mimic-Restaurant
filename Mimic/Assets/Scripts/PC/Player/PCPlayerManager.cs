using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayerManager : MonoBehaviour
{
    [SerializeField] private PCPlayerController pcControll = null;
    [SerializeField] private CameraMove cameraMove = null;

    private void Start()
    {
        pcControll = GetComponentInChildren<PCPlayerController>();
        cameraMove = GetComponentInChildren<CameraMove>();
    }

    void Update()
    {
        pcControll?.PCPlayerMove();
        cameraMove?.FollowPlayer();
    }
}
