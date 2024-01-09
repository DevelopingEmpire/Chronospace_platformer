using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGravityControl : MonoBehaviour, IGravityControl
{
    // �������̽� ���� 
    float gravityStrength = -9.81f;
    public CharacterController controller; // ��Ʈ�ѷ�

    public void AntiGravity() // �߷� ���� �Լ� 
    {
        gravityStrength = 9.81f;
        Invoke("AntiGravity_End", 3f); // 3�ʵ� ���� 
        Debug.Log("AntiGravity On.");
    }
    public void AntiGravity_End()
    {
        gravityStrength = -9.81f; // ���� ���� 
        Debug.Log("AntiGravity Off.");
    }


    private void Update()
    {
        applyGravity();
    }

    void applyGravity()
    {
        // ���� �������� �߷��� ����.
        Vector3 gravityVector = new Vector3(0, gravityStrength, 0);

        // �߷� ���͸� ���� ��ġ�� ����
        controller.Move(gravityVector * Time.deltaTime);
    }

}
