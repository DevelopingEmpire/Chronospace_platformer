using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGravityControl : MonoBehaviour
{
    public float gravityStrength; // 중력의 세기

    public CharacterController controller; // 컨트롤러 

    private void Start()
    {
        gravityStrength = -9.81f;
    }

    public void AntiGravity() // 중력 반전 함수 
    {
        gravityStrength = 9.81f;
        Invoke("AntiGravity_End", 3f); // 3초뒤 해제 
        Debug.Log("AntiGravity On.");
    }

    void AntiGravity_End()
    {
        gravityStrength = -9.81f; // 반전 해제 
        Debug.Log("AntiGravity Off.");
    }

}
