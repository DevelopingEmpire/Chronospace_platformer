using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public float timeLimit; // �ð����� 
    public float speed; // 1�ʿ� �� ��� �� ( �⺻ 1 )
    public float currentTime; // ���� ���� �ð� 
    public Player player; // player ��ũ��Ʈ 

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
            // ����� 
        }
    }
}
