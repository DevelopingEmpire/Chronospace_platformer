using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IGravityControl
{
    // �׳� ������ �ȵ�. ������Ƽ ������ (?? ã�ƺ���)
    public float Gravity { get; set; } // �߷��� ����. 
    public bool IsInRange { get; set; } // ���� ���� �ִ��� ���� 
    

    void AntiGravity(); // �߷� ���� �Լ� 

    void AntiGravityEnd(); // �߷� ���� ������ �Լ� 

}
