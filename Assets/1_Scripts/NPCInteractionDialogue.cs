using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class NPCInteractionDialogue : MonoBehaviour
{
    bool isPlayerDetected = false;
    public TextAsset dialogueSource;
    public TextMeshProUGUI dialogueInstrument;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerDetected && Input.GetButtonDown("Interaction"))
        {
            Debug.Log("Interaction with NPC has called.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isPlayerDetected = true;
            Debug.Log("Interaction with NPC will be started.");
            dialogueInstrument.enabled = true; // ui 끄기 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isPlayerDetected = false;
        Debug.Log("Interaction with NPC has ended.");
        dialogueInstrument.enabled = false; // ui 끄기 
    }
}
