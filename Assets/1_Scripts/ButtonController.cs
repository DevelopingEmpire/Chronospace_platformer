using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : StageMechanicsController 
{
    [SerializeField]
    CharacterController controller;
    public GameObject mesh;// 버튼 부분 메시 
    public Material buttonMat; // 버튼 부분 머티리얼 
    public int buttonID; // 구분용 아이디

    public override int Idx { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Start()
    {
        buttonMat = mesh.GetComponent<MeshRenderer>().material;
        buttonMat.SetColor("_EmissionColor", Color.black); // emission 색 검정이면 빛 안남 
    }
    private void OnTriggerEnter(Collider col)
    {
        controller = col.GetComponent<CharacterController>(); // 밟을 수 있는 애들은 다 캐.콘 갖고있음 

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (controller != null)
        {
            // 밟혔다면~ 
            // mesh 움직이는 모습 보여줘! 
            mesh.transform.DOLocalMoveY(0f, 0.1f);

            // Material 교체
            buttonMat.SetColor("_EmissionColor", Color.red * Mathf.LinearToGammaSpace(5f)); //2f == intensity 값 

            // 모종의 동작 하기. 문열거나.. 뭐.. 
            //Debug.Log("밟힘!");
            
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

            buttonMat.SetColor("_EmissionColor", Color.black); // emission 색 검정이면 빛 안남 

            // 모종의 동작 하기. 문열거나.. 뭐.. 
            //Debug.Log("나감!");
        }
    }

    public void OnButtonPressed()
    {
        // 밟혔다면~ 
        // mesh 움직이는 모습 보여줘! 
        mesh.transform.DOLocalMoveY(0f, 0.1f);

        // Material 교체
        buttonMat.SetColor("_EmissionColor", new Color(24, 118, 191) * Mathf.LinearToGammaSpace(0.001f)); //2f == intensity 값 

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
        //Debug.Log("나감!");

    }

    public override void Trigger()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}
