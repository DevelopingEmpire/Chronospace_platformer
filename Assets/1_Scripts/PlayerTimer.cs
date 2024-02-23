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

    public GameObject timerUI; 
    public TextMeshProUGUI timerText;
    public Image timerBar;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = timeLimit;
        timerText = timerUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        timerBar = timerUI.transform.GetChild(0).GetComponent<Image>();// 빨간 바 
    }

    // Update is called once per frame
    void Update()
    {
        if (MapManager.instance.isPause) return; // 일시정지한 동안 시간초도 멈춤 

        if (player.isAlive) 
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
            timerText.text = ((int)currentTime+1).ToString(); // 0까지 나오므로 +1 해줌 
            timerBar.fillAmount = currentTime / timeLimit;
        }
        else
        {
            timerText.text = "Stop";
            player.isAlive = false;

            // 멈춘다 
        }
    }
}
