using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // 마우스 커서 이탈 방지 
    }

    // 타이틀 씬에서 쓰레기장 씬으로 이동 
    public IEnumerator LoadMainScene(string sceneName)
    {
        // 비동기적으로 씬 로딩
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);



        // 씬 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 로비로 가는 경우
        if (sceneName == "Stage0")
        {
            /*
            UIManager.instance.OnClickBattleButton();
            UIManager.instance.pickUpScreen.SetActive(true);
            UIManager.instance.selectedStageName = preStageName;
            */

            //브금 틀기 
            //AudioManager.instance.PlayBgm(AudioManager.BGM.BGM_Lobby);

        }
        else
        {
            //게임 맵 들어가는 경우 

        }

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
