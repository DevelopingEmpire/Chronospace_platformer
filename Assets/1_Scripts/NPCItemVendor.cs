using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering; // dot ween 사용 

public class NPCItemVendor : MonoBehaviour
{
    [Header("Vended Object")]
    public GameObject vendedObject;
    public GameObject objectSpawnPos;
    public GameObject objectExampleSpawnPos;
    public Vector3 objSpawnOffset = new Vector3(0, 0.2f, 0);

    [Header("UI")]
    public GameObject extUI;
    private TextMeshProUGUI dialogueUIExt;

    [Header("Status")]
    bool isPlayerDetected = false;
    float timeElapsed;
    public float timeDuration = 4f;

    // Start is called before the first frame update
    void Start()
    {
        if(vendedObject == null)
        {
            Debug.LogError("Vended Object is not defined.");
        }
        timeElapsed = timeDuration;
        //Rendered Object Spawn
        objectExampleSpawnPos = Instantiate(vendedObject, objectExampleSpawnPos.transform);
        Debug.Log(objectExampleSpawnPos);
        if(objectExampleSpawnPos.GetComponent<Item>()) {
            Item targetScriptItem = objectExampleSpawnPos.GetComponent<Item>();
            targetScriptItem.enabledToUse = false;
        }
        else {
            Debug.Log("Vended Object Example is not item-typed object. Spawned as normal mode.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime; // 경과 시간 누적

        if (timeElapsed >= timeDuration) // 일정 시간이 지나면 spawnObject 함수를 호출할 수 있음
        {
            spawnObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(dialogueUIExt) {
                dialogueUIExt.text = "자판기 내 물체를 소환하려면 [E]를 누르십시오.";
                dialogueUIExt.enabled = true;
            }
            isPlayerDetected = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(dialogueUIExt) {
                dialogueUIExt.text = "";
                dialogueUIExt.enabled = false;
            }
            isPlayerDetected = false;
        }
    }

    void spawnObject()
    {
        if (isPlayerDetected){
            if (Input.GetButtonDown("Interaction"))
            {
                if(objectSpawnPos){
                    GameObject VendedObjectPos = new GameObject("VendedObject");
                    VendedObjectPos.transform.position = objSpawnOffset + objectSpawnPos.transform.position;
                    Instantiate(vendedObject, VendedObjectPos.transform);
                }
                else{
                    Instantiate(vendedObject, transform);
                }
                timeElapsed = 0f;
            }
        }
    }
}
