using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void PCPlayerMove()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        Vector3 velocity = new Vector3(-inputV, 0, inputH);
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
