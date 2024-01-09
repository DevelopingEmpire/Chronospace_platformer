using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGravityControl : MonoBehaviour, IGravityControl
{
    // 인터페이스 구현 
    float gravity = -9.81f;
    public CharacterController controller; // 컨트롤러

    int isCoolTime = -1; // 너무 빈번하게 일어나서, 약간의 쿨타임

    public void AntiGravity() // 중력 반전 함수 
    {
        if (isCoolTime == -1)
        {
            isCoolTime = 1;
            gravity = 9.81f;
            //Invoke("AntiGravity_End", 3f); // 3초뒤 해제 
            StartCoroutine(coolTimer()); // 쿨타임 0.5초 코루틴 

            Debug.Log("AntiGravity On.");
        }
        
    }
    public void AntiGravity_End()
    {
        //if (isCoolTime == -1)
        //{
            isCoolTime = 1;
            gravity = -9.81f; // 반전 해제 
            StartCoroutine(coolTimer()); // 쿨타임 0.5초 코루틴 

            Debug.Log("AntiGravity Off.");
        //}
        
    }


    private void Update()
    {
        applyGravity();
    }

    void applyGravity()
    {
        // 수직 방향으로 중력을 적용.
        Vector3 gravityVector = new Vector3(0, gravity, 0);

        // 중력 벡터를 현재 위치에 적용
        controller.Move(gravityVector * Time.deltaTime);
    }

    IEnumerator coolTimer()
    {
        yield return new WaitForSeconds(0.5f); // 0.3초 대기
        isCoolTime = -1;
    }
}
