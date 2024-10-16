using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening; // dot ween 사용 

public class InteractionObjectRotatable : StageMechanicsController
{
    [Header("Interaction Target")]
    public GameObject targetObjects;
    public bool activated; // 활성화 여부 

    [Header("Target Transition")]
    public float speed = 2.0f;

    [Header("Material Changes")]
    public GameObject[] selfMesh;
    public GameObject selfMeshLight;
    public int[] selfRecoloredMaterials;
    public int[] selfRecoloredMaterialsGlow;

    private MeshRenderer meshRenderer; //오브젝트의 메시 렌더링 시스템을 불러올 때 사용됨
    private Light meshRendererLight;
    private Material[] originalMaterials; //오브젝트의 머티리얼 리스트를 임시로 불러올 때 사용됨
    private Material importedMaterial; //스위치 오브젝트에서 받아온 머티리얼을 저장
    private Material importedMaterialGlow; //스위치 오브젝트에서 받아온 발광 머티리얼을 저장

    //other vars are granted from switch

    public override int Idx { get; set; }

    void Update()
    {
        if(activated) {
            targetObjects.transform.Rotate(Vector3.up * 10 * speed * Time.deltaTime); // 빙글빙글 회전 효과
        }
    }

    // Update is called once per frame

    public override void Trigger()
    {
        activated = true;

        selfMeshLight.SetActive(true);
        for (int i = 0; i<selfMesh.Length; i++){
            if(selfRecoloredMaterialsGlow[i] != -1) {
                meshRenderer = selfMesh[i].GetComponent<MeshRenderer>();
                originalMaterials = meshRenderer.sharedMaterials;
                originalMaterials[selfRecoloredMaterialsGlow[i]] = importedMaterialGlow;
                meshRenderer.sharedMaterials = originalMaterials;
            }
        }
    }

    public override void Exit()
    {
        activated = false;

        selfMeshLight.SetActive(false);
        for (int i = 0; i<selfMesh.Length; i++){
            if(selfRecoloredMaterialsGlow[i] != -1) {
                meshRenderer = selfMesh[i].GetComponent<MeshRenderer>();
                originalMaterials = meshRenderer.sharedMaterials;
                originalMaterials[selfRecoloredMaterialsGlow[i]] = importedMaterial;
                meshRenderer.sharedMaterials = originalMaterials;
            }
        }
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
