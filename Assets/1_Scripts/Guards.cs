using UnityEngine;
using UnityEngine.AI;
using System; // 이벤트 쓰기 위해 가져옴 
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem.XR;
using System.Collections.Generic;

public class Guards : MonoBehaviour, IGravityControl
{
    public GameObject nearestPlayer;

    [Header("Basic value")]
    public Vector3 initialPosition; // 초기 위치
    public float rotationSpeedPatrol = 5f;
    public float rotationSpeedFire = 1f;
    public float travelDistance = 5f;
    public float chaseRange = 5f; //플레이어 추격 거리
    public float chaseRangeErratum = 1f; //플레이어 추격 거리의 이동시 오차
    public float patrolDelay = 2f; //순찰 텀
    public GameObject detectionRangeObj;
    public float detectionInterval = 0.5f; //순찰 때 플레이어를 찾는 판단 시간
    float detectionTimer = 0; // 0.5초에 1번씩만 detection 할거임 

    [Header("Animation")]
    public Animator anim;

    [Header("Bullet")]
    public GameObject bullet; // 총알 
    public float fireRange = 8f;
    public float fireDelay = 1f;
    private float fireTimer = 0f;
    public Vector3 fireOffset;
    public int bulletAmount = 20;
    private int bulletAmountCurrent;
    public float bulletReloadTime = 2f;
    public float bulletReloadTimeCurrent;


    [Header("NavMesh")]
    // Reference to the NavMeshAgent component
    public NavMeshAgent navMeshAgent;
    private Vector3 targetPosition; // nav의 목표지점 
    public bool isPlayerDetected = false; // 사람 발견시 true 
    public bool isPlayerInAttackRange = false; // 고정 사격 거리 이하 

    private float nextPatrolTime;
    public CharacterController _controller; // 컨트롤러


    [Header("Gravity")]
    // 중력 관련 변수들 
    public bool isGravity; // 중력을 받는 상태인가? 
    bool isGroundChecker; //is Grounded 상태가 변했는지 추적  

    // 중력탬 범위 내에 있는가 
    public bool IsInRange { get; set; }
    public float Gravity { get; set; }

    public void AntiGravity() // 중력 반전 함수 
    {
        IsInRange = true;
        isGravity = true;

        // nav 비활 
        navMeshAgent.enabled = false;
        Gravity *= -1; // 중력반전 

        Debug.Log("AntiGravity On.");
    }
    public void AntiGravityEnd()
    {
        IsInRange = false;
        Gravity *= -1; // 반전 해제
        navMeshAgent.enabled = true;
        Debug.Log("AntiGravity Off.");

    }

