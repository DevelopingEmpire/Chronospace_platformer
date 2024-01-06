using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGravityControl : MonoBehaviour
{
    public float gravityStrength; // �߷��� ����

    public CharacterController controller; // ��Ʈ�ѷ� 

    private void Start()
    {
        gravityStrength = -9.81f;
    }

    public void AntiGravity() // �߷� ���� �Լ� 
    {
        gravityStrength = 9.81f;
        Invoke("AntiGravity_End", 3f); // 3�ʵ� ���� 
        Debug.Log("AntiGravity On.");
    }

    void AntiGravity_End()
    {
        gravityStrength = -9.81f; // ���� ���� 
        Debug.Log("AntiGravity Off.");
    }

}
