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
    
    bool idown; // interaction키 down. 아이템 줍는 입력 e 
    public GameObject[] Items; 
    public bool[] hasItems; // 아이템 가졌는지
    bool sdown1; // 템 스왑 1
    bool sdown2; // 템 스왑 2
    bool sdown3; // 템 스왑 3

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

    //캐릭터가 살았는지! ( 시간) . is TimeOver로 이름 바꿀까 싶네 
    public bool isAlive = true;

    //주변 템
    GameObject nearObject;
    GameObject equipItem; // 현재 손에 들고있는 아이템 
    int equipItemIndex = -1; // 현재 손에 있는 탬 종류 

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

    // 아이템 스왑 
    void Swap()
    {
        if (sdown1 && (!hasItems[0] || equipItemIndex == 0)) // 0번 템을 안갖고 있거나 이미 장착중이면 
            return; // 걍 무시하셈 
        if (sdown2 && (!hasItems[1] || equipItemIndex == 1)) 
            return; // 걍 무시하셈 
        if (sdown3 && (!hasItems[2] || equipItemIndex == 2))
            return; // 걍 무시하셈 

        int itemIndex = -1; // 기본으로 어느것도 선택되지 않도록 
        if(sdown1) itemIndex = 0;
        if(sdown2) itemIndex = 1;
        if(sdown3) itemIndex = 2;

        if ((sdown1 || sdown2 || sdown3) && !isJumping && !isDodging)
        {
            // 손에 이미 선택된 탬이 있을 땐 비활성화 
            if(equipItem != null)
                equipItem.SetActive(false);

            equipItemIndex = itemIndex;
            equipItem = Items[itemIndex];
            equipItem.SetActive(true);

            // 애니매이션 관련 코드 

            isSwaping = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    //swap() 끝날때 
    void SwapOut()
    {
        isSwaping = false;
    }

    //interaction . 아이템 상호작용 키 
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

    //아이템 입수 관련 콜라이더

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Item")
            nearObject = other.gameObject;
        //Debug.Log(nearObject.name);  // 출력 잘된다! 
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
            nearObject = null;
               
           
    }
}