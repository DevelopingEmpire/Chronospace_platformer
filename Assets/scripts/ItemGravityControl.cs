using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGravityControl : MonoBehaviour, IGravityControl
{
    // 인터페이스 구현 
    float gravityStrength = -9.81f;
    public CharacterController controller; // 컨트롤러

    public void AntiGravity() // 중력 반전 함수 
    {
        gravityStrength = 9.81f;
        Invoke("AntiGravity_End", 3f); // 3초뒤 해제 
        Debug.Log("AntiGravity On.");
    }
    public void AntiGravity_End()
    {
        gravityStrength = -9.81f; // 반전 해제 
        Debug.Log("AntiGravity Off.");
    }


    private void Update()
    {
        applyGravity();
    }

    void applyGravity()
    {
        // 수직 방향으로 중력을 적용.
        Vector3 gravityVector = new Vector3(0, gravityStrength, 0);

        // 중력 벡터를 현재 위치에 적용
        controller.Move(gravityVector * Time.deltaTime);
    }

}
