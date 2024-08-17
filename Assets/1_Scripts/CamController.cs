using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{
    public GameObject firstCam; // 1인칭 캠. 사실 뭐 바꿔줄 건 없어서 안불러도 될듯
    public GameObject thirdCam; // 얜 껏다 켰다 해줄거임 
    private bool camToggle = false; // false가 1인칭, true가 3인칭 

    [SerializeField] 
    private CinemachineImpulseSource _source;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SwitchCamera"))// alpha5 
        {
            ToggleCamera(); // 매개변수 없이 호출하면 토글
        }
    }

    // 카메라 전환 메서드
    public void ToggleCamera(int viewMode = -1)
    {
        if (viewMode == 1)
        {
            camToggle = false; // 1인칭
        }
        else if (viewMode == 3)
        {
            camToggle = true; // 3인칭
        }
        else
        {
            camToggle = !camToggle; // 토글
        }

        thirdCam.SetActive(camToggle); // false면 꺼지고 true면 켜짐
    }

    // 피격시 카메라 흔들림
    public void ShakeCam()
    {
        // 피격시 흔들림 생성 
        _source.GenerateImpulse();
    }
}
