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
        // ��ǥ ��ġ ���
        targetPosition = offset;
        targetPosition.x += player.transform.position.x;
        targetPosition.z += player.transform.position.z;

        // ��ǥ ��ġ�� x��� y���� ���� ���� ����
        clampedX = Mathf.Clamp(targetPosition.x, Xbounds.x, Xbounds.y);
        targetPosition.x = clampedX;
        clampedZ = Mathf.Clamp(targetPosition.z, Zbounds.x, Zbounds.y);
        targetPosition.z = clampedZ;

        // ī�޶� ��ġ ������Ʈ
        Vector3 smoothedPosition = Vector3.Lerp(this.transform.position, targetPosition, smoothSpeed); // �ε巴�� �̵�
        this.transform.position = smoothedPosition;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(offset, targetPosition);
    }
}
