using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static MapManager instance;  // Singleton instance
    void Awake() // SingleTon
    {
        // 이미 인스턴스가 존재하면서 이게 아니면 파괴 반환
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object and make sure it persists between scene loads
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public bool[] buttons = new bool[3]; // 기본값이 원래 false라네요? 개꿀 

    private void Start() // 일단 시작시 브금 재생으로 해뒀는데, 다른사람 보니 시작 함수를 다른데서 호출하도록 만들더라 
    {
        AudioManager.instance.playBgm(true); // bgm 재생 
    }

    public void GameClear()
    {
        AudioManager.instance.playBgm(false); // bgm 중지 
    }

    public void OnButtonActive(int buttonID)
    {
        buttons[buttonID] = true;
    }

    public void OnButtonInactive(int buttonID)
    {
        buttons[buttonID] = false;
    }
}
