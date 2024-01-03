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
    
    bool idown; // interactionŰ down. ������ �ݴ� �Է� e 
    public GameObject[] Items; 
    public bool[] hasItems; // ������ ��������
    bool sdown1; // �� ���� 1
    bool sdown2; // �� ���� 2
    bool sdown3; // �� ���� 3

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
    bool isSwaping = false;

    //ĳ���Ͱ� ��Ҵ���! ( �ð�) . is TimeOver�� �̸� �ٲܱ� �ͳ� 
    public bool isAlive = true;

    //�ֺ� ��
    GameObject nearObject;
    GameObject equipItem; // ���� �տ� ����ִ� ������ 
    int equipItemIndex = -1; // ���� �տ� �ִ� �� ���� 

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
        Interaction();
        Swap();
    }

    void GetInput() //method which is used in getting input
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        rotateX = Input.GetAxis("Mouse X");
        inputWalk = Input.GetButton("Walk");
        inputJump = Input.GetButton("Jump");
        inputDodge = Input.GetButton("Dodge");
        idown = Input.GetButtonDown("Interaction");
        sdown1 = Input.GetButtonDown("Swap1");
        sdown2 = Input.GetButtonDown("Swap2");
        sdown3 = Input.GetButtonDown("Swap3");
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

    // ������ ���� 
    void Swap()
    {
        if (sdown1 && (!hasItems[0] || equipItemIndex == 0)) // 0�� ���� �Ȱ��� �ְų� �̹� �������̸� 
            return; // �� �����ϼ� 
        if (sdown2 && (!hasItems[1] || equipItemIndex == 1)) 
            return; // �� �����ϼ� 
        if (sdown3 && (!hasItems[2] || equipItemIndex == 2))
            return; // �� �����ϼ� 

        int itemIndex = -1; // �⺻���� ����͵� ���õ��� �ʵ��� 
        if(sdown1) itemIndex = 0;
        if(sdown2) itemIndex = 1;
        if(sdown3) itemIndex = 2;

        if ((sdown1 || sdown2 || sdown3) && !isJumping && !isDodging)
        {
            // �տ� �̹� ���õ� ���� ���� �� ��Ȱ��ȭ 
            if(equipItem != null)
                equipItem.SetActive(false);

            equipItemIndex = itemIndex;
            equipItem = Items[itemIndex];
            equipItem.SetActive(true);

            // �ִϸ��̼� ���� �ڵ� 

            isSwaping = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    //swap() ������ 
    void SwapOut()
    {
        isSwaping = false;
    }

    //interaction . ������ ��ȣ�ۿ� Ű 
    void Interaction()
    {
        if(idown && nearObject != null && !isJumping && !isDodging)
        {
            if (nearObject.tag == "Item")
            {
                Item item = nearObject.GetComponent<Item>();
                int itemIndex = item.value; // gravity 0, time 1, wind 2 
                hasItems[itemIndex] = true;

                Destroy(nearObject);
            }
        }
    }

    //������ �Լ� ���� �ݶ��̴�

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Item")
            nearObject = other.gameObject;
        //Debug.Log(nearObject.name);  // ��� �ߵȴ�! 
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
            nearObject = null;
               
           
    }
}