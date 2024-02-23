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

    public bool isPause; // 일시정지 상태를 나타낸다 
    public float timeScale; // 타임 스케일 임시저장할 변수 


    private void Start() // 일단 시작시 브금 재생으로 해뒀는데, 다른사람 보니 시작 함수를 다른데서 호출하도록 만들더라 
    {
        AudioManager.instance.PlayBgm(AudioManager.BGM.BGM_Lobby); // bgm 재생 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // esc 눌림! 
        {
            isPause = !isPause;
            if (isPause) // 일시 정지된 상황이면 
            {
                timeScale = Time.timeScale; // 현재 timeScale 을 임시 저장 
                Time.timeScale = 0f; // 일시정지 
            }
            else // 일시정지가 해제된 상황이면 
            {
                Time.timeScale = timeScale; // 값 복원 ( 1이 아닐수도 있으므로) 
            }
            UIManager.instance.OnClickEscButton(isPause); 

        }

    }

    public void GameClear()
    {
        AudioManager.instance.StopBgm(); // bgm 중지 
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
