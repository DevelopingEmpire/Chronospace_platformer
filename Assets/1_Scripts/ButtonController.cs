using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : StageMechanicsController 
{
    [SerializeField]
    private CharacterController controller; // 컨트롤러 담는 용 변수 
    public GameObject mesh;// 버튼 부분 메시 
    public Material buttonMat; // 버튼 부분 머티리얼 
    public StageMechanicsController triggerObject; // 작동시킬 무언가 
    public int idx; // 구분용 아이디

    public override int Idx { get; set; }

    private void Start()
    {
        Idx = idx; // 값 지정.. ?  아 이렇게 쓰지 말라셨는데...? ㅎㅎ;; 

        buttonMat = mesh.GetComponent<MeshRenderer>().material;
        buttonMat.SetColor("_EmissionColor", Color.black); // emission 색 검정이면 빛 안남 

    }

    private void OnTriggerEnter(Collider col)
    {
        controller = col.GetComponent<CharacterController>(); // 밟을 수 있는 애들은 다 캐.콘 갖고있음 

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (controller != null)
        {
            Trigger();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        controller = col.GetComponent<CharacterController>(); // 밟을 수 있는 애들은 다 캐.콘 갖고있음 

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (controller != null)
        {
            Exit();
        }
    }

    public override void Trigger()
    {
        //throw new System.NotImplementedException();
        // 밟힘
        // mesh 움직이는 모습 보여줘! 
        mesh.transform.DOLocalMoveY(0f, 0.1f);

        // Material 교체
        buttonMat.SetColor("_EmissionColor", Color.red * Mathf.LinearToGammaSpace(5f)); //2f == intensity 값 

        triggerObject.Trigger();
    }

    public override void Exit()
    {
        //throw new System.NotImplementedException();

        // 나감 
        // mesh 움직이는 모습 보여줘! 
        mesh.transform.DOLocalMoveY(0.1f, 0.1f);

        buttonMat.SetColor("_EmissionColor", Color.black); // emission 색 검정이면 빛 안남 

        triggerObject.Exit();
    }
}
