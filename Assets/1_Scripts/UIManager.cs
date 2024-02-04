using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject settingScreen;
    public GameObject battleHUDScreen;

    public void OnClickStartButton()
    {
        startScreen.SetActive(false);
    }
    
}
