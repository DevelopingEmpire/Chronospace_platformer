using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Guards : MonoBehaviour
{
    // The tag of the player object
    public string playerTag = "Player";
    public Vector3 initialPosition;

    public float travelDistance = 5f;
    public float sightRange = 10f;
    public float adressRange = 5f;
    public float rotationSpeed = 5f;

    public GameObject projectileObj;
    public float fireDelay = 1f;
    private float fireTimer = 0f;
    public Vector3 fireOffset;

    public float patrolDelay = 2f;

    // Reference to the NavMeshAgent component
    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;
    private int state = 0;
    private float nextPatrolTime;

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
        // If the player is in sight, set the target position to the player's position
        if (PlayerInSight())
        {
            state = 1; // Change to stare at player state
            StareAtPlayer();
            if(PlayerInAdressRange())
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
        else
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

        // Move towards the target position using NavMeshAgent
    }

    // Check if the player is in sight
    bool PlayerInSight()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (GameObject.FindGameObjectWithTag(playerTag).transform.position - transform.position).normalized;

        // Perform a raycast to check if there are obstacles between the opponent and the player
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange))
        {
            if (hit.collider.CompareTag(playerTag))
            {
                // Player is in sight
                return true;
            }
        }

        // Player is not in sight
        return false;
    }

    bool PlayerInAdressRange()
    {
        return (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag(playerTag).transform.position) > adressRange) ? true : false;
    }

    // Stare at the player by rotating the opponent's direction
    void StareAtPlayer()
    {
        Vector3 directionToPlayer = (GameObject.FindGameObjectWithTag(playerTag).transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    //Fire projectile into player
    void Fire()
    {
        // Check if enough time has passed to fire a bullet
        if (Time.time >= fireTimer)
        {
            GameObject projectileIns = Instantiate(projectileObj);
            projectileIns.transform.position = transform.position + fireOffset;
            projectileIns.SetActive(true);
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
        Vector2 randomCircle = Random.insideUnitCircle * travelDistance;
        targetPosition = initialPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }

    // Move the opponent towards the target position using NavMeshAgent
    void MoveTowardsTarget()
    {
        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(targetPosition);
    }
}