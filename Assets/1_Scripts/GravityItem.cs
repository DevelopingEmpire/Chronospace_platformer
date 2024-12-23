using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityItem : MonoBehaviour
{
    public GameObject itemMeshObj; // 이름이 아이템 어쩌구로 수정
    public Rigidbody rb;
    public BoxCollider colliderRange; // 탬 적용 범위 콜라이더 
    public MeshRenderer meshRenderer; // 템 범위 mesh 

    // 적용 당할 오브젝트의 itemGravityControl
    IGravityControl iGravityControl;

    public float effectDuration = 5f;

    // colliderRange 내의 col 들 모아두는 list 
    public List<Collider> colInRange = new List<Collider>();


    private void Awake()
    {
        // 카메라가 보는 방향으로 아이템이 날아가도록 설정
        Vector3 cameraForward = Camera.main.transform.forward; // 카메라의 앞방향
        rb.AddForce(cameraForward * 10, ForceMode.Impulse); // 카메라가 보는 방향으로 힘을 가함

        StartCoroutine(Explosion());
    }

    // 시간차를 위해 코루틴으로
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.5f); // 1초 길어서 줄임  

        // 물리적인 속도들 모두 0으로 해줌 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false; // 리지드의 중력 끄기 

        // 켜짐 
        colliderRange.enabled = true; // 콜라이더 켜기 
        meshRenderer.enabled = true;  // 효과 보여주기 

        yield return new WaitForSeconds(effectDuration); // effectDuration 동안 대기 

        // 남은 놈들도 싹 정리해주기 
        foreach (Collider col in colInRange)
        {
            iGravityControl = col.GetComponent<IGravityControl>();
            if (iGravityControl != null)
            {
                iGravityControl.AntiGravityEnd();
                Debug.Log(col + "의 Antigravity 해제");
            }
        }

        Destroy(transform.parent.gameObject); // 아이템 clone 삭제 
    }

    private void OnTriggerEnter(Collider col)
    {
        iGravityControl = col.GetComponent<IGravityControl>();

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (iGravityControl != null && iGravityControl.IsInRange == false)
        {
            //해당 스크립트가 있고 또한 이미 범위에 추가된 것이 아니라면
            Debug.Log(col + "은 IGravityControl 있음");
            colInRange.Add(col); // 추가함 
            iGravityControl.AntiGravity();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        ColAntiGravityEnd(col);
    }

    private void ColAntiGravityEnd(Collider col)
    {
        iGravityControl = col.GetComponent<IGravityControl>();

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (iGravityControl != null)
        {
            iGravityControl.AntiGravityEnd();
            colInRange.Remove(col);
        }
    }

}
