using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Goalpoint : MonoBehaviour
{
    public static Goalpoint instance;

    // Awake 함수에서 싱글톤 인스턴스를 초기화합니다.
    void Awake()
    {
        if (instance == null)
        {
            instance = this; // 현재 인스턴스를 싱글톤 인스턴스로 설정합니다.
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 파괴되지 않도록 설정합니다.
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 이미 싱글톤 인스턴스가 존재하면, 중복 인스턴스를 파괴합니다.
        }
    }
}