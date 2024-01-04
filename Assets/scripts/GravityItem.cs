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

    // 적용 당할 오브젝트
    GameObject nearObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion()); // 시간차를 위해 코루틴 
    }

    private void Update()
    {
        
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f); // 3초 대기 

        float time = 0.0f; // 진행시간 

        // 물리적인 속도들 모두 0으로 해줌 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; 
        meshObj.SetActive(false); // 비활성화 
        //effectObj.SetActive(true); // 효과 보여주는거

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
            15, Vector3.up, 0f, LayerMask.GetMask("Default"));

        while(time < 3.0f) // 3초간 동작 
        {
            time += Time.deltaTime;

            foreach (RaycastHit hitObj in rayHits)
            {
                //Debug.Log(hitObj);
                if (hitObj.rigidbody == null) continue; // 리지드 없으면 넘어가 
                hitObj.rigidbody.AddForce(Vector3.up * 10f); //위로 10 힘 받음 (중력반전 )
            }
            yield return null;
        }
        
        
    }
}
