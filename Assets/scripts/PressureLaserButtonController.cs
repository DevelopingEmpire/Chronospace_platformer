using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureLaserButtonController : MonoBehaviour
{
    [SerializeField]
    //CharacterController controller;
    public GameObject mesh;// 버튼 부분 메시 
    public bool isPressed = false; // 눌렸는가? 


    public void OnButtonPressed()
    {
        // 밟혔다면~ 
        // mesh 움직이는 모습 보여줘! 
        mesh.transform.DOLocalMoveY(0f, 0.1f);
        isPressed = true;
        // 모종의 동작 하기. 문열거나.. 뭐.. 
        Debug.Log("밟힘!");

    }

    public void OnButtonUp()
    {
        // 눌림 해제~ 
        // mesh 움직이는 모습 보여줘! 
        mesh.transform.DOLocalMoveY(0.1f, 0.1f);
        // 모종의 동작 하기. 문열거나.. 뭐.. 
        Debug.Log("나감!");
        
    }
}
