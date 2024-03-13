using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public GameObject firstCam; // 1인칭 캠. 사실 뭐 바꿔줄 건 없어서 안불러도 될듯
    public GameObject thirdCam; // 얜 껏다 켰다 해줄거임 
    private bool camToggle = false; // false가 1인칭, true가 3인칭 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SwitchCamera"))// alpha5 
        {
            camToggle = !camToggle; // 뒤집어줌 
            thirdCam.SetActive(camToggle); // false면 꺼지고 true면 켜짐 

        }
    }
}
