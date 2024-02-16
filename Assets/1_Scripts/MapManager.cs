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


    public void OnButtonActive(int buttonID)
    {
        buttons[buttonID] = true;
    }

    public void OnButtonInactive(int buttonID)
    {
        buttons[buttonID] = false;
    }
}
