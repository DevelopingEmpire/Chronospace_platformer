using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGravityControl : MonoBehaviour, IGravityControl
{
    bool isInRange = false; // 중력 범위 내에 있는가 
    public bool IsInRange
    {
        get { return isInRange; }
        set { isInRange = value; }
    }

    // 인터페이스 구현 
    public float gravity = -9.81f;
    public float Gravity
    {
        get { return gravity; }
        set { gravity = value; }
    }
    public CharacterController controller; // 컨트롤러

    public void AntiGravity() // 중력 반전 함수 
    {
        IsInRange = true;
        Gravity = 9.81f;
        Debug.Log("AntiGravity On.");
   
    }
    public void AntiGravityEnd()
    {
        IsInRange = false;
        Gravity = -9.81f; // 반전 해제 

        Debug.Log("AntiGravity Off.");
    }


    private void Update()
    {
        applyGravity();
    }

    void applyGravity()
    {
        // 수직 방향으로 중력을 적용.
        Vector3 gravityVector = new Vector3(0, Gravity, 0);

        // 중력 벡터를 현재 위치에 적용
        controller.Move(gravityVector * Time.deltaTime);
    }

}
