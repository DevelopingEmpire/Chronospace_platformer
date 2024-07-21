using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : StageMechanicsController
{
    public bool isActivated; // 포탈 켜졌는지 아닌지 
    public override int Idx { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public string targetScene;

    [Header("Symbol")]
    public GameObject symbolObject;
    public GameObject symbolObjectPos;
    private Vector3 symbolInitPos;
    private Quaternion symbolInitRot;
    public Vector3 symbolRotAxis = new Vector3(0, 0, 1);
    public float symbolRotSpeed = 1.0f;

    [Header("Save and Load")]
    public StageManager smanager;

    [Header("Material Changes")]
    public GameObject[] selfMesh;
    public GameObject selfMeshLight;
    public int[] selfRecoloredMaterials;
    public int[] selfRecoloredMaterialsGlow;

    private MeshRenderer meshRenderer;
    private Light meshRendererLight;
    private Material[] originalMaterials;
    private Material importedMaterial;
    private Material importedMaterialGlow;

    private void Start()
    {
        PosSymbol();
    }

    void FixedUpdate() {
        if (symbolObjectPos != null) {
            symbolObjectPos.transform.Rotate(symbolRotAxis * symbolRotSpeed * Time.deltaTime, Space.World); // 빙글빙글 회전 효과
        }
    }

    private void PosSymbol() {
        if (symbolObjectPos != null)
        {
            symbolInitPos = symbolObjectPos.transform.position;
            symbolInitRot = symbolObjectPos.transform.rotation;
            symbolObjectPos = Instantiate(symbolObject, symbolInitPos, symbolInitRot);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("포탈에 닿음");
        // 플레이어가 액티브 포탈에 닿았는지 확인 
        if (!(other.CompareTag("Player") && isActivated)) {
            Debug.Log("활성화 안됨");
            return;
        }
        Debug.Log("활성화 확인");

        /*
        // 스테이지 입장 가능한 상태인지 확인 
        if (!StageManager.Instance.CanEnterStage(targetScene)) return;
        Debug.Log("스테이지 입장가능 ");
        */
        

        // 스테이지 이동 가즈아 

        // if stage0 으로 가는거면 그냥 가고, 아니면 스테이지 클리어 처리 
        if(targetScene == "Stage0" || targetScene == "Stage1")
        {
            StageManager.Instance.SetStageCleared(); // 현재 스테이지 클리어 됨 
        }

        GameManager.Instance.LoadSceneCall(targetScene); // 다음 스테이지 로드 요청

        Debug.Log("게임매니저 - 로드씬 요청 됨 ");

    }

    public override void Trigger()
    {
        isActivated = true;

        for (int i = 0; i<selfMesh.Length; i++){
            if(selfRecoloredMaterialsGlow[i] != -1) {
                meshRenderer = selfMesh[i].GetComponent<MeshRenderer>();
                originalMaterials = meshRenderer.sharedMaterials;
                originalMaterials[selfRecoloredMaterialsGlow[i]] = importedMaterialGlow;
                meshRenderer.sharedMaterials = originalMaterials;
            }
        }
        
        selfMeshLight.SetActive(true);
    }

    public override void Exit()
    {
        isActivated = false;

        for (int i = 0; i<selfMesh.Length; i++){
            if(selfRecoloredMaterialsGlow[i] != -1) {
                meshRenderer = selfMesh[i].GetComponent<MeshRenderer>();
                originalMaterials = meshRenderer.sharedMaterials;
                originalMaterials[selfRecoloredMaterialsGlow[i]] = importedMaterial;
                meshRenderer.sharedMaterials = originalMaterials;
            }
        }

        selfMeshLight.SetActive(false);
    }

    public override void SetInitialColor(Material targetColor, Material targetColorGlow)
    {
        importedMaterial = targetColor;
        importedMaterialGlow = targetColorGlow;
        for (int i = 0; i<selfMesh.Length; i++){
            if(selfRecoloredMaterialsGlow[i] != -1) {
                meshRenderer = selfMesh[i].GetComponent<MeshRenderer>();
                originalMaterials = meshRenderer.sharedMaterials;
                originalMaterials[selfRecoloredMaterials[i]] = importedMaterial;
                originalMaterials[selfRecoloredMaterialsGlow[i]] = importedMaterial;
                meshRenderer.sharedMaterials = originalMaterials;
            }
        }

        if (selfMeshLight != null)
        {
            meshRendererLight = selfMeshLight.GetComponent<Light>();
            meshRendererLight.color = importedMaterial.color;
            selfMeshLight.SetActive(false);
        }
    }
}