    //중력을 더하는 함수 
    void ApplyGravity()
    {
        // 수직 방향으로 중력을 적용.
        Vector3 gravityVector = new Vector3(0, Gravity, 0);

        // 경박스런 움직임. Lerp으로 퇴마  
        gravityVector = Vector3.Lerp(_controller.velocity, gravityVector, Time.deltaTime);

        // 중력 벡터를 현재 위치에 적용
        _controller.Move(gravityVector * Time.deltaTime);
    }

    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the NavMeshAgent component
        initialPosition = transform.position;
        targetPosition = initialPosition;
        nextPatrolTime = Time.time + patrolDelay;
        isGroundChecker = _controller.isGrounded;
        //_controller.detectCollisions = false;
        Gravity = -9.81f;
    }

    // Update is called once per frame
    void Update()
    {
        // detectionTimer의 주기적인 초기화
        detectionTimer += Time.deltaTime;
        if (detectionTimer >= detectionInterval)
        {
            // 탐지 실행
            DetectPlayer();
            detectionTimer = 0;
        }

        //캐릭터가 땅바닥에 있지 않는 경우를 체크함
        if (isGroundChecker != _controller.isGrounded) // 변화가 생겼다면
        {
            isGroundChecker = _controller.isGrounded; // 똑같이 맞춰준다
            // 만약 공중에 뜬 거라면? 머.. 알빠없음. 그래비티 true 됐을거임
            // 만약 착지 한다면? 그래비티 false하고 nav 켜줘야함 
            if (isGroundChecker)
            {
                isGravity = false; // 중력 영향력 상태 끝 
                navMeshAgent.enabled = true; // navMeshAgent 활성화
            }
        }

        if (!isGravity) // 중력 영향력 상태가 아니라면 
        {
            // If the player is in sight, set the target position to the player's position
            if (isPlayerDetected) // 범위 안이면 
            {

                // Move towards the target position using NavMeshAgent
                targetPosition = nearestPlayer.transform.position;
                anim.SetBool("isDetected", true); // 발견! 공격 

                if (PlayerInFireRange()) // 사거리 내 + 장전수가 남아 있다면 공격
                {
                    // 목표 회전 계산
                    StareAtPlayer();
                    //발사
                    if (PlayerHasAmmos())
                    {
                        Fire();
                    }
                }

                // 고정 사격 거리 측정 
                if (Vector3.Distance(transform.position, nearestPlayer.transform.position) > fireRange) // 고정사격 거리보다 크다면
                {
                    isPlayerInAttackRange = false;

                    MoveTowardsTarget(); // 움직이면서 사격 
                    anim.SetBool("isInAttackRange", false);
                }
                else 
                {
                    // 고정사격 거리보다 작다면 멈춰서 사격 
                    navMeshAgent.ResetPath();
                    isPlayerInAttackRange = true;
                    anim.SetBool("isDetected", true);
                    anim.SetBool("isInAttackRange", true);
                }

            }
            else // 평화로운 상태. idle
            {
                if (Time.time >= nextPatrolTime)
                {
                    Patrol();
                    // Set the next patrol time
                    nextPatrolTime = Time.time + patrolDelay;
                    MoveTowardsTarget();
                    anim.SetBool("isDetected", false);
                    anim.SetBool("isInAttackRange", false);

                }
            }
        }
        else // 중력 받는 상태라면 
        {
            ApplyGravity();
            if (isPlayerDetected) // 범위 안이면 
            {
                if (PlayerInFireRange()) // 사거리 내 + 장전수가 남아 있다면
                {
                    // 목표 회전 계산
                    StareAtPlayer();
                    //발사
                    if (PlayerHasAmmos())
                    {
                        Fire();
                    }
                }
            }
        }
    }

    // Stare at the player by rotating the opponent's direction
    void StareAtPlayer()
    {
        // Rotate towards the nearest player
        Vector3 directionToPlayer = (nearestPlayer.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeedPatrol * Time.deltaTime);
    }

    // Patrol by setting a new random target within the travel distance
    void Patrol()
    {
        //범위내에서 랜덤하게 patrol
        anim.SetBool("isDetected", false);
        anim.SetBool("isInAttackRange", true); //isInAttackWithRunning
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * travelDistance;
        targetPosition = initialPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }

    // 탐지 범위 내, 가장 가까운 플레이어를 탐색해 냄 ( 추적 대상 nearestPlayer) 
    ///*
    private void DetectPlayer()
    {
        MeshCollisionDetector detector = detectionRangeObj.GetComponent<MeshCollisionDetector>();
        isPlayerDetected = false;
        nearestPlayer = null;

        if (detector != null)
        {
            // MeshCollisionDetector의 변수에 접근
            isPlayerDetected = detector.isPlayerDetected;
            nearestPlayer = detector.nearestPlayer;

        }
        else
        {
            Debug.LogError("MeshCollisionDetector 컴포넌트를 찾을 수 없습니다!");
        }
    }
    //*/

    // Move the opponent towards the target position using NavMeshAgent
    void MoveTowardsTarget()
    {
        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(targetPosition);
        anim.SetBool("isDetected", true);
        anim.SetBool("isInAttackRange", false); 
    }

    bool PlayerOutOfChaseRange()
    {
        // 쿨타임 돌았고 사거리 안이면 
        if (nearestPlayer != null && Vector3.Distance(transform.position, nearestPlayer.transform.position) > chaseRange + ((UnityEngine.Random.value - 0.5f) * chaseRangeErratum))
        {
            return true;
        }
        return false;
    }

    bool PlayerInFireRange()
    {
        // 사거리에 플레이어가 있는지 확인
        if (Vector3.Distance(transform.position, nearestPlayer.transform.position) < fireRange)
        {
            return true;
        }

        return false;
    }

    bool PlayerHasAmmos()
    {
        // 쿨타임 증가
        fireTimer += Time.deltaTime;

        // 장전 중인지 확인
        if (bulletReloadTimeCurrent > 0f)
        {
            // 장전 시간 감소
            bulletReloadTimeCurrent -= Time.deltaTime;
            return false;
        }

        // 쿨타임이 지났는지 확인
        if (fireTimer < fireDelay)
        {
            return false;
        }

        // 탄약이 있는지 확인
        if (bulletAmountCurrent > 0)
        {
            //bulletAmountCurrent--; // 총알 발사
            fireTimer = 0f; // 쿨타임 초기화
            return true;
        }
        else
        {
            // 탄약이 없으면 장전 시작
            bulletReloadTimeCurrent = bulletReloadTime;
            bulletAmountCurrent = bulletAmount; // 탄약을 다시 채움
            return false;
        }
    }

    //Fire projectile into player
    void Fire()
    {
        AudioManager.instance.PlaySfx(AudioManager.SFX.SFX_FireSound);

        bulletAmountCurrent--; // 총알 발사
        // Check if enough time has passed to fire a bullet
        GameObject projectileIns = Instantiate(bullet);
        projectileIns.transform.position = transform.position + fireOffset;
        // Reset the timer for the next bullet
        fireTimer = 0f; // 초기화


    }

    public void BlackHole(Vector3 fieldCenter)
    {
        navMeshAgent.enabled = false;
        isGravity = false;
        Vector3 direction = fieldCenter - transform.position;
        //direction = Vector3.Normalize(direction); // 방향만 구함 
        _controller.Move(direction * 2.0f * Time.deltaTime);
        //navMeshAgent.enabled = true;
        //transform.position = Vector3.Lerp(transform.position, fieldCenter, Time.deltaTime); // lerp 로 움직여보자! 
        //targetPosition = fieldCenter;

    }

}