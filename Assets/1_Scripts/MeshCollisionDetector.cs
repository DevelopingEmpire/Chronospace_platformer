using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCollisionDetector : MonoBehaviour
{
    public bool isPlayerDetected = false;
    public GameObject nearestPlayer = null;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = true;
            nearestPlayer = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = false;
            nearestPlayer = null; // 비움 
        }
    }

}
