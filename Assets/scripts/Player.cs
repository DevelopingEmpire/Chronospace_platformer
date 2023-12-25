using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public variables
    public float speed = 8.0f;
    public float jumpPwr = 5.0f;
    public float agilityPwr = 5.0f;
    public float rotationSpeed = 10.0f;

    //moving variables
    float hAxis;
    float vAxis;
    float mouseX;
    Vector3 moveVec;
    private Vector3 destVec;
    private Vector3 storedVec;

    //action shift variables
    bool wDown; //walk mode
    bool jDown; //jump mode
    bool dDown; //dodge mode
    bool isJumping;
    bool isDodging;

    //interaction variables
    Rigidbody rigid;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        //anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Jump();
        Dodge();
    }
    void Update()
    {
        GetInput();
        Rotate();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        dDown = Input.GetButtonDown("Dodge");
        mouseX = Input.GetAxis("Mouse X");
    }

    void Move()
    {
        //moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        Vector3 inputVec = new Vector3(hAxis, 0, vAxis).normalized;

        moveVec = transform.TransformDirection(inputVec);
        if (!isJumping && !isDodging){
            storedVec = moveVec;
        }
        else{
            moveVec = storedVec;
        }

        //added lerp smoothing
        //destVec += moveVec * speed * Time.deltaTime * (wDown ? 0.3f : 1f); //walk mode switch
        //transform.position = Vector3.Lerp(transform.position, destVec, agilityPwr * Time.deltaTime);
        //none lerp
        transform.position += moveVec * speed * Time.deltaTime * (wDown ? 0.3f : 1f);
        //anim.SetBool("isRunning", moveVec != Vector3.zero);
        //anim.SetBool("isWalking", wDown);
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up * mouseX * rotationSpeed);
    }

    void Jump()
    {
        if (jDown && !isJumping && !isDodging){
            //jDown && moveVec == Vector3.zero && 
            rigid.AddForce((Vector3.up + storedVec * 0.5f) * jumpPwr, ForceMode.Impulse);
            //anim.SetBool("isJumping", true);
            //anim.SetTrigger("doJumping");
            isJumping = true;
        }
    }

    void Dodge()
    {
        if (dDown && (moveVec != Vector3.zero) && !isJumping){ 
            moveVec = storedVec;
            speed *= 2;
            //anim.SetTrigger("doDodging");
            isDodging = true;

            Invoke("DodgeOut", 0.35f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodging = false;
    }


    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground"){
            isJumping = false;
            //anim.SetBool("isJumping", false);
        }
    }
}