using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IGravityControl
{
    [Header("Component")] //플레이어 외부 컴포넌트 변수
    public Animator anim;
    public CharacterController controller; // 이건  IGravityControl 에 있음 
    public GameObject windKey; // 내 태엽 
    public GameObject[] gravityPrefebs;  // 던질 중력반전, // 던질 중력장  
    public float[] timeScaleMultiplier = new float[] {0.25f, 0.005f }; // 시간 계수 // roh 가라사대 감으로 값을 정했다 하시느니라 
    public Transform itemPointTransform; // 탬 생성 위치
    public PlayerTimer timer;
    public Vector3 respawnPosition; // 리스폰 위치 

    [Header("PhysicsValue")] //플레이어 물리 효과 컨트롤 변수
    public float jumpForce = 8f;
    public float movSpeed = 5f;
    public float rotSpeed = 300f;
    public float pushPower = 0.03f;

    [Header("InputValue")] //플레이어 이동 사용 변수
    Vector3 moveDirection;
    private float inputV;
    private float inputH;
    private float rotateX;
    private bool inputWalk;
    private bool inputJump;
    private float directionInput;

    [Header("Item")] //플레이어 아이템 사용 변수
    bool inputInteraction; // interaction키 down. 아이템 줍는 입력 e 
    public GameObject[] Items; 
    public bool[] hasItems; // 아이템 가졌는지
    private bool inputKeyButton1; // 템 스왑 1
    private bool inputKeyButton2; // 템 스왑 2
    private bool inputKeyButton3; // 템 스왑 3
    private bool inputKeyR; // 탬사용1
    private bool inputKeyF; // 탬사용2

    bool isSwaping = false; //사용하고 있는 변수. Console에 사용하지 않는다고 뜬다. 

    //플레이어 애니메이션 작동 변수
    #region animValue
    //character status
    bool isJumping = false;
    bool isDodging = false;
    bool isWinding = false; 
    #endregion

    //플레이어 상태 변수
    //bool isPlayerNear = false; // 주변에 동료가 있는가 
    public bool isAlive = true;
    public bool isBlackHoling; // 블랙홀에 잡혀있는 중 

    //주변 아이템 변수
    [SerializeField]
    public GameObject nearObject;
    GameObject equipItem; // 현재 손에 들고있는 아이템 
    public Item.Type equipItemIndex = Item.Type.Null; // 현재 손에 있는 탬 종류 

    // 아이템 습득 UI 
    public TextMeshProUGUI textMeshProUGUI;

    private Vector3 blackholeVector = Vector3.zero; // 블랙홀 힘 저장 
    /// <summary>
    /// 중력 인터페이스 구현부 
    public bool IsInRange { get; set; }

    public float Gravity { get; set; }

    // 플레이어를 싱글톤으로??
    #region SingleTon Pattern
    public static Player Instance { get; private set; }
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Set this as the instance and ensure it persists across scenes
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

    }

    #endregion

    public void AntiGravity() // 중력 반전 함수 
    {
        IsInRange = true;
        Gravity = 9.81f;
        //Invoke("AntiGravity_End", 3f); // 3초뒤 해제 
        Debug.Log("AntiGravity On.");
    }
    public void AntiGravityEnd() //중력 반전 종료 함수
    {
        IsInRange = false;
        Gravity = -9.81f; // 반전 해제 
        Debug.Log("AntiGravity Off.");
    }
    /// </summary>


    void Update() //플레이어 상태 관리 함수
    {
        GetInput();
        if (!isAlive)  return;
        if (isWinding) return; //플레이어가 살아있지 않거나, 플레이어가 윈드 업 중일 때에는 동작 불가능

        SetDir();
        if (inputInteraction) Interaction(); //interaction item이 주변에 있을 때 상호작용 활성화
        if(inputKeyButton1 || inputKeyButton2 || inputKeyButton3) Swap(); //input key 버튼으로 아이템 선택 활성화
        if (inputKeyR) //아이템 사용(모드 0, 모드 1로 이원화)
        {
            UseItem(0);
        }
        if (inputKeyF)
        {
            UseItem(1);
        }
        //Debug.Log("R"+inputKeyR); Debug.Log("F" + inputKeyF); //디버깅
    }
    void FixedUpdate() //플레이어 행동 관리 함수
    {
        if (!isAlive)  return;
        if (isWinding) return; //플레이어가 살아있지 않거나, 플레이어가 윈드 업 중일 때에는 동작 불가능

        Rotate();
        Move();
        
    }
    void GetInput() //인풋 받는 함수
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical"); 
        rotateX = Input.GetAxis("Mouse X");
        inputWalk = Input.GetButton("Walk"); // -> Dash
        inputJump = Input.GetButton("Jump");

        inputInteraction = Input.GetButtonDown("Interaction"); //e
        inputKeyButton1 = Input.GetButtonDown("Swap1");
        inputKeyButton2 = Input.GetButtonDown("Swap2");
        inputKeyButton3 = Input.GetButtonDown("Swap3");
        inputKeyR = Input.GetButtonDown("Effect1"); //r
        inputKeyF = Input.GetButtonDown("Effect2"); //f
    }

    void SetDir() //이동 함수
    {
        float targetDirectionInput = 0f;

        if (inputV > 0.1 && inputH == 0) targetDirectionInput = 0f;
        else if (inputV > 0.1 && inputH < 0) targetDirectionInput = 0.125f;
        else if (inputV == 0 && inputH < 0) targetDirectionInput = 0.25f;
        else if (inputV < -0.1 && inputH > 0) targetDirectionInput = 0.375f;
        else if (inputV < -0.1 && inputH == 0) targetDirectionInput = 0.5f;
        else if (inputV < -0.1 && inputH < 0) targetDirectionInput = 0.625f;
        else if (inputV == 0 && inputH > 0) targetDirectionInput = 0.75f;
        else if (inputV > 0.1 && inputH > 0) targetDirectionInput = 0.875f;

        // Smoothly interpolate the current value to the target value
        float smoothness = 0.1f; // Adjust this value for the desired smoothness
        directionInput = Mathf.Lerp(directionInput, targetDirectionInput, smoothness);

        anim.SetFloat("blendvarRun", directionInput);
    }

    void Move() //이동 함수
    {
        if (controller.isGrounded || isBlackHoling)
        {
            isJumping = false;
            moveDirection = new Vector3(inputH * movSpeed * (inputWalk ? 0.3f : 1f), 0f, inputV * movSpeed * (inputWalk ? 0.3f : 1f));

            if (inputJump)
            {
                moveDirection.y += jumpForce;
                isJumping = true;
                anim.SetTrigger("doJump");
            }
        }
        else
        {
            // 공중에 있을 때의 움직임 조정, Time.deltaTime을 곱하여 일관된 속도 유지
            moveDirection.x = inputH * movSpeed * (inputWalk ? 0.3f : 1f);
            moveDirection.z = inputV * movSpeed * (inputWalk ? 0.3f : 1f);

            // 대각선 노말라이제이션 
            
            // 추가 중력 적용
            moveDirection.y += Gravity *2* Time.unscaledDeltaTime; // 중력 적용에도 Time.deltaTime을 사용
        }
        moveDirection = transform.TransformDirection(moveDirection);
        controller.Move(moveDirection * Time.unscaledDeltaTime); // 이동 명령에 Time.deltaTime 적용

        bool isRunning = Mathf.Abs(inputH) > 0f || Mathf.Abs(inputV) > 0f; // 입력에 따른 달리기 상태 결정
        anim.SetBool("isRunning", isRunning); // Animator에 달리기 상태 전달

        // 끌어당김 상태 업데이트
        isBlackHoling = false;
    }

    void Rotate()
    {
        transform.Rotate((Vector3.up * rotateX * rotSpeed * Time.unscaledDeltaTime));
    }

    // 사망
    public void Die()
    {
        // 사망 처리
        isAlive = false;
        // anim.SetTrigger("die"); // 애니메이션 트리거 설정 
 

        // 사망 후 일정 시간 후에 시작 위치로 이동
        StartCoroutine(Respawn());
    }

    // 저장 위치에서 부활 
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f); // 사망 후 2초 대기 (옵션)

        // 플레이어 위치 초기화
        PlayerInit();

        // 플레이어 상태 초기화
        isAlive = true;
    }


    //아이템 사용 함수
    #region Item

    void Swap() //아이템 스왑 함수
    {
        if ((inputKeyButton1 && hasItems[0]) || (inputKeyButton2 && hasItems[1]) || (inputKeyButton3 && hasItems[2]))
            //아이템 선택 키를 눌렀음&아이템이 있을 때에만 아이템 꺼내 주기
        {
            Debug.Log("스왑 눌림");
            // Deactivate current equipItem
            if (equipItem != null) equipItem.SetActive(false); //없으면 안 꺼내주기

            // 각 번호에 맞는 아이템 꺼내주기
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

            //아이템 인덱스에 따라서 아이템을 지정해 줌
            equipItem = Items[(int)equipItemIndex];
            equipItem.SetActive(true);

            // 아이템 UI에 장착 장비 표시
            UIManager.instance.equipItemUI(equipItemIndex); // 해당하는 아이템 장착 ui 표시 

            isSwaping = true;
            Invoke("SwapOut", 0.4f); // Assuming you want to perform some post-swap logic
        }
    }

    void SwapOut() //스왑 끝내기
    {
        isSwaping = false;
    }

    // 플레이어 상태 초기화 
    public void PlayerInit()
    {
        
        moveDirection = Vector3.zero; // 이 값 임의로 초기화 
        controller.enabled = false; // 잠시 끄고 

        transform.position = respawnPosition;  // 현재 체크포인트에서 시작. 처음엔 처음 위치임 

        controller.enabled = true; // 다시 켠다 

        // 걍 설정 
        Application.targetFrameRate = 50; // 서주민 전용코드 
        
        Gravity = -9.81f;
        
        //controller.detectCollisions = false; // 이거 끄면.. 플레이어가 발판을 못 밟음 

        if(StageManager.Instance.currentStageName == "stage0") timer.isPlaying = false;
        else timer.isPlaying = true;
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        respawnPosition = checkpointPosition;
    }

    void Interaction() //아이템 줍기
    {
        if (nearObject != null && nearObject.tag == "Item")
        {
            textMeshProUGUI.enabled = false; // ui 끄기 
            Item item = nearObject.GetComponent<Item>();
            int itemIndex = (int)item.type; // gravity 0, time 1, wind 2 
            //Debug.Log("itemIndex" + itemIndex);
            hasItems[itemIndex] = true;  //아이템 인덱스 활성화(활성화된 인덱스에서의 아이템 꺼내기가 활성화됨)
            UIManager.instance.hasItemUI(item.type, true);

            Destroy(nearObject); //지역에 떨어진 아이템 삭제하기
        }
        else if (nearObject != null && nearObject.tag == "Switch")
        {
            textMeshProUGUI.enabled = false; // ui 끄기 
            SwitchTrigger targetSwitchScript = nearObject.GetComponent<SwitchTrigger>();
            if (targetSwitchScript != null)
            {
                targetSwitchScript.Activate();
            }
            else
            {
                Debug.LogError("Switch Object" + nearObject + "Has no script for functioning.");
            }
        }
    }

    void UseItem(int RFnum) // R이면 0, F면 1 이 전달됨 
    {
        switch (equipItemIndex)
        {
            case Item.Type.Gravity: //중력(중력 적용장치 인스턴스를 생성한 다음 정해진 방향으로 투척
                Instantiate(gravityPrefebs[RFnum], itemPointTransform.position + itemPointTransform.forward, itemPointTransform.rotation);

                break;

            case Item.Type.TimeStop: //시간 정지(입력에 따라서 시간 속도를 조절함
                StartCoroutine(TweakTimeEffect(timeScaleMultiplier[RFnum], 5));
                break;

            case Item.Type.WindKey: //윈드 키(다른 플레이어가 존재할 때에만 활성화됨
                timer.TimeChange(30f); // 30초 추가 
                /*
                if (nearObject != null && isPlayerNear == true)  
                {
                    nearObject.GetComponent<Player>().WindKeyActivate();
                }*/
                
                break;

            case Item.Type.Null: //없음
                // Handle no item selected
                return;

            default: //모든 확인할 수 없는 아이템이 잡혀 있는 경우 에러 메시지 전달
                Debug.LogError("Unknown item type: " + equipItemIndex);
                return;
        }

        //사용 후 처리들 
        
        UIManager.instance.hasItemUI(equipItemIndex, false);
        hasItems[(int)equipItemIndex] = false;
        equipItem.SetActive(false);
        equipItemIndex = Item.Type.Null;        // 사용시 사라진다 
        UIManager.instance.equipItemUI(equipItemIndex); // itemFrame 꺼진다 
    }


    IEnumerator TweakTimeEffect(float scale, float duration)
    {
        TweakTimeStart(scale); //지정된 값만큼 시간 속도를 조절
        yield return new WaitForSecondsRealtime(duration); // Unscaled time을 사용해 실제 흐르는 시간을 측정하고 이후에 시간 조작 비활성화
        TweakTimeEnd();
    }

    public void TweakTimeStart(float timeScaleMultiplier) //시간 조작 활성화
    {
        Time.timeScale = timeScaleMultiplier;
        //Time.fixedDeltaTime = Time.timeScale * (1 / Application.targetFrameRate);
        OnCallBackTweakTimeStart();
    }
    public void TweakTimeEnd() //시간 조작 비활성화
    {
        Time.timeScale = 1.0f;

        OnCallBackTweakTimeEnd();
    }
    public void OnCallBackTweakTimeStart() // CallBack을 그냥 Method Call로 대체 
    {
        anim.speed = 1.0f / Time.timeScale; // 애니메이션 속도도 바꿔준다
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    public void OnCallBackTweakTimeEnd() // CallBack을 그냥 Method Call로 대체 
    {
        anim.speed = 1.0f;
        Time.fixedDeltaTime = 0.02f;
        // 추가적으로 처리해줘야 할 부분들
        // 추가 처리 더 없다면, 위 함수와 합쳐줘도 될듯 
    }


    //Winding
    public void WindKeyActivate() // 활성화되면서 
    {
        windKey.SetActive(true); 
    }

    // 필수 조건 
    // 1. 둘 중에 하나에 무조건 rigidbody
    // 2. 둘 중에 하나에 무조건 isTrigger 체크 
    private void OnTriggerStay(Collider other) //플레이어가 윈드키 영향을 줄 수 있는 범위에 있을 때
    {
        if (other.CompareTag("Item")) //태엽으로 돌릴 수 있는 아이템의 경우 근처 오브젝트를 활성화시킬 수 있다는 메시지 전송
        {
            //UI 켜기
            textMeshProUGUI.enabled = true;
            nearObject = other.gameObject;
        }
        else if (other.CompareTag("Switch")) //스위치이면 활성화 준비
        {
            textMeshProUGUI.enabled = true;
            nearObject = other.gameObject;
        }
        if (other.CompareTag("CheckPoint"))
        {
            respawnPosition = other.transform.position;
            Debug.Log("Checkpoint reached: " + respawnPosition);
        }
        /*
        else if (other.CompareTag("Player")) //플레이어면 플레이어임을 확인하고 true
        {
            nearObject = other.gameObject;
            isPlayerNear = true;
        }
        */


    }

    private void OnTriggerExit(Collider other) //플레이어가 윈드키 영향을 줄 수 있는 범위를 벗어났을 때
    {
        if (other.CompareTag("Item"))
        {
            textMeshProUGUI.enabled = false; // ui 끄기 
            nearObject = null;
        }
        else if (other.CompareTag("Switch")) //플레이어면 플레이어임을 확인하고 true
        {
            textMeshProUGUI.enabled = false; // ui 끄기 
            nearObject = null;
        }
        /*
        else if(other.CompareTag("Player"))
        {
            nearObject = null;
            isPlayerNear = false;
        }
        */
    }

    // 상자 밀기 
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CharacterController hitController = hit.gameObject.GetComponent(typeof(CharacterController)) as CharacterController;
        
        if (hitController)
        {
            // hitController.SimpleMove(moveDirection);
            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            hitController.Move(pushDirection * pushPower); // * Time.deltaTime
        }
    }
    public void BlackHole(Vector3 fieldCenter)
    {
        isBlackHoling = true;
        blackholeVector = fieldCenter - transform.position; // + new Vector3(0f,1.3f,0f)
        //blackholeVector = Vector3.Normalize(blackholeVector); // 방향만 구함 
        controller.Move(blackholeVector* Time.deltaTime); //* blackholeStrength *Time.unscaledDeltaTime

    }

    #endregion
}