using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector2 Xbounds = new Vector2(-2f, 2f);
    [SerializeField] private Vector2 Zbounds = new Vector2(20f, 30f);
    private Vector3 offset = Vector3.zero;
    float clampedX;
    float clampedZ;

    private Vector3 targetPosition;

    private void Start()
    {
        //offset = this.transform.position;
    }

    public void FollowPlayer() {
        // 목표 위치 계산
        targetPosition = offset;
        targetPosition.x += player.transform.position.x;
        targetPosition.z += player.transform.position.z;

        // 목표 위치의 x축과 y축을 범위 내로 제한
        clampedX = Mathf.Clamp(targetPosition.x, Xbounds.x, Xbounds.y);
        targetPosition.x = clampedX;
        clampedZ = Mathf.Clamp(targetPosition.z, Zbounds.x, Zbounds.y);
        targetPosition.z = clampedZ;

        // 카메라 위치 업데이트
        Vector3 smoothedPosition = Vector3.Lerp(this.transform.position, targetPosition, smoothSpeed); // 부드럽게 이동
        this.transform.position = smoothedPosition;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(offset, targetPosition);
    }
}
