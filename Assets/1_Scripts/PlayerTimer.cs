using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public float timeLimit; // 시간제한 
    public float speed; // 1초에 몇 닳는 지 ( 기본 1 )
    public float currentTime; // 현재 남은 시간 
    public Player player; // player 스크립트 

    public bool isPlaying = false; // play 중에만 타이머 시작 

    public GameObject timerUI; 
    public TextMeshProUGUI timerText;
    public Image timerBar;

    public void TimerUIInit()
    {
        // 시간초 리셋 
        currentTime = timeLimit;

        // UI 요소들이 null이 아닌지 확인하고, null이라면 다시 찾기
        if (UIManager.instance != null && UIManager.instance.battleHUDScreen != null)
        {
            timerUI = UIManager.instance.battleHUDScreen.transform.GetChild(0).gameObject;
            timerBar = timerUI.transform.GetChild(0).GetComponent<Image>();// 테두리 원 
            timerText = timerUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("UIManager 또는 battleHUDScreen이 null입니다.");
        }
    }

    private void Start()
    {
        TimerUIInit(); // Start에서 UI 초기화 호출
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManager.Instance.isPause) return; // 일시정지한 동안 시간초도 멈춤 

        if (isPlaying && player.isAlive) 
        {
            CountDown();
        }
        
    }

    public void TimeChange(float changeTime)
    {
        // 태엽 감으면 늘어나는거. 혹시 뺄 일도 있을까 싶어서 이름은 change로 지음 
        currentTime = timeLimit < currentTime + changeTime ? timeLimit : currentTime + changeTime; // 시간 제한보다 많아지진 않는다 
    }

    public void CountDown()
    {
        if (currentTime > 0)
        {
            currentTime -= speed * Time.deltaTime;
            if(timerText != null && timerBar != null){
                timerText.text = ((int)currentTime+1).ToString(); // 0까지 나오므로 +1 해줌 
                timerBar.fillAmount = currentTime / timeLimit;
            }
        }
        else
        {
            timerText.text = "Stop";
            player.Die();

            // 멈춘다 
        }
    }
}
