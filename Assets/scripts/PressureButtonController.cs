using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButtonController : MonoBehaviour
{
    [SerializeField]
    CharacterController controller;
    public GameObject mesh;// 버튼 부분 메시 

    private void OnTriggerEnter(Collider col)
    {
        controller = col.GetComponent<CharacterController>(); // 밟을 수 있는 애들은 다 캐.콘 갖고있음 

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (controller != null)
        {
            // 밟혔다면~ 
            // mesh 움직이는 모습 보여줘! 
            mesh.transform.DOLocalMoveY(0f, 0.1f);

            // 모종의 동작 하기. 문열거나.. 뭐.. 
            Debug.Log("밟힘!");

        }
    }

    private void OnTriggerExit(Collider col)
    {
        controller = col.GetComponent<CharacterController>(); // 밟을 수 있는 애들은 다 캐.콘 갖고있음 

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (controller != null)
        {
            // 밟혔다면~ 
            // mesh 움직이는 모습 보여줘! 
            mesh.transform.DOLocalMoveY(0.1f, 0.1f);
            // 모종의 동작 하기. 문열거나.. 뭐.. 
            Debug.Log("나감!");
        }
    }
}
