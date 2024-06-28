using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PCPlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 5f;

    private float inputH;
    private float inputV;

    private Rigidbody characterRigidbody;
    private Animator anim;

    void Start()
    {
        characterRigidbody = GetComponent<Rigidbody>();
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    private void OnTriggerStay(Collider _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("Dispenser"))
        {
            InteractDispenser(_other.gameObject);
        }
    }

    private void InteractDispenser(GameObject go) {
        IDispenser dispenser = go.GetComponent<IDispenser>();

        if (Input.GetKeyDown("f"))
        {
            dispenser?.OperateDispenser();

            transform.LookAt(go.transform.position);

            /*// 현재 방향과 타겟 방향 사이의 회전 값을 계산
            Quaternion targetRotation = Quaternion.LookRotation(go.transform.position - this.transform.position);

            // 부드럽게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);*/
        }
    }

    public void PCPlayerMove()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        Vector3 velocity = new Vector3(-inputH, 0, -inputV);
        velocity *= moveSpeed;

        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            anim.SetInteger("AnimationPar", 1);
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);
        }

        if (!(inputH == 0 && inputV == 0))
        {
            characterRigidbody.velocity = velocity;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * rotateSpeed);
        }
    }
}
