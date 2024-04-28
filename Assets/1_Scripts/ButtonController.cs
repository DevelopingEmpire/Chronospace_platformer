using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : StageMechanicsController 
{
    [SerializeField]
    [Header("Functions")]
    private CharacterController controller; // 컨트롤러 담는 용 변수 
    //public GameObject mesh;// 버튼 부분 메시 

    public Material buttonMat; // 버튼 부분 머티리얼 
    public StageMechanicsController[] triggerObject; // 작동시킬 무언가 
    public int idx; // 구분용 아이디

    [Header("Material Changes")]
    private MeshRenderer meshRenderer;
    private Light meshRendererLight;
    private MeshRenderer meshRendererTarget;
    private Light meshRendererLightTarget;
    private Material[] originalMaterials;
    private Material[] originalMaterialsTarget;
    private Material newMaterial;
    private Material newGlowMaterial;    
    public Color selfColor;

    public GameObject[] selfMesh; //childObj
    public GameObject selfMeshLight; //childObjLight
    public int[] selfRecoloredMaterials;
    public int[] selfRecoloredMaterialsGlow;
    public Material selfRefMaterial;
    public Material selfRefMaterialGlow;

    public override int Idx { get; set; }

    private void Start()
    {
        Idx = idx; // 값 지정.. ?  아 이렇게 쓰지 말라셨는데...? ㅎㅎ;; 
        for (int i = 0; i<selfMesh.Length; i++){
            RecolorMaterialsInit(selfMesh[i], selfRecoloredMaterials[i], selfRecoloredMaterialsGlow[i]);
        }
        meshRendererLight = selfMeshLight.GetComponent<Light>();
        meshRendererLight.color = selfColor;
        selfMeshLight.SetActive(false);

        //타겟 오브젝트 색 채색하기
        foreach (StageMechanicsController tObj in triggerObject) {
            if(tObj != null) {
                tObj.SetInitialColor(newMaterial, newGlowMaterial);
            }
        }
        //buttonMat = mesh.GetComponent<MeshRenderer>().material;
        //buttonMat.SetColor("_EmissionColor", Color.black); // emission 색 검정이면 빛 안남 
    }

    private void OnTriggerEnter(Collider col)
    {
        controller = col.GetComponent<CharacterController>(); // 밟을 수 있는 애들은 다 캐.콘 갖고있음 
        selfMeshLight.SetActive(true);

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (controller != null)
        {
            Trigger();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        controller = col.GetComponent<CharacterController>(); // 밟을 수 있는 애들은 다 캐.콘 갖고있음 
        selfMeshLight.SetActive(false);

        // 컴포넌트 안달린 놈은 null 반환하는데, 걔는 접근하면 오류남{
        if (controller != null)
        {
            Exit();
        }
    }

    public override void SetInitialColor(Material targetColor, Material targetColorGlow)
    {

    }

    public override void Trigger()
    {
        //트리거 오브젝트 작동시키기
        foreach (StageMechanicsController tObj in triggerObject) {
            if(tObj != null) {
                tObj.Trigger();
            }
        }

        //머티리얼 교체
        for (int i = 0; i<selfMesh.Length; i++){
            if(selfRecoloredMaterialsGlow[i] != -1) {
                meshRenderer = selfMesh[i].GetComponent<MeshRenderer>();
                originalMaterials = meshRenderer.sharedMaterials;
                originalMaterials[selfRecoloredMaterialsGlow[i]] = newGlowMaterial;
                meshRenderer.sharedMaterials = originalMaterials;
            }
        }
    }

    public override void Exit()
    {
        //throw new System.NotImplementedException();

        //오브젝트 작동 해제
        foreach (StageMechanicsController tObj in triggerObject) {
            if(tObj != null) {
                tObj.Exit();
            }
        }

        //머티리얼 교체
        for (int i = 0; i<selfMesh.Length; i++){
            if(selfRecoloredMaterialsGlow[i] != -1) {
                meshRenderer = selfMesh[i].GetComponent<MeshRenderer>();
                originalMaterials = meshRenderer.sharedMaterials;
                originalMaterials[selfRecoloredMaterialsGlow[i]] = newMaterial;
                meshRenderer.sharedMaterials = originalMaterials;
            }
        }
    }

    void RecolorMaterialsInit(GameObject targetMesh, int matTarget, int matTargetGlow)
    {
        if (targetMesh != null)
        {
            meshRenderer = targetMesh.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                originalMaterials = meshRenderer.sharedMaterials;

                // Recolor main materials
                if(matTarget != -1){
                    newMaterial = new Material(selfRefMaterial);
                    newMaterial.color = selfColor;
                    originalMaterials[matTarget] = newMaterial;
                }
                // Recolor glow materials
                if(matTargetGlow != -1){
                    newGlowMaterial = new Material(selfRefMaterialGlow);
                    newGlowMaterial.color = selfColor;
                    newGlowMaterial.SetColor("_EmissionColor", selfColor); // Set emission color
                    originalMaterials[matTargetGlow] = newMaterial;
                }

                meshRenderer.sharedMaterials = originalMaterials;
            }
            else
            {
                Debug.LogError("MeshRenderer component not found on selfMesh.");
            }
        }
        else
        {
            Debug.LogError("selfMesh not assigned.");
        }
    }
}
