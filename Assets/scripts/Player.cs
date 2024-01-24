using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Player : MonoBehaviour, IGravityControl
{
    //game object elements
    [Header("Component")]
    public Animator anim;
    public CharacterController controller; // 이건  IGravityControl 에 있음 
    public GameObject windKey; // 내 태엽 
    public GameObject throwGravityItem;    // 던질 중력탬  
    public Transform itemPointTransform; // 탬 생성 위치

    [Header("PhysicsValue")]
    //physical param variables
    float jumpForce = 8f;    
    float movSpeed = 5f; 
    float rotSpeed = 300f;

    [Header("InputValue")]
    Vector3 moveDirection;
    private float inputV;
    private float inputH;
    private float rotateX;
    private bool inputWalk;
    private bool inputJump;

    [Header("Item")]   
    bool inputInteraction; // interaction키 down. 아이템 줍는 입력 e 
    public GameObject[] Items; 
    public bool[] hasItems; // 아이템 가졌는지
    private bool inputKeyButton1; // 템 스왑 1
    private bool inputKeyButton2; // 템 스왑 2
    private bool inputKeyButton3; // 템 스왑 3
    private bool inputKeyR;

    bool isSwaping = false; //사용하고 있는 변수. Console에 사용하지 않는다고 뜬다. 

    #region animValue
    //character status
    bool isJumping = false;
    bool isDodging = false;
    bool isWinding = false; 
    #endregion

    bool isPlayerNear = false; // 주변에 동료가 있는가 
    public bool isAlive = true;

    //주변 템
    [SerializeField]
    GameObject nearObject;
    GameObject equipItem; // 현재 손에 들고있는 아이템 
    public Item.Type equipItemIndex = Item.Type.Null; // 현재 손에 있는 탬 종류 
    public float timeScaleMultiplier = 0.5f; // 타임 스케일 계수 

    // 아이템 습득 UI 
    public TextMeshProUGUI textMeshProUGUI;

    /// <summary>
    /// 중력 인터페이스 구현부 
    public bool IsInRange { get; set; }

    public float Gravity { get; set; }

    
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
        //set framerate
        Application.targetFrameRate = 60;

        Gravity = -9.81f;

        controller.detectCollisions = false;
    }
    void FixedUpdate()
    {
        if (!isAlive)  return;
        if (isWinding) return; // 와인딩 중엔 암것도 못해! 
        
        //시야조정
        GetInput();
        Rotate();

        //이동
        Move();

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

        inputInteraction = Input.GetButtonDown("Interaction"); //e
        inputKeyButton1 = Input.GetButtonDown("Swap1");
        inputKeyButton2 = Input.GetButtonDown("Swap2");
        inputKeyButton3 = Input.GetButtonDown("Swap3");
        inputKeyR = Input.GetButtonDown("Effect1"); //r
    }

    void Move()  //integrated jump and moving control
    {
        if (controller.isGrounded)
        {
            isJumping = false;
            moveDirection = new Vector3(inputH * movSpeed * (inputWalk ? 0.3f : 1f), 0f, inputV * movSpeed * (inputWalk ? 0.3f : 1f)) ;

            if (inputJump)
            {
                moveDirection.y = jumpForce;
                isJumping = true;
                anim.SetTrigger("doJump");
            }
        }
        else
        {
            // In the air, apply falling and allow directional input
            moveDirection.x = inputH * movSpeed * (inputWalk ? 0.3f : 1f);
            moveDirection.z = inputV * movSpeed * (inputWalk ? 0.3f : 1f);

            // Apply additional gravity to simulate a more natural fall
            moveDirection.y += Gravity * Time.unscaledDeltaTime * 2;
        }
        moveDirection = transform.TransformDirection(moveDirection);
        controller.Move(moveDirection * Time.unscaledDeltaTime);

        bool isRunning = moveDirection.x != 0f || moveDirection.z != 0f; // You might need to adjust this condition based on your needs
        anim.SetBool("isRunning", isRunning); // Set the isMove parameter in the Animator
    }

    void Rotate()
    {
        transform.Rotate((Vector3.up * rotateX * rotSpeed * Time.unscaledDeltaTime));
    }

    #region Item

    void Swap()
    {
        if ((inputKeyButton1 && hasItems[0]) || (inputKeyButton2 && hasItems[1]) || (inputKeyButton3 && hasItems[2]))
        {
            // Deactivate current equipItem
            if (equipItem != null) equipItem.SetActive(false);

            // Update equipItem based on input
            if (inputKeyButton1)
            {
                equipItemIndex = Item.Type.Gravity;
            }
            else if (inputKeyButton2)
            {
                equipItemIndex = Item.Type.TimeStop;
            }
            else if (inputKeyButton3)
            {
                equipItemIndex = Item.Type.WindKey;
            }

            // Activate new equipItem and store its reference
            equipItem = Items[(int)equipItemIndex];
            equipItem.SetActive(true);

            isSwaping = true;
            Invoke("SwapOut", 0.4f); // Assuming you want to perform some post-swap logic
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
            Debug.Log("isJumping" + isJumping);
            if (inputInteraction)
            {
                Item item = nearObject.GetComponent<Item>();
                int itemIndex = (int)item.type; // gravity 0, time 1, wind 2 
                Debug.Log("itemIndex" + itemIndex);
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
        if (!inputKeyR) return;

        switch (equipItemIndex)
        {
            case Item.Type.Gravity:
                Instantiate(throwGravityItem, itemPointTransform.position + itemPointTransform.forward
                    , itemPointTransform.rotation);
                break;

            case Item.Type.TimeStop:
                TweakTimeStart(timeScaleMultiplier);
                Invoke("TweakTimeEnd", 5 * Time.unscaledTime); // Adjust based on the intended effect duration
                break;

            case Item.Type.WindKey:
                if (nearObject != null && isPlayerNear == true)  
                {
                    nearObject.GetComponent<Player>().WindKeyActivate();
                }
                break;

            case Item.Type.Null:
                // Handle no item selected
                break;
            default:
                Debug.LogError("Unknown item type: " + equipItemIndex);
                break;
        }
    }

    public void TweakTimeStart(float timeScaleMultiplier)
    {
        Time.timeScale = timeScaleMultiplier;
        //Time.fixedDeltaTime = Time.timeScale * (1 / Application.targetFrameRate);
        OnCallBackTweakTimeStart();
    }
    public void TweakTimeEnd()
    {
        Time.timeScale = 1.0f;

        OnCallBackTweakTimeEnd();
    }
    public void OnCallBackTweakTimeStart() // CallBack을 그냥 Method Call로 대체 
    {
        anim.speed = 1.0f / Time.timeScale; // 애니메이션 속도도 바꿔준다
        // 추가적으로 처리해줘야 할 부분들 
    }
    public void OnCallBackTweakTimeEnd() // CallBack을 그냥 Method Call로 대체 
    {
        anim.speed = 1.0f;
        // 추가적으로 처리해줘야 할 부분들
    }

    //Winding
    public void WindKeyActivate() // 활성화되면서 
    {
        windKey.SetActive(true); 
        
    }

    // 필수 조건 
    // 1. 둘 중에 하나에 무조건 rigidbody
    // 2. 둘 중에 하나에 무조건 isTrigger 체크 
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item")) nearObject = other.gameObject;

        if (other.CompareTag("Player")) //플레이어면 
        {
            nearObject = other.gameObject;
            isPlayerNear = true;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item")) nearObject = null;

        if (other.CompareTag("Player"))
        {
            nearObject = null;
            isPlayerNear = false;
        }
    }

    #endregion
}