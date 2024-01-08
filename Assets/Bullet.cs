using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float concentrationScale = 0.1f;
    public float lifetime = 5f;
    private float timeDilation = 1f;
    private Vector3 initialDirection;

    void Start() //instanciate 이후 실행되는 스크립트이기에 본래 계획대로 총알이 방향을 향함
    {
        SetInitialDirectionToPlayer();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        timeDilation = Time.timeScale;
        MoveBullet();
    }

    void SetInitialDirectionToPlayer() //sets the direction of the bullet into player
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 0)
        {
            GameObject nearestPlayer = players[0];
            float minDistance = Vector3.Distance(transform.position, nearestPlayer.transform.position);

            // Find the nearest player
            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPlayer = player;
                }
            }

            // Set the initial direction towards the nearest player
            initialDirection = (nearestPlayer.transform.position - transform.position).normalized;
            initialDirection += Random.onUnitSphere * concentrationScale;
            initialDirection.Normalize();
        }
    }

    void MoveBullet() //moves the bullet into player
    {
        transform.Translate(initialDirection * speed * timeDilation * Time.deltaTime, Space.World);
    }
}
