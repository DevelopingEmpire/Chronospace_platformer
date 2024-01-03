using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public float timeLimit; // �ð����� 
    public float speed; // 1�ʿ� �� ��� �� ( �⺻ 1 )
    public float currentValue; // ���� ���� �ð� 
    public Player player; // player ��ũ��Ʈ 

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
        }
        else
        {
            player.isAlive = false;
            // ����� 
        }
    }
}
