using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public float timeLimit; // 시간제한 
    public float checkPointTimeLimit; // 체크포인트 찍은 후 남은 시간제한 
    public float paneltyTimeDecrease = 20f;

    public float speed; // 1초에 몇 닳는 지 ( 기본 1 )
    public float currentTime; // 현재 남은 시간 
    public Player player; // player 스크립트 

    public bool isPlaying = false; // play 중에만 타이머 시작

    public GameObject timerUI; 
    public TextMeshProUGUI timerText;
    public Image timerBar;

    [Header("warning")]
    private float warningTime = 10f; // 경고 사운드를 시작할 기준 시간
    private float soundTickInterval = 1f; // 1초마다 경고음
    private float nextTickTime = 0f; // 다음 경고음 재생 시간

    public void TimerUIInit()
    {
        currentTime = checkPointTimeLimit;

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
        // 시간초 리셋 
        checkPointTimeLimit = timeLimit; 

        TimerUIInit(); // Start에서 UI 초기화 호출
    }

    // Update is called once per frame
    void Update()
    {

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

    public void SetCheckPointTime()
    {
        Debug.Log("set check point time");

        checkPointTimeLimit = currentTime;

    }

    public void CountDown()
    {
        if (currentTime > 0)
        {
            currentTime -= speed * Time.deltaTime;
            timerText.text = ((int)currentTime+1).ToString(); // 0까지 나오므로 +1 해줌 
            timerBar.fillAmount = currentTime / timeLimit;

            // 남은 시간이 경고 시간 이하일 때 1초마다 경고음 재생
            if (currentTime <= warningTime && currentTime > 0)
            {
                if (Time.time >= nextTickTime)
                {
                    AudioManager.instance.PlaySfx(AudioManager.SFX.SFX_TimerTickSound); // 경고음 재생
                    nextTickTime = Time.time + soundTickInterval; // 다음 경고음 재생 시간 설정
                }
            }
        }
        else
        {
            timerText.text = "Stop";
            player.Die();

            // 멈춘다 
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet")){
            currentTime -= paneltyTimeDecrease;
        }
    }
}
