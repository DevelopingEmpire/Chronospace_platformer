using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityItem : MonoBehaviour 
{
    public enum Type { Gravity, TimeStop, WindKey }; // 중력, 시간, 태엽 
    public Type type;
    public int value;


    public GameObject meshObj;
    //public GameObject effectObj; // 폭발 효과 
    public Rigidbody rb;

    // 적용 당할 오브젝트의 itemVar
    ItemGravityControl itemGravityControl; 
    Player player;

    void Start()
    {
        //3초 후 폭발 
        Invoke("Explosion", 3f);
        
    }

    //IEnumerator Explosion()
    void Explosion()
    {
        // 물리적인 속도들 모두 0으로 해줌 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        meshObj.SetActive(false); // 비활성화 
        //effectObj.SetActive(true); // 효과 보여주는거

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
            1, Vector3.up, 0f, LayerMask.GetMask("Default"));

        foreach (RaycastHit hitObj in rayHits)
        {

            if (hitObj.collider.gameObject.tag == "Player")
            {
                hitObj.collider.GetComponent<Player>().AntiGravity();

            }
            else
            {
                itemGravityControl = hitObj.collider.GetComponent<ItemGravityControl>();
                // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
                if (itemGravityControl != null)
                {
                    itemGravityControl.AntiGravity();
                }
            }


        }

        // 아래 시간 임의로 넣음. 유의 
        Destroy(transform.parent.gameObject, 1f); // 1초뒤 아이템 clone 삭제 

    }
}
