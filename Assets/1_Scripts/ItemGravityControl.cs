using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGravityControl : MonoBehaviour, IGravityControl
{

    public bool IsInRange { get; set; }

    // 인터페이스 구현 
    public float Gravity { get; set; }
    public CharacterController controller; // 컨트롤러

    public Vector3 gravityVector;

    public void AntiGravity() // 중력 반전 함수 
    {
        IsInRange = true;
        Gravity = 9.81f; // 반전
        ApplyGravity();
        Debug.Log(name + " has set to AntiGravity On.");
    }
    public void AntiGravityEnd()
    {
        IsInRange = false;
        Gravity = -9.81f; // 반전 해제 
        ApplyGravity();
        Debug.Log(name + " has set to AntiGravity Off.");
    }

    private void Start()
    {

    }
    private void Awake()
    {

    }
    private void Update()
    {
        if(IsInRange){
            Gravity = 9.81f;
        }
        else{
            Gravity = -9.81f;
        }
        
        ApplyGravity();
    }

    void ApplyGravity()
    {
        // 수직 방향으로 중력을 적용.
        gravityVector = new Vector3(0f, Gravity, 0f);

        // 경박스런 움직임. Lerp으로 퇴마  
        gravityVector = Vector3.Lerp(controller.velocity, gravityVector, Time.deltaTime);

        // 중력 벡터를 현재 위치에 적용
        controller.Move(gravityVector * Time.deltaTime);

    }

    public void BlackHole(Vector3 fieldCenter)
    {
        Vector3 direction = fieldCenter - transform.position;
        direction = Vector3.Normalize(direction); // 방향만 구함 
        controller.Move(direction * Time.deltaTime); // lerp 로 움직여보자! 
    }

}
