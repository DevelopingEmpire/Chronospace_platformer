using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Set this as the instance and ensure it persists across scenes
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // 마우스 커서 이탈 방지 

        StageManager.Instance.InitializeStageClearStatus(); // 기본 정보 초기화 
    }

    // 타이틀 씬에서 쓰레기장 씬으로 이동 
    public IEnumerator LoadScene(string sceneName)
    {
        Debug.Log("게임매니저 - 로드씬 실행 됨  ");

        // 비동기적으로 씬 로딩
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        Debug.Log("비동기 씬 로딩 시작 ");


        // 씬 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // PlayerTimer 제어 로직 추가
        PlayerTimer playerTimer = FindObjectOfType<PlayerTimer>();

        // 로비로 가는 경우
        if (sceneName == "Stage0")
        {

            if (playerTimer != null)
            {
                playerTimer.enabled = false; // PlayerTimer 비활성화
            }

            //UIManager.instance.OnClickBattleButton();
            //UIManager.instance.pickUpScreen.SetActive(true);f
            //UIManager.instance.selectedStageName = preStageName;



            //브금 틀기 
            //AudioManager.instance.PlayBgm(AudioManager.BGM.BGM_Lobby);

        }
        else
        {
            //게임 맵 들어가는 경우 
            if (playerTimer != null)
            {
                playerTimer.enabled = true; // PlayerTimer 활성화
            }
        }

        StageManager.Instance.currentStageName = sceneName; // 바뀐 씬을 현재 씬으로 변경해줌 
        Debug.Log("바뀐 씬 이름으로 바꿔줌 ");
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
