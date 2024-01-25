using UnityEngine;
using UnityEngine.AI;
using System; // 이벤트 쓰기 위해 가져옴 
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Guards : MonoBehaviour, IGravityControl
{
    // The tag of the player object
    public string playerTag = "Player";
    public GameObject player1;
    public Vector3 initialPosition;
    public float sightRange = 10f;
    public float addressRange = 5f;
    public float rotationSpeed = 5f;

    public float travelDistance = 5f;
    public float patrolDelay = 2f;

    public GameObject projectileObj;
    public float fireDelay = 1f;
    private float fireTimer = 0f;
    public Vector3 fireOffset;

    // Reference to the NavMeshAgent component
    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;
    private int state = 0;
    private float nextPatrolTime;
    public CharacterController controller; // 컨트롤러

    bool isGravity; // 중력을 받는 상태인가? 

    public bool isGroundChecker; //is Grounded 상태가 변했는지 추적  


    /// <summary>
    /// 중력 인터페이스 구현부 
    /// 
    bool isInRange = false; // 중력 범위 내에 있는가 
    public bool IsInRange
    {
        get { return isInRange; }
        set { isInRange = value; }
    }

    public float gravity = -9.81f;

    public float Gravity {
        get { return gravity; }
        set { gravity = value; }
    }

    public void AntiGravity() // 중력 반전 함수 
    {
        isInRange = true;
        isGravity = true;

        // nav 비활 
        navMeshAgent.enabled = false;
        Gravity = 9.81f;

        Debug.Log("AntiGravity On.");
    }
    public void AntiGravityEnd()
    {
        isInRange = false;
        Gravity = -9.81f; // 반전 해제 
        Debug.Log("AntiGravity Off.");

    }

    //중력을 더하는 함수 
    void ApplyGravity()
    {
        // 수직 방향으로 중력을 적용.
        Vector3 gravityVector = new Vector3(0, Gravity, 0);

        // 경박스런 움직임. Lerp으로 퇴마  
        gravityVector = Vector3.Lerp(controller.velocity, gravityVector, Time.deltaTime);

        // 중력 벡터를 현재 위치에 적용
        controller.Move(gravityVector * Time.deltaTime);
    }

    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
        targetPosition = initialPosition;
        nextPatrolTime = Time.time + patrolDelay;
        isGroundChecker = controller.isGrounded;
    }

    // Update is called once per frame
    void Update()
    {
        
        // 이거, 그냥 이벤트 쓰지 말고 여기서 판단하는 식으로 바꾸기.

        if (isGroundChecker != controller.isGrounded) // 변화가 생겼다면
        {
            isGroundChecker = controller.isGrounded; // 똑같이 맞춰준다
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
            // If the player is in sight, set the target position to the player's position
            if (PlayerInSight())
            {
                state = 1; // Change to stare at player state
                StareAtPlayer();
                if (PlayerInAdressRange())
                {
                    targetPosition = (GameObject.FindGameObjectWithTag(playerTag).transform.position);
                    MoveTowardsTarget();
                    Fire();
                }
                else
                {
                    Fire();
                }
            }
            else // Move towards the target position using NavMeshAgent
            {
                state = 0; // Change to patrol state
                if (Time.time >= nextPatrolTime)
                {
                    Patrol();
                    // Set the next patrol time
                    nextPatrolTime = Time.time + patrolDelay;
                    MoveTowardsTarget();
                }
            }
        }
        else // 중력 받는 상태라면 
        {
            ApplyGravity();
        }

        
    }

    // Check if the player is in sight
    bool PlayerInSight()
    {
        // Find all GameObjects with the player tag
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);

        foreach (GameObject player in players)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

            // Perform a raycast to check if there are obstacles between the opponent and the player
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, sightRange))
            {
                if (hit.collider.CompareTag(playerTag))
                {
                    // Player is in sight
                    return true;
                }
            }
        }

        // Player is not in sight
        return false;
    }

    bool PlayerInAdressRange()
    {
        // Find all GameObjects with the player tag
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);

        foreach (GameObject player in players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < addressRange)
            {
                // Player is in address range
                return true;
            }
        }

        // No players in address range
        return false;
    }

    // Stare at the player by rotating the opponent's direction
    void StareAtPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);

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

            // Rotate towards the nearest player
            Vector3 directionToPlayer = (nearestPlayer.transform.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    //Fire projectile into player
    void Fire()
    {
        // Check if enough time has passed to fire a bullet
        if (Time.time >= fireTimer)
        {
            GameObject projectileIns = Instantiate(projectileObj);
            projectileIns.transform.position = transform.position + fireOffset;
            // Reset the timer for the next bullet
            fireTimer = Time.time + fireDelay;
        }
    }

    // Patrol by setting a new random target within the travel distance
    void Patrol()
    {
        SetRandomPatrolTarget();
    }

    // Set a new random target within the travel distance
    void SetRandomPatrolTarget()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * travelDistance;
        targetPosition = initialPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }

    // Move the opponent towards the target position using NavMeshAgent
    void MoveTowardsTarget()
    {
        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(targetPosition);
        
    }
}