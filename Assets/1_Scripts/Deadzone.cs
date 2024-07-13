using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    //트리거와 충돌 시 -  데드존, 아이템 사용 
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어인지 확인
        if ((other.CompareTag("Player")))
        {
            other.gameObject.GetComponent<Player>().Die(); // 닿은 플레이어 죽음 ㄱㄱ 
        }

        
    }
}