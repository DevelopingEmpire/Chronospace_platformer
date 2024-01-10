using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGravityControl : MonoBehaviour, IGravityControl
{
    // �������̽� ���� 
    float gravity = -9.81f;
    public CharacterController controller; // ��Ʈ�ѷ�

    public void AntiGravity() // �߷� ���� �Լ� 
    {
            gravity = 9.81f;
            Debug.Log("AntiGravity On.");
   
    }
    public void AntiGravityEnd()
    {
            gravity = -9.81f; // ���� ���� 

            Debug.Log("AntiGravity Off.");
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

}
