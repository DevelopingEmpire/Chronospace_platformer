using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGravityControl : MonoBehaviour, IGravityControl
{
    // �������̽� ���� 
    float gravity = -9.81f;
    public CharacterController controller; // ��Ʈ�ѷ�

    int isCoolTime = -1; // �ʹ� ����ϰ� �Ͼ��, �ణ�� ��Ÿ��

    public void AntiGravity() // �߷� ���� �Լ� 
    {
        if (isCoolTime == -1)
        {
            isCoolTime = 1;
            gravity = 9.81f;
            //Invoke("AntiGravity_End", 3f); // 3�ʵ� ���� 
            StartCoroutine(coolTimer()); // ��Ÿ�� 0.5�� �ڷ�ƾ 

            Debug.Log("AntiGravity On.");
        }
        
    }
    public void AntiGravity_End()
    {
        //if (isCoolTime == -1)
        //{
            isCoolTime = 1;
            gravity = -9.81f; // ���� ���� 
            StartCoroutine(coolTimer()); // ��Ÿ�� 0.5�� �ڷ�ƾ 

            Debug.Log("AntiGravity Off.");
        //}
        
    }


    private void Update()
    {
        applyGravity();
    }

    void applyGravity()
    {
        // ���� �������� �߷��� ����.
        Vector3 gravityVector = new Vector3(0, gravity, 0);

        // �߷� ���͸� ���� ��ġ�� ����
        controller.Move(gravityVector * Time.deltaTime);
    }

    IEnumerator coolTimer()
    {
        yield return new WaitForSeconds(0.5f); // 0.3�� ���
        isCoolTime = -1;
    }
}
