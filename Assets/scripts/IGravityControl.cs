using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IGravityControl
{
    // �׳� ������ �ȵ�. ������Ƽ ������
    public static float gravityStrength { get; set; } // �߷��� ����. 

    public static CharacterController controller { get; set; } // ��Ʈ�ѷ� 

    void AntiGravity(); // �߷� ���� �Լ� 

    void AntiGravity_End(); // �߷� ���� ������ �Լ� 

}
