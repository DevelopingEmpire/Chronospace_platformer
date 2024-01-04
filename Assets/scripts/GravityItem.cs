using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityItem : MonoBehaviour 
{
    public enum Type { Gravity, TimeStop, WindKey }; // �߷�, �ð�, �¿� 
    public Type type;
    public int value;

    public GameObject meshObj;
    //public GameObject effectObj; // ���� ȿ�� 
    public Rigidbody rb;

    // ���� ���� ������Ʈ
    GameObject nearObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion()); // �ð����� ���� �ڷ�ƾ 
    }

    private void Update()
    {
        
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f); // 3�� ��� 

        float time = 0.0f; // ����ð� 

        // �������� �ӵ��� ��� 0���� ���� 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; 
        meshObj.SetActive(false); // ��Ȱ��ȭ 
        //effectObj.SetActive(true); // ȿ�� �����ִ°�

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
            15, Vector3.up, 0f, LayerMask.GetMask("Default"));

        while(time < 3.0f) // 3�ʰ� ���� 
        {
            time += Time.deltaTime;

            foreach (RaycastHit hitObj in rayHits)
            {
                //Debug.Log(hitObj);
                if (hitObj.rigidbody == null) continue; // ������ ������ �Ѿ 
                hitObj.rigidbody.AddForce(Vector3.up * 10f); //���� 10 �� ���� (�߷¹��� )
            }
            yield return null;
        }
        
        
    }
}
