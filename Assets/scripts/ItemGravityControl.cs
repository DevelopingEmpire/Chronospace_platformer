using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGravityControl : MonoBehaviour, IGravityControl
{
    bool isInRange = false; // �߷� ���� ���� �ִ°� 
    public bool IsInRange
    {
        get { return isInRange; }
        set { isInRange = value; }
    }

    // �������̽� ���� 
    public float gravity = -9.81f;
    public float Gravity
    {
        get { return gravity; }
        set { gravity = value; }
    }
    public CharacterController controller; // ��Ʈ�ѷ�

    public void AntiGravity() // �߷� ���� �Լ� 
    {
        IsInRange = true;
        Gravity = 9.81f;
        Debug.Log("AntiGravity On.");
   
    }
    public void AntiGravityEnd()
    {
        IsInRange = false;
        Gravity = -9.81f; // ���� ���� 

        Debug.Log("AntiGravity Off.");
    }


    private void Update()
    {
        applyGravity();
    }

    void applyGravity()
    {
        // ���� �������� �߷��� ����.
        Vector3 gravityVector = new Vector3(0, Gravity, 0);

        // �߷� ���͸� ���� ��ġ�� ����
        controller.Move(gravityVector * Time.deltaTime);
    }

}
