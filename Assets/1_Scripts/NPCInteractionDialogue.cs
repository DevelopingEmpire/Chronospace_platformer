using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using DG.Tweening; // dot ween 사용 

public class NPCInteractionDialogue : MonoBehaviour
{
    [Header("NPC & Dialogue Content")]
    public string npcName = "NPC";
    public TextAsset dialogueSource;

    [Header("Content Display")]
    public GameObject dialogueUI;
    private TextMeshProUGUI dialogueUIName;
    private TextMeshProUGUI dialogueUIContent;

    [Header("External-Content Display")]
    public GameObject extUI;
    private TextMeshProUGUI dialogueUIExt;

    [Header("Dialog Status")]
    bool isPlayerDetected = false;
    private string dialogueUIContentInitialVal;
    private string[] dialogueList;
    private int dialogueLnNumber;

    // Start is called before the first frame update
    void Start()
    {
        // 각 컴포넌트들 캐싱 
        dialogueUIName = dialogueUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        dialogueUIContent = dialogueUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        dialogueUIExt = extUI.GetComponent<TextMeshProUGUI>();

        dialogueUI.SetActive(false);
        dialogueUIContentInitialVal = dialogueUIExt.text;
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

            //dialogueUIName.text = npcName;
            dialogueUIExt.text = "To talk with " + npcName + ", press [E] to continue.";
            dialogueUIExt.enabled = true; // ui 끄기 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        dialogueLnNumber = 0;
        isPlayerDetected = false;
        Debug.Log("Interaction with NPC has ended.");

        dialogueUI.SetActive(false); // ui 끄기
        if(extUI != null){
            extUI.SetActive(true);
        }
        //dialogueUIName.text = "";
        dialogueUIExt.text = dialogueUIContentInitialVal;
        dialogueUIExt.enabled = false; // ui 끄기 
    }

    void showDialogue()
    {
        if (isPlayerDetected){
            if (Input.GetButtonDown("Interaction"))
            {
                if (dialogueLnNumber<dialogueList.Length) {
                    if(dialogueLnNumber == 0){
                        if(extUI != null){
                            extUI.SetActive(false);
                        }
                        dialogueUI.SetActive(true);
                    }
                    dialogueUIContent.text = dialogueList[dialogueLnNumber];
                    //Debug.Log(dialogueList[dialogueLnNumber]);
                    dialogueLnNumber++;
                }
                else {
                    dialogueLnNumber = 0;
                    dialogueUIContent.text = dialogueList[dialogueLnNumber];
                    //dialogueUIContent.enabled = false; // ui 끄기 
                }
            }
        }
    }

}
