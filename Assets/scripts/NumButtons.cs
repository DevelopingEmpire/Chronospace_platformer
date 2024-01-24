using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // UI 관련 작업을 하기 위해 필요

public class NumButtons : MonoBehaviour
{

    public void OnClickButton()
    {
        Debug.Log("1");
        Debug.Log(gameObject.name); // 현재 게임 오브젝트의 이름을 콘솔에 출력합니다.
    }

}