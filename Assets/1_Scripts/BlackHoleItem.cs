using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHoleItem : MonoBehaviour
{
    public GameObject itemMeshObj; // 이름이 아이템 어쩌구로 수정
    //public GameObject effectObj; // 폭발 효과 
    public Rigidbody rb;
    public SphereCollider colliderRange; // 탬 적용 범위 콜라이더 
    public MeshRenderer meshRenderer; // 템 범위 mesh 
    public float duration = 10f; // 템 범위 mesh 
    public float throwforce = 18f; // 템 범위 mesh 
    public float brakeAfter = 1.2f;
    public float rotationSpeed = 5.0f;

    // 적용 당할 오브젝트의 itemGravityControl
    IGravityControl iGravityControl;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private float timeElapsed = 0f;
    public List<GameObject> objInBlackHoleRange;

    public float rotationY = 45f; // Added to store the accumulated vertical rotation

    private void Awake()
    {
        //rb.AddForce(((transform.forward * math.sin(rotationY) + transform.up * math.cos(rotationY))) * throwforce, ForceMode.Impulse); // 앞으로 슝~
        rb.AddForce((transform.forward) * throwforce, ForceMode.Impulse); // 앞으로 슝~
        StartCoroutine(Explosion());
        StartCoroutine(Brake());
    }

    // 시간차를 위해 코루틴으로
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(duration); // 1초 길어서 줄임  

        // 물리적인 속도들 모두 0으로 해줌 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false; // 리지드의 중력 끄기
        if(itemMeshObj != null) {
            itemMeshObj.SetActive(false); // 비활성화 
        }
        //effectObj.SetActive(true); // 효과 보여주는거
        colliderRange.enabled = true; // 콜라이더 켜기 
        meshRenderer.enabled = true;

        yield return new WaitForSeconds(duration); // 4초 대기

        foreach(GameObject obj in objInBlackHoleRange){
            iGravityControl = obj.GetComponent<IGravityControl>();
            if(iGravityControl != null) {
                iGravityControl.AntiGravityEnd();
            }
        }

        Destroy(transform.parent.gameObject); // 아이템 clone 삭제 
    }

    private void OnTriggerStay(Collider col) // 매 프레임 실행되도록 이렇게 해줘봄 
    {
        iGravityControl = col.GetComponent<IGravityControl>();

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (iGravityControl != null && !col.gameObject.CompareTag("Player"))
        {
            //해당 스크립트가 있는 놈이면?
            iGravityControl.BlackHole(transform.position);
        }
        else{
            if((!col.gameObject.CompareTag("Untagged") && timeElapsed >= 0.5f)){
                //col.gameObject.CompareTag("Untagged")
                BrakeNow();
            }
        }

        objInBlackHoleRange.Add(col.GameObject());
    }

    private IEnumerator Brake()
    {
        // 1초 동안 물체의 움직임을 지켜봄
        yield return new WaitForSeconds(brakeAfter);
        BrakeNow();
    }

    private void BrakeNow(){
        // 마지막 위치와 회전을 저장
        lastPosition = transform.position;
        lastRotation = transform.rotation;

        // Rigidbody의 움직임을 중지시킴
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; // 물리적인 상호작용을 종료하고 물체를 고정

        // 물체를 마지막 위치와 회전에 고정
        transform.position = lastPosition;
        transform.rotation = lastRotation;
    }

    private void Update() {
        timeElapsed += Time.deltaTime;
    }
}
