using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static StageManager Instance { get; private set; }
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

        // Initialize other components or variables if needed
        stageClearStatus = new Dictionary<string, bool>();

    }

    #endregion

    [SerializeField]
    public Dictionary<string, bool> stageClearStatus;
    public string currentStageName; // 이 변수에 꼭 현재 진행중인 스테이지를 넣어줘야한다. 
    public int idx = 0; // 인덱스 끝 번을 넣어준다. 해당 인덱스가 해결되면 스테이지 클리어 
    public Vector3 spawnCharacterOffset; // 인덱스 끝 번을 넣어준다. 해당 인덱스가 해결되면 스테이지 클리어 
    public GameObject npcDialogueUI;

    public bool isPause = true; // 일시정지 상태를 나타낸다 
    public float timeScale; // 타임 스케일 임시저장할 변수 


    void Start()
    {
        npcDialogueUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // esc 눌림! 
        {

            isPause = !isPause;

            
            if (isPause) // 일시 정지된 상황이면 
            {
                //timeScale = Time.timeScale; // 현재 timeScale 을 임시 저장 
                Time.timeScale = 0f; // 일시정지 
            }
            else // 일시정지가 해제된 상황이면 
            {
                //Time.timeScale = timeScale; // 값 복원 ( 1이 아닐수도 있으므로) 
                Time.timeScale = 1;
            }
            

            UIManager.instance.OnClickEscButton(isPause);

        }

    }

    public void UnPause()
    {
        //Time.timeScale = timeScale; // 값 복원 ( 1이 아닐수도 있으므로) 
        Time.timeScale = 1;
    }


    public void InitializeStageClearStatus()
    {
        stageClearStatus = new Dictionary<string, bool>();

        for (int i = 0; i <= 10; i++)
        {
            stageClearStatus.Add("Stage" + i, false);
        }
    }

    public bool CanEnterStage(string stageName)
    {
        // 첫 스테이지는 그냥 들어가게 해야하고 그 뒤 스테이지는 체크가 필요하다. 
        
        int stageNumber = int.Parse(stageName.Replace("Stage", ""));
        // 일반 스테이지 1은 항상 진입 가능
        if (stageNumber == 1 || stageNumber == 0)
        {
            return true;
        }
        // 그 외의 일반 스테이지는 이전 스테이지 클리어 여부 확인
        else if (stageNumber > 1)
        {
            return IsStageCleared("Stage" + (stageNumber - 1));
        }
        
        return false; // 기본적으로는 진입 불가능
    }

    // 게임 클리어 하면 이 함수를 실행.
    public void SetStageCleared()
    {
        stageClearStatus[currentStageName] = true;
        DataManager.Instance.SaveJson();
    }

    public bool IsStageCleared(string stageName)
    {
        return stageClearStatus.TryGetValue(stageName, out bool cleared) && cleared;
    }

    // stage 첫 시작시 init()
    public void StageInit(string sceneName)
    {
        // 현재 씬 이름 변경 
        currentStageName = sceneName;
        GameObject respawnPoint = GameObject.FindGameObjectWithTag("StartPosition");
        //Player.Instance.SetCheckpoint(new Vector3(0,1.5f,0)); // 처음 시작 위치를 리스폰 위치로 설정 - 일단 하드코딩 ㅋㅋ 
        Player.Instance.SetCheckpoint(respawnPoint.transform.position + spawnCharacterOffset);

        // 캐릭터 스폰 
        Player.Instance.PlayerInit();

        // time Scale 은 정상으로 되돌려둠 
        UnPause();
    }


}