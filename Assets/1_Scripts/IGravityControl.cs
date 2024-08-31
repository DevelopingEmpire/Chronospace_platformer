using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IGravityControl
{
    public float Gravity { get; set; } // 중력의 세기. 
    public bool IsInRange { get; set; } // 범위 내에 있는지 여부 

    void AntiGravity(); // 중력 반전 함수 

    void AntiGravityEnd(); // 중력 반전 끝나는 함수 

    
    void BlackHole(Vector3 fieldCenter);//중력장 중심으로 끌려가는 함수
}
