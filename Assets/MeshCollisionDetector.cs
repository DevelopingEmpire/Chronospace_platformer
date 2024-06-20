using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCollisionDetector : MonoBehaviour
{
    public bool isPlayerDetected = false;
    public GameObject nearestPlayer = null;
    public List<GameObject> playersInRange = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInRange.Add(other.gameObject);
            UpdateNearestPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInRange.Remove(other.gameObject);
            UpdateNearestPlayer();
        }
    }

    private void UpdateNearestPlayer()
    {
        isPlayerDetected = playersInRange.Count > 0;
        nearestPlayer = null;

        float nearestDistance = float.MaxValue;

        foreach (GameObject player in playersInRange)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPlayer = player;
            }
        }

        if (isPlayerDetected)
        {
            Debug.Log("Player detected: " + nearestPlayer.name);
        }
        else
        {
            Debug.Log("No player detected");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpdateNearestPlayer();
        }
    }
}
