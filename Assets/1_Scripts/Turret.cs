using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Basic Value")]
    public float rotSpeed = 10f;

    public float rotationSpeedPatrol = 1f;
    public GameObject detectionRangeObj;
    public GameObject turretHead;
    private bool isPlayerDetected = false; // 사람 발견시 true 
    private GameObject nearestPlayer;

    [Header("Bullet")]
    public GameObject bullet; // 총알 
    public float fireDelay = 1f;
    private float fireTimer = 0f;
    public Vector3 fireOffset;
    public Vector3 direction;


    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        if(isPlayerDetected) {

            StareAtPlayer();
            Fire();
        }
        else {
            Quaternion currentRotation = turretHead.transform.rotation;
            turretHead.transform.rotation = Quaternion.Lerp(currentRotation, transform.rotation, Time.deltaTime * rotationSpeedPatrol);
            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        }
    }

    private void DetectPlayer()
    {
        MeshCollisionDetector detector = detectionRangeObj.GetComponent<MeshCollisionDetector>();
        isPlayerDetected = false;
        nearestPlayer = null;

        if (detector != null)
        {
            // MeshCollisionDetector의 변수에 접근
            bool isDetected = detector.isPlayerDetected;
            nearestPlayer = detector.nearestPlayer;
            //List<GameObject> players = detector.playersInRange;

            // 변수를 사용하여 원하는 작업 수행
            //Debug.Log("Is Player Detected: " + isDetected);
            if (nearestPlayer != null)
            {
                AudioManager.instance.PlaySfx(AudioManager.SFX.SFX_DetectionSound);

                isPlayerDetected = true;
                //Debug.Log("Nearest Player: " + nearestPlayer.name);
            }
            if (nearestPlayer == null){
                isPlayerDetected = false;
            }
            //Debug.Log("Players in Range Count: " + players.Count);
        }
        else
        {
            Debug.LogError("MeshCollisionDetector 컴포넌트를 찾을 수 없습니다!");
        }
    }
    void StareAtPlayer()
    {
        if (nearestPlayer != null)
        {
            // 타겟 방향 계산 (y축 고정을 위해 y축 값은 무시)
            Vector3 direction = nearestPlayer.transform.position - transform.position;
            direction.y = 0; // y축 방향 무시

            // 방향이 0이 아니면 회전
            if (direction != Vector3.zero)
            {
                // 지정된 방향으로 회전하도록 회전값 계산
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // 현재 회전값에서 목표 회전값으로 부드럽게 회전
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeedPatrol);

                // 타겟 플레이어의 방향 계산
                Vector3 directionHead = nearestPlayer.transform.position - turretHead.transform.position;
                //directionHead.y = 0; // y축 고정
                
                // 현재 회전과 목표 회전 계산
                Quaternion currentRotation = turretHead.transform.rotation;

                // Lerp를 사용하여 부드럽게 회전
                turretHead.transform.rotation = Quaternion.Lerp(currentRotation, Quaternion.LookRotation(directionHead), Time.deltaTime * rotationSpeedPatrol);

                //turretHead.transform.LookAt(nearestPlayer.transform, Vector3.up);
            }
        }
    }

    Vector3 GenerateTargetDirection()
    {
        if (nearestPlayer != null) {
            // 타겟 방향 계산 (y축 고정을 위해 y축 값은 무시)
            direction = nearestPlayer.transform.position - transform.position - fireOffset;
            return direction;
        }
        else {
            return Vector3.up;
        }
    }

    void Fire()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireDelay)
        {
            AudioManager.instance.PlaySfx(AudioManager.SFX.SFX_FireSound);

            GameObject projectileIns = Instantiate(bullet);
            projectileIns.transform.position = transform.position + fireOffset;
            BulletDirectionDefined projectileInsScript = projectileIns.GetComponent<BulletDirectionDefined>();

            if (projectileInsScript != null)
            {
                // TargetScript의 SomeFunction 호출
                projectileInsScript.initialDirection = GenerateTargetDirection();
            }
            else
            {
                Debug.LogError("projectileInsScript 찾을 수 없습니다.");
            }
            fireTimer = 0f; // 쿨타임 초기화
        }
    }
}
