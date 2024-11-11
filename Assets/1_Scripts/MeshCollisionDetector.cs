using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCollisionDetector : MonoBehaviour
{
    public bool isDetected = false;
    public String target = "Player";
    public GameObject nearestPlayer = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(target))
        {
            isDetected = true;
            nearestPlayer = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(target))
        {
            isDetected = false;
            nearestPlayer = null; // 비움 
        }
    }

}
