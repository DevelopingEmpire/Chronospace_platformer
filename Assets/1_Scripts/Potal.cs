using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Potal : StageMechanicsController
{
    public bool isActivated; // 포탈 켜졌는지 아닌지 

    public override int Idx { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Start()
    {
        Exit(); // 초기화.. 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isActivated)
        {
            StageManager.Instance.SetStageCleared(); // 스테이지 클리어 됨 
            SceneManager.LoadScene("Stage0");
        }
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
