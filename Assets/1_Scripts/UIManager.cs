using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static UIManager instance;  // Singleton instance

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

    public GameObject startScreen;
    public GameObject settingScreen;
    public GameObject battleHUDScreen;

    public bool isPause; // 일시정지 상태를 나타낸다 

    private void Update()
    {
        // 뒤로가기 키를 누르면 일시정지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            settingScreen.SetActive(isPause);
        }
    }

    public void OnClickStartButton()
    {
        startScreen.SetActive(false);
    }

    
}
