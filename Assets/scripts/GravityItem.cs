using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityItem : MonoBehaviour 
{
    public enum Type { Gravity, TimeStop, WindKey }; // 중력, 시간, 태엽 
    public Type type;
    public int value;


    public GameObject meshObj;
    public Camera playerCam;
    //public GameObject effectObj; // 폭발 효과 
    public Rigidbody rb;
    public Collider colliderRange; // 탬 적용 범위 콜라이더 


    // 적용 당할 오브젝트의 itemGravityControl
    ItemGravityControl itemGravityControl; 
    //Player player;

    private void Awake()
    {
        playerCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>(); //보는 방향으로 던지기 위해
        Vector3 dir = playerCam.transform.localRotation * Vector3.forward; // 보는 방향
        dir *= 20;// 앞으로 슝~
        //rb.velocity = transform.forward * 10; // 앞으로 슝~
        rb.AddForce(dir, ForceMode.Impulse);
        StartCoroutine(Explosion());
    }

    // 시간차를 위해 코루틴으로
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1f); // 1초 대기 

        // 물리적인 속도들 모두 0으로 해줌 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false; // 리지드의 중력 끄기 
        meshObj.SetActive(false); // 비활성화 
        //effectObj.SetActive(true); // 효과 보여주는거
        colliderRange.enabled = true; // 콜라이더 켜기 

        // 아래 시간 임의로 넣음. 유의 
        Destroy(transform.parent.gameObject, 4f); // 4초뒤 아이템 clone 삭제 

    }

    private void OnTriggerStay(Collider col)
    {
        // 플레이어면 
        if (col.gameObject.transform.tag == "Player")
        {
            col.GetComponent<Player>().AntiGravity();

        }

        //아니면
        itemGravityControl = col.GetComponent<ItemGravityControl>();

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (itemGravityControl != null)
        {
            itemGravityControl.AntiGravity();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        // 플레이어면 
        if (col.gameObject.transform.tag == "Player")
        {
            col.GetComponent<Player>().AntiGravity_End();

        }

        //아니면
        itemGravityControl = col.GetComponent<ItemGravityControl>();

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (itemGravityControl != null)
        {
            itemGravityControl.AntiGravity_End();
        }
    }


}
