using UnityEngine;
using UnityEngine.AI;
using System; // �̺�Ʈ ���� ���� ������ 
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
    public CharacterController controller; // ��Ʈ�ѷ�

    bool isGravity; // �߷��� �޴� �����ΰ�? 
    //�̺�Ʈ ���� 
    public bool ISGROUNDEDEVENT; // ������ ������ . �̺�Ʈ���� ���� ���� 
  
    bool isGroundedEvent // controller�� isgrounded ���°� ���ߴ��� �����Ѵ�  
    {
        set
        {
            if(ISGROUNDEDEVENT == value) return; // ������ ���ٸ� �Ѿ 
            ISGROUNDEDEVENT = value; // �޶����ٸ�? �ϴ� �� �ٲ��ְ� 
            //�� ���� 
            //���� �����ߴٸ�? 
            if(ISGROUNDEDEVENT)
            {
                isGravity = false; // �߷� ���� �� 
                navMeshAgent.enabled = true; // ���ش� 
            }
        }
        get
        {
            return ISGROUNDEDEVENT;
        }
    }

    /// <summary>
    /// �߷� �������̽� ������ 
    /// 
    bool isInRange = false; // �߷� ���� ���� �ִ°� 
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

    public void AntiGravity() // �߷� ���� �Լ� 
    {
        isInRange = true;
        isGravity = true;

        // nav ��Ȱ 
        navMeshAgent.enabled = false;
        Gravity = 9.81f;

        Debug.Log("AntiGravity On.");
    }
    public void AntiGravityEnd()
    {
        isInRange = false;
        Gravity = -9.81f; // ���� ���� 
        Debug.Log("AntiGravity Off.");

    }

    //�߷��� ���ϴ� �Լ� 
    void ApplyGravity()
    {
        // ���� �������� �߷��� ����.
        Vector3 gravityVector = new Vector3(0, Gravity, 0);

        // ��ڽ��� ������. Lerp���� ��  
        gravityVector = Vector3.Lerp(controller.velocity, gravityVector, Time.deltaTime);

        // �߷� ���͸� ���� ��ġ�� ����
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

    }

    // Update is called once per frame
    void Update()
    {
        //is Grounded ���°� ���ߴ��� ����
        isGroundedEvent = controller.isGrounded;

        if (!isGravity) // �߷� �޴� ���°� �ƴ϶�� 
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
        else // �߷� �޴� ���¶�� 
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