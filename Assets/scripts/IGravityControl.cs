using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IGravityControl
{
    // �׳� ������ �ȵ�. ������Ƽ ������ (?? ã�ƺ���)
    public static float gravity { get; set; } // �߷��� ����. 

    void AntiGravity(); // �߷� ���� �Լ� 

    void AntiGravityEnd(); // �߷� ���� ������ �Լ� 

}
