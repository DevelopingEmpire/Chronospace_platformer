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
    public BoxCollider colliderRange; // �� ���� ���� �ݶ��̴� 
    public MeshRenderer meshRenderer; // �� ���� mesh 

    // ���� ���� ������Ʈ�� itemGravityControl
    IGravityControl iGravityControl;

    // colliderRange ���� col �� ��Ƶδ� list 
    private List<Collider> colInRange = new List<Collider>();


    private void Awake()
    {
        rb.AddForce(transform.forward*10, ForceMode.Impulse); // ������ ��~
        StartCoroutine(Explosion());
    }

    // �ð����� ���� �ڷ�ƾ����
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.5f); // 1�� �� ����  

        // �������� �ӵ��� ��� 0���� ���� 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false; // �������� �߷� ���� 
        meshObj.SetActive(false); // ��Ȱ��ȭ 
        //effectObj.SetActive(true); // ȿ�� �����ִ°�
        colliderRange.enabled = true; // �ݶ��̴� �ѱ� 
        meshRenderer.enabled = true;

        yield return new WaitForSeconds(3f); // 4�� ��� 

        // ���� ��鵵 �� �������ֱ� 
        for (int i = colInRange.Count - 1; i >= 0; i--)
        {
            Collider col = colInRange[i];
            IGravityControl iGravityControl = col.GetComponent<IGravityControl>();
            if (iGravityControl != null)
            {
                iGravityControl.AntiGravityEnd();
            }
            colInRange.RemoveAt(i);
        }

        //���� �ʿ������?
        //colInRange.Clear();

        Destroy(transform.parent.gameObject); // ������ clone ���� 

    }

    private void OnTriggerStay(Collider col)
    {
        iGravityControl = col.GetComponent<IGravityControl>();

        // ������Ʈ �ȴ޸� ���� null ��ȯ�ϴµ�, �´� �����ϸ� ������{
        if (iGravityControl != null)
        {
            colInRange.Add(col); // �߰��� 
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

        // ������Ʈ �ȴ޸� ���� null ��ȯ�ϴµ�, �´� �����ϸ� ������{
        if (iGravityControl != null)
        {
            iGravityControl.AntiGravityEnd();
            colInRange.Remove(col);
        }
    }

}
