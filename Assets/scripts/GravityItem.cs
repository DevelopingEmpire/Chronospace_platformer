using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityItem : MonoBehaviour 
{
    public enum Type { Gravity, TimeStop, WindKey }; // �߷�, �ð�, �¿� 
    public Type type;
    public int value;


    public GameObject meshObj;
    public Camera playerCam;
    //public GameObject effectObj; // ���� ȿ�� 
    public Rigidbody rb;
    public BoxCollider colliderRange; // �� ���� ���� �ݶ��̴� 
    public MeshRenderer meshRenderer; // �� ���� mesh 


    // ���� ���� ������Ʈ�� itemGravityControl
    IGravityControl iGravityControl; 
    //Player player;

    private void Awake()
    {
        //playerCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>(); //���� �������� ������ ����
        //Vector3 dir = playerCam.transform.localRotation * Vector3.forward; // ���� ����
        //dir *= 20;// ������ ��~
        //rb.velocity = transform.forward * 10; // ������ ��~
        //rb.AddForce(dir, ForceMode.Impulse);
        rb.AddForce(transform.forward * 10, ForceMode.Impulse); // ������ ��~
        StartCoroutine(Explosion());
    }

    // �ð����� ���� �ڷ�ƾ����
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1f); // 1�� ��� 

        // �������� �ӵ��� ��� 0���� ���� 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false; // �������� �߷� ���� 
        meshObj.SetActive(false); // ��Ȱ��ȭ 
        //effectObj.SetActive(true); // ȿ�� �����ִ°�
        colliderRange.enabled = true; // �ݶ��̴� �ѱ� 
        meshRenderer.enabled = true; 

        // �Ʒ� �ð� ���Ƿ� ����. ���� 
        Destroy(transform.parent.gameObject, 4f); // 4�ʵ� ������ clone ���� 

    }

    private void OnTriggerStay(Collider col)
    {
        iGravityControl = col.GetComponent<IGravityControl>();

        // ������Ʈ �ȴ޸� ���� null ��ȯ�ϴµ�, �´� �����ϸ� ������{
        if (iGravityControl != null)
        {
            iGravityControl.AntiGravity();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        iGravityControl = col.GetComponent<IGravityControl>();

        // ������Ʈ �ȴ޸� ���� null ��ȯ�ϴµ�, �´� �����ϸ� ������{
        if (iGravityControl != null)
        {
            iGravityControl.AntiGravity_End();
        }
    }


}
