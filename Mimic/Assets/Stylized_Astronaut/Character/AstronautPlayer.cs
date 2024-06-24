using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace AstronautPlayer
{

    public class AstronautPlayer : MonoBehaviour {

        private Animator anim;
        private CharacterController controller;

        public float speed = 600.0f;
        public float turnSpeed = 400.0f;
        private Vector3 moveDirection = Vector3.zero;
        public float gravity = 20.0f;

        void Start() {
            controller = GetComponent<CharacterController>();
            anim = gameObject.GetComponentInChildren<Animator>();
        }

        void Update() {
            if (Input.GetKey("w")) {
                anim.SetInteger("AnimationPar", 1);
            } else {
                anim.SetInteger("AnimationPar", 0);
            }

            if (controller.isGrounded) {
                moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
            }

            float turn = Input.GetAxis("Horizontal");
            transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
            controller.Move(moveDirection * Time.deltaTime);
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }
}
/*    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float turnSpeed = 400.0f;
    [SerializeField] private float gravity = 20.0f;

    private Animator anim;
    private CharacterController controller;

    private float inputH;
    private float inputV;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 turnDirection;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    public void PCPlayerMove()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            anim.SetInteger("AnimationPar", 1);
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);
        }

        if (controller.isGrounded)
        {
            moveDirection = (Vector3.forward * inputV + Vector3.right * -inputH) * Time.deltaTime;
            //moveDirection.x = transform.position.x;
            turnDirection = new Vector3(transform.rotation.x, moveDirection.x, moveDirection.z);
        }

        if (!(inputH == 0 && inputV == 0))
        {
            controller.Move(moveDirection * Time.deltaTime * moveSpeed);
            moveDirection.y -= gravity * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(turnDirection), Time.deltaTime * turnSpeed);
        }
    }*/
