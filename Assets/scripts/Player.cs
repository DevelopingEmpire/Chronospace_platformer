using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //game object elements
    Transform tr;
    CharacterController controller;
    Vector3 moveDirection;

    //physical param variables
    public float jumpSpeed = 10f;
    public float gravity = 20f;
    public float movSpeed = 20f;
    public float rotSpeed = 400f;
    public float walkSpeedPercentage = 0.35f;
    public GameObject spawnPoint;
    public GameObject goalPoint;

    //movement param variables
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
    Vector3 resetCoordinate = Vector3.zero;
    Vector3 completeCoordinate = Vector3.zero;

    void Start()
    {
        //self component import
        tr = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();

        //set framerate
        Application.targetFrameRate = 60;

        //set reset and complete pos ref
        spawnPoint = GameObject.Find("Spawnpoint_1P");
        resetCoordinate = spawnPoint.transform.position;
        goalPoint = GameObject.Find("Goalpoint_1P");
        completeCoordinate = goalPoint.transform.position;
    }
    void FixedUpdate()
    {
        GetInput();
        Rotate();

        Move();
    }

    void GetInput() //method which is used in getting input
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        rotateX = Input.GetAxis("Mouse X");
        inputWalk = Input.GetButton("Walk");
        inputJump = Input.GetButton("Jump");
        //inputDodge = Input.GetButton("Dodge");
    }

    void Move()  //integrated jump and moving control
    {
        if (controller.isGrounded)
        {
            // On the ground, apply regular movement and jump if needed
            moveDirection = new Vector3(inputH * movSpeed * (inputWalk ? 0.3f : 1f), 0f, inputV * movSpeed * (inputWalk ? 0.3f : 1f));

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
        moveDirection = tr.TransformDirection(moveDirection);
        controller.Move(moveDirection * Time.deltaTime);
    }

    void Rotate()
    {
        tr.Rotate((Vector3.up * rotateX * rotSpeed * Time.deltaTime));
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("ResetTrigger"))
        {
            // Move the player to the reset position
            controller.enabled = false;
            transform.position = resetCoordinate;
            controller.enabled = true;
        }
    }
}