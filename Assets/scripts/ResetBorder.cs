using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ResetBorder : MonoBehaviour
{
    // The tag of the player object
    public string playerTag1 = "Player";
    public string playerTag2 = "Player2";
    Vector3 startingPosition1;
    Vector3 startingPosition2;

    // The starting position where the player will be reset
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public Vector3 startingPositionOffset = new Vector3(0f, 1f, 0f);

    void Awake()
    {
        startingPosition1 = spawnPoint1.transform.position + startingPositionOffset;
        startingPosition2 = spawnPoint2.transform.position + startingPositionOffset;
    }

    // Called when another collider enters the trigger collider attached to this object
    void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider has the specified tag (is a player)
        if (other.CompareTag(playerTag1))
        {
            ResetPlayerPosition(other.transform, startingPosition1);
        }
        else if (other.CompareTag(playerTag2))
        {
            ResetPlayerPosition(other.transform, startingPosition2);
        }
    }

    // Reset player's position using CharacterController methods
    void ResetPlayerPosition(Transform playerTransform, Vector3 resetPosition)
    {
        CharacterController characterController = playerTransform.GetComponent<CharacterController>();

        if (characterController != null)
        {
            // Reset the player's position using CharacterController methods
            characterController.enabled = false; // Disable the controller temporarily to set position
            characterController.transform.position = resetPosition;
            characterController.enabled = true; // Enable the controller back
        }
        else
        {
            Debug.LogWarning("CharacterController component not found on the player object.");
        }
    }
}
