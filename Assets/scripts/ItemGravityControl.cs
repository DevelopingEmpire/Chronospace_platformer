using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGravityControl : IGravityControl
{

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
