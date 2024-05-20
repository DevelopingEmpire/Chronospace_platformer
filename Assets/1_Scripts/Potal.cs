using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Potal : StageMechanicsController
{
    public bool isActivated; // 포탈 켜졌는지 아닌지

    public string targetScene; // 목표 씬 이름 - 인스펙터에서 넣어줌 

    public override int Idx { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Start()
    {
        // Exit(); // 초기화.. 
        targetScene = "Stage1";
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("포탈에 닿음");
        // 플레이어가 액티브 포탈에 닿았는지 확인 
        if (!(other.CompareTag("Player") && isActivated)) {
            Debug.Log("활성화 안됨");
            return;
        }


        Debug.Log("활성화 확인");
        // 스테이지 입장 가능한 상태인지 확인 
        if (!StageManager.Instance.CanEnterStage(targetScene)) return;
        Debug.Log("스테이지 입장가능 ");

        // 스테이지 이동 가즈아 
        // StageManager.Instance.SetStageCleared(); // 현재 스테이지 클리어 됨 
        StartCoroutine(GameManager.Instance.LoadScene(targetScene)); // 다음 스테이지 로드 - 비동기 이넘이므로 스타트 코루틴 
        Debug.Log("게임매니저 - 로드씬 호출 됨 ");

        // SceneManager.LoadScene(targetScene);  // 씬 로드하기 임시 
        


    }

    public override void Trigger()
    {
        isActivated = true;
    }

    public override void Exit()
    {
        isActivated = false;
    }

    public override void SetInitialColor(Material targetColor, Material targetColorGlow)
    {

    }
}
