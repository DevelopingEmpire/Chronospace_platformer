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

    void Start() //instanciate ���� ����Ǵ� ��ũ��Ʈ�̱⿡ ���� ��ȹ��� �Ѿ��� ������ ����
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        initialDirection = (player.transform.position - transform.position).normalized;
        initialDirection += Random.onUnitSphere * concentrationScale;
        initialDirection.Normalize();
    }

    void MoveBullet() //moves the bullet into player
    {
        transform.Translate(initialDirection * speed * timeDilation * Time.deltaTime, Space.World);
    }
}
