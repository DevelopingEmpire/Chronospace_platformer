using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //game object elements
    public Transform transformSelf;
    public CharacterController controller;

    //physical param variables
    public float jumpSpeed = 10f;
    public float gravity = 20f;
    public float movSpeed = 20f;
    public float rotSpeed = 400f;
    public float walkSpeedPercentage = 0.35f;

    //movement param variables
    Vector3 moveDirection;
    Vector3 savedDirection;
    private float inputV;
    private float inputH;
    private float rotateX;
    private bool inputWalk;
    private bool inputJump;
    private bool inputDodge;

    /*
    Input Axis V(front and back)
    Input Axis H(left and right)
    Input of Walking Switch
    Input of Jumping Switch
    Input of Dodging Switch
    Input Axis X of POV(Y in camera)
    */

    //character status
    bool isJumping = false;
    bool isDodging = false;

    public bool isAlive = true;

    //주변 템
    GameObject nearObject;

    void Start()
    {
        //self component import
        //transformSelf = GetComponent<Transform>();
        //controller = GetComponent<CharacterController>();

        //set framerate
        Application.targetFrameRate = 60;
    }
    void FixedUpdate()
    {
        if (!isAlive) return;
        GetInput();
        Rotate();
        Move();
        Dodge();
    }

    void GetInput() //method which is used in getting input
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        rotateX = Input.GetAxis("Mouse X");
        inputWalk = Input.GetButton("Walk");
        inputJump = Input.GetButton("Jump");
        inputDodge = Input.GetButton("Dodge");
    }

    void Move()  //integrated jump and moving control
    {
        if (controller.isGrounded)
        {
            // On the ground, apply regular movement and jump if needed
            moveDirection = new Vector3(inputH * movSpeed * (inputWalk ? 0.3f : 1f), 0f, inputV * movSpeed * (inputWalk ? 0.3f : 1f));
            isJumping = false;
            savedDirection = moveDirection;

            if (inputJump)
            {
                moveDirection.y = jumpSpeed;
                isJumping = true;
            }
        }
        else
        {
            // In the air, apply falling and allow directional input
            moveDirection.x = inputH * movSpeed * (inputWalk ? 0.3f : 1f);
            moveDirection.z = inputV * movSpeed * (inputWalk ? 0.3f : 1f);

            // Apply additional gravity to simulate a more natural fall
            moveDirection.y -= gravity * Time.deltaTime;
        }
        moveDirection = transformSelf.TransformDirection(moveDirection);
        controller.Move(moveDirection * Time.deltaTime);
    }

    void Rotate()
    {
        transformSelf.Rotate((Vector3.up * rotateX * rotSpeed * Time.deltaTime));
    }

    void Dodge()
    {
        if (inputDodge && (moveDirection != Vector3.zero) && !isJumping && !isDodging)
        {
            moveDirection = savedDirection;
            movSpeed *= 2;
            //anim.SetTrigger("doDodging");
            isDodging = true;

            Invoke("DodgeOut", 0.35f);
        }
    }

    void DodgeOut()
    {
        movSpeed *= 0.5f;
        isDodging = false;
    }

    //아이템 입수 관련 콜라이더

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}