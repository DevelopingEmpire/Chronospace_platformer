using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<string> stageNames = new List<string>();
    public List<bool> stageClearStatuses = new List<bool>();
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }  
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
    }
    // 저장 파일 경로
    public string path;
    void Start()
    {
        // 안드로이드나 다른 플랫폼에 배포할 때를 고려하여 persistentDataPath 사용
        path = Path.Combine(Application.persistentDataPath, "StageData.json");
        JsonLoad(); // 게임 시작 시 데이터 불러오기
    }

    public void JsonLoad()
    {
        if (!File.Exists(path))
        {
            StageManager.Instance.InitializeStageClearStatus();
            SaveJson(); // 초기 데이터를 파일에 저장
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {

                // 스테이지 클리어 상태 불러오기
                StageManager.Instance.stageClearStatus.Clear(); // 딕셔너리 초기화

                for (int i = 0; i < saveData.stageNames.Count; i++)
                {
                    // 이미 존재하는 키에 대한 처리가 필요 없으므로, Add 대신 인덱싱을 사용하여 값을 할당
                    StageManager.Instance.stageClearStatus.Add(saveData.stageNames[i], saveData.stageClearStatuses[i]);
                }
            }
        }
        SaveJson();
    }

    public void SaveJson()
    {
        SaveData saveData = new SaveData{};

        foreach (var stage in StageManager.Instance.stageClearStatus)
        {
            saveData.stageNames.Add(stage.Key);
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}