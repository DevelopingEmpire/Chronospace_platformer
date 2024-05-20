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

    GameObject playerObject;

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
        asyncLoad.allowSceneActivation = true; // 씬 활성화 허용 여부 초기화

        Debug.Log("비동기 씬 로딩 시작 ");

        // 씬 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            Debug.Log("로딩 진행 중: " + asyncLoad.progress);

            // progress가 0.9에 도달했을 때 씬 활성화 허용
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log("로딩 0.9 도달, 씬 활성화 준비 완료");
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
        Debug.Log("비동기 씬 로딩 됨  ");

        // PlayerTimer 제어 로직 추가
        PlayerTimer playerTimer = FindObjectOfType<PlayerTimer>();
        playerObject = GameObject.FindWithTag("Player");

        // 로비로 가는 경우
        if (sceneName == "Stage0")
        {
            if (playerTimer != null)
            {
                playerTimer.enabled = false; // PlayerTimer 비활성화
            }

            // UIManager.instance.OnClickBattleButton();
            // UIManager.instance.pickUpScreen.SetActive(true);
            // UIManager.instance.selectedStageName = preStageName;

            // 브금 틀기 
            // AudioManager.instance.PlayBgm(AudioManager.BGM.BGM_Lobby);
        }
        else
        {
            // 게임 맵 들어가는 경우 
            if (playerTimer != null)
            {
                playerTimer.enabled = true; // PlayerTimer 활성화
            }
        }

        StageManager.Instance.currentStageName = sceneName; // 바뀐 씬을 현재 씬으로 변경해줌 
        Debug.Log("바뀐 씬 이름으로 바꿔줌 ");

        // 전환 후 해당 씬의 시작 위치로 이동
        StartCoroutine(MovePlayerToStartPosition());
    }

    private IEnumerator MovePlayerToStartPosition()
    {
        yield return new WaitForSeconds(1); // 씬이 완전히 로드되도록 1초 대기

        GameObject startPosition = GameObject.FindWithTag("StartPosition");
        if (startPosition != null && playerObject != null)
        {
            playerObject.transform.position = startPosition.transform.position;
            playerObject.transform.rotation = startPosition.transform.rotation;
            Debug.Log("캐릭터를 시작 위치로 이동시켰습니다.");
        }
        else
        {
            Debug.LogError("StartPosition 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TestTitle"); // 타이틀 씬 불러오기 
    }
}
