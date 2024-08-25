using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IGravityControl
{
    public float speed = 20f;
    public float spreadScale = 0.075f;
    public float lifetime = 5f;
    private float timeDilation = 1f;
    private Vector3 initialDirection;
    private Rigidbody rb;

    public bool IsInRange { get; set; }

    // 인터페이스 구현 
    public float Gravity { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false; // 중력 영향을 받지 않도록 설정

        SetInitialDirectionToPlayer();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        timeDilation = Time.timeScale;
    }

    void FixedUpdate()
    {
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

            initialDirection += Random.onUnitSphere * spreadScale;

            initialDirection.Normalize();

            // 총알을 타겟 방향으로 회전
            transform.rotation = Quaternion.LookRotation(initialDirection);
        }
    }

    void MoveBullet() //moves the bullet into player
    {
        rb.velocity = initialDirection * speed * timeDilation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Enemy")){
            if(other.CompareTag("Player")){
                Debug.Log("총알이 플레이어에 닿았기 때문에 사라졌습니다");
            }
            Destroy(gameObject);
        }
    }

    public void AntiGravity() // 중력 반전 함수 
    {
        IsInRange = true;
        rb.velocity = initialDirection * speed * timeDilation;
    }
    public void AntiGravityEnd()
    {
        IsInRange = false;
        rb.velocity = initialDirection * speed * timeDilation;
    }

    public void BlackHole(Vector3 fieldCenter)
    {
        Vector3 direction = fieldCenter - transform.position;
        direction = Vector3.Normalize(direction); // 방향만 구함 
        rb.velocity = speed * timeDilation * direction;
    }
}