using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : StageMechanicsController
{
    public bool isActivated; // 포탈 켜졌는지 아닌지 
    public override int Idx { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
        isActivated = false;
        PosSymbol();
    }

    void FixedUpdate() {
        symbolObjectPos.transform.Rotate(symbolRotAxis * symbolRotSpeed * Time.deltaTime); // 빙글빙글 회전 효과
    }

    private void PosSymbol() {
        symbolInitPos = symbolObjectPos.transform.position;
        symbolInitRot = symbolObjectPos.transform.rotation;
        symbolObjectPos = Instantiate(symbolObject, symbolInitPos, symbolInitRot);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isActivated)
        {
            StageManager.Instance.SetStageCleared(); // 스테이지 클리어 됨 
            SceneManager.LoadScene("Stage0");
        }
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
