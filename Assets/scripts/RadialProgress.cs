using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour
{
    public Text ProgressIndicator;
    public Image LoadingBar;
    
    public GameObject Player;// 플레이어의 오브젝트 가져옴
    PlayerTimer playerTimer; // 거기 붙은 스크립트 (컴포넌트) 

    void Awake()
    {
        playerTimer = Player.GetComponent<PlayerTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTimer.isAlive == false)
        {
            ProgressIndicator.text = "Stop";
        }
        else
        {
            ProgressIndicator.text = ((int)playerTimer.currentValue+1).ToString(); // 9부터 0으로 나오므로 +1 해줌 
            LoadingBar.fillAmount = playerTimer.currentValue / playerTimer.timeLimit;
        }
        
    }
}