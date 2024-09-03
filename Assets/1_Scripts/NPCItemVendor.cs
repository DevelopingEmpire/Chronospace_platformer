using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using DG.Tweening; // dot ween 사용 

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
        AssignMeshToRoot(vendedObject);
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
                AssignMeshToRoot(vendedObject);

            }
        }
    }

    void AssignMeshToRoot(GameObject instantiatedObject)
    {
        // 자식 오브젝트 중 첫 번째로 발견된 메시 필터와 메시 렌더러를 objectExampleSpawnPos에 할당합니다.
        MeshFilter rootMeshFilter = objectExampleSpawnPos.GetComponent<MeshFilter>();
        MeshRenderer rootMeshRenderer = objectExampleSpawnPos.GetComponent<MeshRenderer>();
        Debug.Log(instantiatedObject + "의 메시" + rootMeshFilter.mesh + "좌표에 표시함");

        // 기존에 없을 경우 추가합니다.
        if (rootMeshFilter == null)
        {
            rootMeshFilter = objectExampleSpawnPos.AddComponent<MeshFilter>();
        }

        if (rootMeshRenderer == null)
        {
            rootMeshRenderer = objectExampleSpawnPos.AddComponent<MeshRenderer>();
        }

        // 자식 오브젝트들 탐색
        foreach (Transform child in instantiatedObject.transform)
        {
            MeshFilter childMeshFilter = child.GetComponent<MeshFilter>();
            MeshRenderer childMeshRenderer = child.GetComponent<MeshRenderer>();
            Debug.Log(child + "의 메시" + childMeshFilter.mesh + "좌표에 표시함");

            if (childMeshFilter != null)
            {
                // 자식의 메시를 복사하여 objectExampleSpawnPos의 MeshFilter에 할당합니다.
                rootMeshFilter.mesh = childMeshFilter.mesh;
            }

            if (childMeshRenderer != null)
            {
                // 자식의 머티리얼을 복사하여 objectExampleSpawnPos의 MeshRenderer에 할당합니다.
                rootMeshRenderer.materials = childMeshRenderer.materials;
            }
        }
    }

}
