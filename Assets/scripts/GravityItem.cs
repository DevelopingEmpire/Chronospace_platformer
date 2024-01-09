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

    // ���� ���� ������Ʈ�� itemVar
    ItemGravityControl itemGravityControl; 
    Player player;

    void Start()
    {
        //3�� �� ���� 
        Invoke("Explosion", 3f);
        
    }

    //IEnumerator Explosion()
    void Explosion()
    {
        // �������� �ӵ��� ��� 0���� ���� 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        meshObj.SetActive(false); // ��Ȱ��ȭ 
        //effectObj.SetActive(true); // ȿ�� �����ִ°�

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
                // ������Ʈ �ȴ޸� ���� null ��ȯ�ϴµ�, �´� �����ϸ� ������{
                if (itemGravityControl != null)
                {
                    itemGravityControl.AntiGravity();
                }
            }


        }

        // �Ʒ� �ð� ���Ƿ� ����. ���� 
        Destroy(transform.parent.gameObject, 1f); // 1�ʵ� ������ clone ���� 

    }
}
