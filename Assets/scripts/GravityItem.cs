using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityItem : MonoBehaviour 
{
    public enum Type { Gravity, TimeStop, WindKey }; // 중력, 시간, 태엽 
    public Type type;
    public int value;

    public GameObject meshObj; // 이름이 아이템 어쩌구로 수정
    //public GameObject effectObj; // 폭발 효과 
    public Rigidbody rb;
    public BoxCollider colliderRange; // 탬 적용 범위 콜라이더 
    public MeshRenderer meshRenderer; // 템 범위 mesh 

    // 적용 당할 오브젝트의 itemGravityControl
    IGravityControl iGravityControl;

    // colliderRange 내의 col 들 모아두는 list 
    private List<Collider> colInRange = new List<Collider>();


    private void Awake()
    {
        rb.AddForce(transform.forward*10, ForceMode.Impulse); // 앞으로 슝~
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
        meshObj.SetActive(false); // 비활성화 
        //effectObj.SetActive(true); // 효과 보여주는거
        colliderRange.enabled = true; // 콜라이더 켜기 
        meshRenderer.enabled = true;

        yield return new WaitForSeconds(3f); // 4초 대기 

        // 남은 놈들도 싹 정리해주기 
        for (int i = colInRange.Count - 1; i >= 0; i--) // for ( )
        {
            Collider col = colInRange[i];
            IGravityControl iGravityControl = col.GetComponent<IGravityControl>();
            if (iGravityControl != null)
            { 
                iGravityControl.AntiGravityEnd();
            }
            colInRange.RemoveAt(i);
        }

        //굳이 필요없겠찌?
        //colInRange.Clear();

        Destroy(transform.parent.gameObject); // 아이템 clone 삭제 

    }

    private void OnTriggerEnter(Collider col)
    {
        iGravityControl = col.GetComponent<IGravityControl>();

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (iGravityControl != null && iGravityControl.IsInRange == false)
        {
            //해당 스크립트가 있고 또한 이미 범위에 추가된 것이 아니라면
            colInRange.Add(col); // 추가함 
            iGravityControl.AntiGravity();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        ColAntiGravity_End(col);
    }

    private void ColAntiGravity_End(Collider col)
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
