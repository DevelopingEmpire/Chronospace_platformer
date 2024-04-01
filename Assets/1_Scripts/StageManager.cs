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
    public int idx = 0;
    public GameObject npcDialogueUI;
    //private static SceneTransitionManager instance;

    //박강 작성 구간:


    //roh 작성 구간:

    void Start()
    {
        npcDialogueUI.SetActive(true);
    }
    public void InitializeStageClearStatus()
    {
        stageClearStatus = new Dictionary<string, bool>();

        for (int i = 1; i <= 10; i++)
        {
            stageClearStatus.Add("Stage" + i, false);
        }
    }

    public bool CanEnterStage(string stageName)
    {
        if (stageName.StartsWith("Stage1")) // 첫 스테이지는 그냥 들어가게 해야하고 그 뒤 스테이지는 체크가 필요하다. 
        {
            int stageNumber = int.Parse(stageName.Replace("Stage", ""));
            // 일반 스테이지 1은 항상 진입 가능
            if (stageNumber == 1)
            {
                return true;
            }
            // 그 외의 일반 스테이지는 이전 스테이지 클리어 여부 확인
            else if (stageNumber > 1)
            {
                return IsStageCleared("Stage" + (stageNumber - 1));
            }
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
}

