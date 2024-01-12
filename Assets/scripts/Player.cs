using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Player : MonoBehaviour, IGravityControl
{
    //game object elements
    public Transform transformSelf;
    //public CharacterController controller; // �̰�  IGravityControl �� ���� 

    //physical param variables
    public float jumpSpeed = 10f;
    //public float gravity = 20f;  // �̰�  IGravityControl �� ���� 
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
    bool inputInteraction; // interactionŰ down. ������ �ݴ� �Է� e 
    public GameObject[] Items; 
    public bool[] hasItems; // ������ ��������
    public int itemIndex = -1; // �⺻���� ����͵� ���õ��� �ʵ��� 
    private bool inputSelect1; // �� ���� 1
    private bool inputSelect2; // �� ���� 2
    private bool inputSelect3; // �� ���� 3
    private bool inputUseItem1;
    private bool inputUseItem2;

    public UnityEvent useItem1;
    public UnityEvent useItem2;

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
    // ������ ���� UI 
    public TextMeshProUGUI textMeshProUGUI;

    /// <summary>
    /// �߷� �������̽� ������ 
    bool isInRange = false; // �߷� ���� ���� �ִ°� 
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
    public CharacterController controller; // ��Ʈ�ѷ�


    public void AntiGravity() // �߷� ���� �Լ� 
    {
        IsInRange = true;
        Gravity = 9.81f;
        //Invoke("AntiGravity_End", 3f); // 3�ʵ� ���� 
        Debug.Log("AntiGravity On.");
    }
    public void AntiGravityEnd()
    {
        IsInRange = false;
        Gravity = -9.81f; // ���� ���� 
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

        //Ŀ�� ���
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void FixedUpdate()
    {
        timeCrit = Time.unscaledDeltaTime;

        if (!isAlive) return;
        //�þ�����
        GetInput();
        Rotate();

        //�̵�
        Move();
        Dodge();

        //������ �Լ�+���
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

    // ������ ���� 
    void Swap()
    {
        if (inputSelect1 && (!hasItems[0] || equipItemIndex == 0)) // 0�� ���� �Ȱ��� �ְų� �̹� �������̸� 
            return; // �� �����ϼ� 
        if (inputSelect2 && (!hasItems[1] || equipItemIndex == 1)) 
            return; // �� �����ϼ� 
        if (inputSelect3 && (!hasItems[2] || equipItemIndex == 2))
            return; // �� �����ϼ� 

        if(inputSelect1) itemIndex = 0;
        if(inputSelect2) itemIndex = 1;
        if(inputSelect3) itemIndex = 2;

        if ((inputSelect1 || inputSelect2 || inputSelect3) && !isJumping && !isDodging)
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
        if (nearObject != null && nearObject.tag == "Item")
        {
            //UI �ѱ�
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
            textMeshProUGUI.enabled = false; // ui ���� 
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