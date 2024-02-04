using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public float timeLimit; // 시간제한 
    public float speed; // 1초에 몇 닳는 지 ( 기본 1 )
    public float currentTime; // 현재 남은 시간 
    public Player player; // player 스크립트 

    // Start is called before the first frame update
    void Start()
    {
        currentTime = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= speed * Time.deltaTime;
        }
        else
        {
            player.isAlive = false;
            // 멈춘다 
        }
    }
}
