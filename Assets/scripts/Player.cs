using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Player : MonoBehaviour, IGravityControl
{
    //game object elements
    public Transform transformSelf;
    //public CharacterController controller; // 이건  IGravityControl 에 있음 

    //physical param variables
    public float jumpSpeed = 10f;
    //public float gravity = 20f;  // 이건  IGravityControl 에 있음 
    public float movSpeed = 20f;
    public float rotSpeed = 400f;
    public float walkSpeedPercentage = 0.35f;
    public float timeCrit;

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

    //item inputs    
    bool inputInteraction; // interaction키 down. 아이템 줍는 입력 e 
    public GameObject[] Items; 
    public bool[] hasItems; // 아이템 가졌는지
    public int itemIndex = -1; // 기본으로 어느것도 선택되지 않도록 
    private bool inputSelect1; // 템 스왑 1
    private bool inputSelect2; // 템 스왑 2
    private bool inputSelect3; // 템 스왑 3
    private bool inputUseItem1;
    private bool inputUseItem2;

    public UnityEvent useItem1;
    public UnityEvent useItem2;

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
    // 아이템 습득 UI 
    public TextMeshProUGUI textMeshProUGUI;

    /// <summary>
    /// 중력 인터페이스 구현부 
    bool isInRange = false; // 중력 범위 내에 있는가 
    public bool IsInRange
    {
        get { return isInRange; }
        set { isInRange = value; }
    }
    public float gravity = -9.81f;

    public float Gravity
    {
        get { return gravity; }
        set { gravity = value; }
    }
    public CharacterController controller; // 컨트롤러


    public void AntiGravity() // 중력 반전 함수 
    {
        IsInRange = true;
        Gravity = 9.81f;
        //Invoke("AntiGravity_End", 3f); // 3초뒤 해제 
        Debug.Log("AntiGravity On.");
    }
    public void AntiGravityEnd()
    {
        IsInRange = false;
        Gravity = -9.81f; // 반전 해제 
        Debug.Log("AntiGravity Off.");
    }
    /// </summary>

    void Start()
    {
        //self component import
        //transformSelf = GetComponent<Transform>();
        //controller = GetComponent<CharacterController>();

        //set framerate
        Application.targetFrameRate = 60;

        //커서 잠금
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void FixedUpdate()
    {
        timeCrit = Time.unscaledDeltaTime;

        if (!isAlive) return;
        //시야조정
        GetInput();
        Rotate();

        //이동
        Move();
        Dodge();

        //아이템 입수+사용
        Interaction();
        Swap();
        UseItem();
    }

    void GetInput() //method which is used in getting input
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        rotateX = Input.GetAxis("Mouse X");
        inputWalk = Input.GetButton("Walk");
        inputJump = Input.GetButton("Jump");
        inputDodge = Input.GetButton("Dodge");
        inputInteraction = Input.GetButtonDown("Interaction"); //e
        inputSelect1 = Input.GetButtonDown("Swap1");
        inputSelect2 = Input.GetButtonDown("Swap2");
        inputSelect3 = Input.GetButtonDown("Swap3");
        inputUseItem1 = Input.GetButtonDown("Effect1"); //r
        inputUseItem2 = Input.GetButtonDown("Effect2"); // f
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
            moveDirection.y += Gravity * timeCrit * 2;
        }
        moveDirection = transformSelf.TransformDirection(moveDirection);
        controller.Move(moveDirection * timeCrit);
    }

    void Rotate()
    {
        transformSelf.Rotate((Vector3.up * rotateX * rotSpeed * timeCrit));
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
        if (inputSelect1 && (!hasItems[0] || equipItemIndex == 0)) // 0번 템을 안갖고 있거나 이미 장착중이면 
            return; // 걍 무시하셈 
        if (inputSelect2 && (!hasItems[1] || equipItemIndex == 1)) 
            return; // 걍 무시하셈 
        if (inputSelect3 && (!hasItems[2] || equipItemIndex == 2))
            return; // 걍 무시하셈 

        if(inputSelect1) itemIndex = 0;
        if(inputSelect2) itemIndex = 1;
        if(inputSelect3) itemIndex = 2;

        if ((inputSelect1 || inputSelect2 || inputSelect3) && !isJumping && !isDodging)
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
        if (nearObject != null && nearObject.tag == "Item")
        {
            //UI 켜기
            textMeshProUGUI.enabled = true;
            if (inputInteraction && !isJumping && !isDodging)
            {
                Item item = nearObject.GetComponent<Item>();
                int itemIndex = item.value; // gravity 0, time 1, wind 2 
                hasItems[itemIndex] = true;
                Destroy(nearObject);
            }
        }
        else
        {
            textMeshProUGUI.enabled = false; // ui 끄기 
        }
    }

    void UseItem()
    {

        if (inputUseItem1)
        {
            //event activation 1
            useItem1.Invoke(); //r
        }
        else if (inputUseItem2)
        {
            useItem2.Invoke(); //f
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