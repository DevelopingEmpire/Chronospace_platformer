using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goalpoint : MonoBehaviour
{
    // The tag of the player objects
    public string playerTag = "Player";

    // Called when another collider enters the trigger collider attached to this object
    void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider has the specified tag (is a player)
        if (other.CompareTag(playerTag))
        {
            Debug.Log("(From Debug Log on Goalpoint)Stage completed! Player reached the goal!");
            // You can add additional logic here, such as loading the next level
        }
    }
}
