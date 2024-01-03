using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUIControl : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    public Image timerBar; 

    public Player player; 
    public PlayerTimer playerTimer;


    void Update()
    {
        if (player.isAlive == false) timerText.text = "Stop";
        else
        {
            timerText.text = ((int)playerTimer.currentValue + 1).ToString(); // 9부터 0으로 나오므로 +1 해줌 
            timerBar.fillAmount = playerTimer.currentValue / playerTimer.timeLimit;
        }

    }
}