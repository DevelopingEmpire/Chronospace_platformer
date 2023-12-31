using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour
{
    public Text ProgressIndicator;
    public Image LoadingBar;

    public float timeLimit; // 시간제한 
    public float speed; // 1초에 몇 닳는 지 ( 기본 1 )
    float currentValue; // 현재 남은 시간 

    // Start is called before the first frame update
    void Start()
    {
        currentValue = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentValue > 0)
        {
            currentValue -= speed * Time.deltaTime; 
            ProgressIndicator.text = ((int)currentValue).ToString();
        }
        else
        {
            ProgressIndicator.text = "Done";
        }

        LoadingBar.fillAmount = currentValue / timeLimit;
    }
}