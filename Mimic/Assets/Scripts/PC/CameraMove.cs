using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector2 xBounds = new Vector2(7f, 16f);
    //[SerializeField] private Vector2 zBounds = new Vector2(-3.3f, 3.3f);

    private Vector3 offset;
    private Vector3 targetPosition;

    private void Start()
    {
        offset = this.transform.position;
    }

    public void FollowPlayer() {
        // ��ǥ ��ġ ���
        targetPosition.x = offset.x + player.transform.position.x;
        //targetPosition.z = offset.z + player.transform.position.z;

        // ��ǥ ��ġ�� x��� y���� ���� ���� ����
        float clampedX = Mathf.Clamp(targetPosition.x, xBounds.x, xBounds.y);
        //float clampedZ = Mathf.Clamp(targetPosition.z, zBounds.x, zBounds.y);
        targetPosition = new Vector3(clampedX, transform.position.y, transform.position.z);

        // ī�޶� ��ġ ������Ʈ
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed); // �ε巴�� �̵�
        transform.position = smoothedPosition;
    }
}
