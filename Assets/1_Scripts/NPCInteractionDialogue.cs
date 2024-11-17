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
    public string npcInteractionDesc = "와/과의 대화를 위해 [E]를 누르십시오.";
    public TextAsset dialogueSource;

    [Header("Content Display")]
    public GameObject dialogueUI;
    private TextMeshProUGUI dialogueUIName;
    private TextMeshProUGUI dialogueUIContent;

    [Header("External-Content Display")]
    public GameObject extUI;
    public GameObject[] excludedUI;
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
        dialogueUI = CanvasScripts.instance.transform.Find("MainScreen").GetChild(2).gameObject;
        extUI = CanvasScripts.instance.transform.Find("MainScreen").GetChild(1).gameObject;

        dialogueUIName = dialogueUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        dialogueUIContent = dialogueUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        dialogueUIExt = extUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

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
        if (dialogueSource) showDialogue();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            dialogueLnNumber = 0;
            isPlayerDetected = true;
            Debug.Log("Interaction with NPC will be started.");

            dialogueUIName.text = npcName;
            dialogueUIExt.text = npcName + npcInteractionDesc;
            dialogueUIExt.enabled = true;
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
        dialogueUIName.text = "";
        dialogueUIExt.text = dialogueUIContentInitialVal;
        dialogueUIExt.enabled = false; // ui 끄기 
        if(excludedUI != null) {
            for(int i = 0; i < excludedUI.Length; i++) {
                if(excludedUI[i] && excludedUI[i].activeSelf == false){
                    excludedUI[i].SetActive(true);
                }
            }
        }
    }

    void showDialogue()
    {
        if (isPlayerDetected){
            if (Input.GetButtonDown("Interaction"))
            {
                if(excludedUI != null) {
                    for(int i = 0; i < excludedUI.Length; i++) {
                        if(excludedUI[i] && excludedUI[i].activeSelf == true){
                            excludedUI[i].SetActive(false);
                        }
                    }
                }
                if (dialogueLnNumber<dialogueList.Length) {
                    if(dialogueLnNumber == 0){
                        if(extUI != null){
                            extUI.SetActive(false);
                        }
                        dialogueUI.SetActive(true);
                    }
                    dialogueUIContent.text = dialogueList[dialogueLnNumber].Replace("\\n", "\n");
                    //줄바꿈 문자 지원하기
                    dialogueLnNumber++;
                }
                else {
                    dialogueLnNumber = 0;
                    dialogueUIContent.text = dialogueList[dialogueLnNumber].Replace("\\n", "\n");
                }
            }
        }
    }
}
