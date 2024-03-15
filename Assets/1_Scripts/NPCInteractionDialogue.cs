using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class NPCInteractionDialogue : MonoBehaviour
{
    public string npcName = "NPC";
    public TextAsset dialogueSource;
    public TextMeshProUGUI dialogueInstrument;

    bool isPlayerDetected = false;
    private string dialogueInstrumentInitialVal;
    private string[] dialogueList;
    public int dialogueLnNumber;

    // Start is called before the first frame update
    void Start()
    {
        dialogueInstrumentInitialVal = dialogueInstrument.text;
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
            dialogueInstrument.text = "To talk with " + npcName + ", press [E] to continue.";
            dialogueInstrument.enabled = true; // ui 끄기 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        dialogueLnNumber = 0;
        isPlayerDetected = false;
        Debug.Log("Interaction with NPC has ended.");
        dialogueInstrument.text = dialogueInstrumentInitialVal;
        dialogueInstrument.enabled = false; // ui 끄기 
    }

    void showDialogue()
    {
        if (isPlayerDetected){
            if (Input.GetButtonDown("Interaction"))
            {
                if (dialogueLnNumber<dialogueList.Length) {
                    dialogueInstrument.text = npcName + " : " + dialogueList[dialogueLnNumber];
                    //Debug.Log(dialogueList[dialogueLnNumber]);
                    dialogueLnNumber++;
                }
                else {
                    dialogueLnNumber = 0;
                    dialogueInstrument.text = dialogueList[dialogueLnNumber];
                    dialogueInstrument.enabled = false; // ui 끄기 
                }
            }
        }
    }

}
