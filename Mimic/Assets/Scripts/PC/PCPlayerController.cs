using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    private Rigidbody playerRb;

    private float inputH;
    private float inputV;
    private Vector3 velocity = Vector3.zero;


    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();    
    }

    public void PCPlayerMove()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        velocity = new Vector3(inputH, 0, inputV);
        velocity *= moveSpeed;

        if (!(inputH == 0 && inputV == 0))
        {
            playerRb.velocity = velocity;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * moveSpeed);
        }
    }
}
