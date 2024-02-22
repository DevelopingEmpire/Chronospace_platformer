using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // 타이틀 씬에서 쓰레기장 씬으로 이동 
    public void LoadMainScene()
    {
        SceneManager.LoadScene("UITest");
    }

    /*
    public void LoadGameScence() // stage 의 번호를 int로 받고, stage1 stage2 요런식으로 구분하면 좋을듯..!  
    {

    }
    */
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TestTitle"); // 타이틀 씬 불러오기 
    }
}
