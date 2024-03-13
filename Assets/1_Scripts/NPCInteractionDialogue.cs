using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class NPCInteractionDialogue : MonoBehaviour
{
    bool isPlayerDetected = false;
    public TextAsset dialogueSource;
    private string[] dialogueList;
    private int dialogueLnNumber;
    public TextMeshProUGUI dialogueInstrument;

    // Start is called before the first frame update
    void Start()
    {
        if(dialogueSource != null)
        {
            dialogueList = dialogueSource.text.Split('\n'); // Split text into lines
            Debug.Log("Interaction with NPC content loaded.");
        }
        else
        {
            Debug.LogError("Dialogue file is not defined.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        showDialogue();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            dialogueLnNumber = 0;
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

    void showDialogue()
    {
        if (isPlayerDetected){
            if (Input.GetButtonDown("Interaction"))
            {
                if (dialogueLnNumber<dialogueList.Length) {
                    Debug.Log(dialogueList[dialogueLnNumber]);
                    dialogueLnNumber++;
                }
                else 
                {
                    dialogueLnNumber = 0;
                }
            }
        }
    }

}
