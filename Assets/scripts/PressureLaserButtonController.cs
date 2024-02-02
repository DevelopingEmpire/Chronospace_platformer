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

    public Material buttonMat; // 버튼 부분 머티리얼 

    public void Start()
    {
        buttonMat = mesh.GetComponent<MeshRenderer>().material;
        //buttonMat.EnableKeyword("_EMISSION");
        buttonMat.SetColor("_EmissionColor", Color.black); // emission 색 검정이면 빛 안남 
    }
    public void OnButtonPressed()
    {
        // 밟혔다면~ 
        // mesh 움직이는 모습 보여줘! 
        mesh.transform.DOLocalMoveY(0f, 0.1f);
        isPressed = true;

        // Material 교체
        buttonMat.SetColor("_EmissionColor", new Color(24,118,191) * Mathf.LinearToGammaSpace(0.001f)); //2f == intensity 값 

        // 모종의 동작 하기. 문열거나.. 뭐.. 
        Debug.Log("밟힘!");

    }

    public void OnButtonUp()
    {
        // 눌림 해제~ 
        // mesh 움직이는 모습 보여줘! 
        mesh.transform.DOLocalMoveY(0.1f, 0.1f);

        // Material 교체
        buttonMat.SetColor("_EmissionColor", Color.black); // emission 색 검정이면 빛 안남 

        // 모종의 동작 하기. 문열거나.. 뭐.. 
        Debug.Log("나감!");
        
    }

    private void Update()
    {
        if (isPressed) {
            
        }
    }
}
