using UnityEngine;
using UnityEngine.AI;
using System; // 이벤트 쓰기 위해 가져옴 
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using UnityEngine.Diagnostics;

public class Guards : MonoBehaviour, IGravityControl
{
    public GameObject[] players; // 이거 자주 쓰길래 일단 전역으로 빼 봄 
    public GameObject nearestPlayer;

    [Header("Basic value")]
    public Vector3 initialPosition; // 초기 위치
    public float rotationSpeed = 5f;
    public float travelDistance = 5f;
    public float patrolDelay = 2f;
    public float detectionDelay = 0.5f;
    float detectionTimer = 0; // 0.5초에 1번씩만 detection 할거임 

    [Header("Bullet")]
    public GameObject bullet; // 총알 
    public float fireRange = 5f; // 공격 사거리
    public float fireDelay = 1f;
    private float fireTimer = 0f;
    public Vector3 fireOffset;

    // Reference to the NavMeshAgent component
    public NavMeshAgent navMeshAgent;
    private Vector3 targetPosition; // nav의 목표지점 
    public bool detectState = false; // 사람 발견시 true 
    private float nextPatrolTime;
    public CharacterController _controller; // 컨트롤러

    // 중력 관련 변수들 
    bool isGravity; // 중력을 받는 상태인가? 
    bool isGroundChecker; //is Grounded 상태가 변했는지 추적  

    /// <summary>
    /// 중력 인터페이스 구현부 
    /// 

    // 중력탬 범위 내에 있는가 
    public bool IsInRange {get; set;} 

    public float Gravity {get;set;}

    public void AntiGravity() // 중력 반전 함수 
    {
        IsInRange = true;
        isGravity = true;

        // nav 비활 
        navMeshAgent.enabled = false;
        Gravity = 9.81f;

        Debug.Log("AntiGravity On.");
    }
    public void AntiGravityEnd()
    {
        IsInRange = false;
        Gravity = -9.81f; // 반전 해제 
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
        _controller.detectCollisions = false;
    }

    // Update is called once per frame
    void Update()
    {
        detectionTimer += Time.deltaTime;
        if(detectionTimer >= detectionDelay)
        {
            // 탐지 실행
            DetectPlayer();
            detectionTimer = 0;
        }

        if (isGroundChecker != _controller.isGrounded) // 변화가 생겼다면
        {
            isGroundChecker = _controller.isGrounded; // 똑같이 맞춰준다
            // 만약 공중에 뜬 거라면? 머.. 알빠없음. 그래비티 true 됐을거임
            // 만약 착지 한다면? 그래비티 false하고 nav 켜줘야함 
            if(isGroundChecker)
            {
                isGravity = false; // 중력 상태 끝 
                navMeshAgent.enabled = true; // 켜준다
            }
        }


        if (!isGravity) // 중력 받는 상태가 아니라면 
        {
            MoveTowardsTarget();
            // If the player is in sight, set the target position to the player's position
            if (detectState == true)// 범위 안이면 
            {
                // Move towards the target position using NavMeshAgent
                targetPosition = nearestPlayer.transform.position;

                
                if (PlayerInFireRange()) // 사거리 내라면
                {
                    Fire();
                }
                
                //Fire();
            }
            else // 평화로운 상태
            {
                if (Time.time >= nextPatrolTime)
                {
                    Patrol();
                    // Set the next patrol time
                    nextPatrolTime = Time.time + patrolDelay;
                }
            }
            
            
        }
        else // 중력 받는 상태라면 
        {
            ApplyGravity();
            if (detectState == true) // 범위 안이면 
            {
                StareAtPlayer();
                
                if (PlayerInFireRange())
                {
                    Fire();
                }
                
                //Fire();

            }
            
        }

    }

    

    

    // Stare at the player by rotating the opponent's direction
    
    void StareAtPlayer()
    {
            // Rotate towards the nearest player
            Vector3 directionToPlayer = (nearestPlayer.transform.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
    
    
    

    // Patrol by setting a new random target within the travel distance
    void Patrol()
    {
        //범위내에서 랜덤하게 patrol 
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * travelDistance;
        targetPosition = initialPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }

    // 탐지 범위 내, 가장 가까운 플레이어를 탐색해 냄 ( 추적 대상 nearestPlayer) 
    private void DetectPlayer()
    {
        //주변 col들 추출해서 배열에 저장
        Collider[] hitColls = Physics.OverlapSphere(transform.position, 10f); // 시작 지점, 반지름, 레이어 

        detectState = false;
        nearestPlayer = null; // 
        foreach (Collider col in hitColls)
        {
            //if (col == _controller) Debug.Log("같다 ");
            CharacterController characterController = col as CharacterController;
            if (characterController != null && col.gameObject.CompareTag("Player")) // 캐릭터 콜라이더만 인식 
            {
                //Debug.Log(col.gameObject.name);
                //Debug.Log(col.gameObject);
                //Debug.Log(Time.realtimeSinceStartup); 
                detectState = true;

                if (nearestPlayer == null) // 아직 nearest가 없다면 
                {
                    nearestPlayer = characterController.gameObject;
                }
                else
                {
                    nearestPlayer = Vector3.Distance(transform.position, nearestPlayer.transform.position) < Vector3.Distance(transform.position, characterController.gameObject.transform.position) ? nearestPlayer : characterController.gameObject;
                }

            }
        }

    }

    // Move the opponent towards the target position using NavMeshAgent
    void MoveTowardsTarget()
    {
        
        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(targetPosition);
        
    }
    
    bool PlayerInFireRange()
    {
        // 쿨타임 돌았고 사거리 안이면 
        if (fireTimer >= fireDelay && Vector3.Distance(transform.position, nearestPlayer.transform.position) < fireRange)
        {
            return true;
        }

        fireTimer += Time.deltaTime;
        return false;
    }
    

    //Fire projectile into player
    void Fire()
    {
        // Check if enough time has passed to fire a bullet
        GameObject projectileIns = Instantiate(bullet);
        projectileIns.transform.position = transform.position + fireOffset;
        // Reset the timer for the next bullet
        fireTimer = 0f; // 초기화 
    }
}