using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : StageMechanicsController
{

    [Header("Laser")]
    RaycastHit hit;
    public LineRenderer lineRenderMethod; // 얘가 선을 그어줄거야! 
    public Vector3 newPosition;
    public Vector3 newDir;

    [Header("Interactions")]
    public int idx; // 인스펙터에서 정해주기  
    public GameObject lastPressedButton;
    public bool activated; // 활성화 여부 

    [Header("Material Changes")]
    public GameObject[] selfMesh;
    public GameObject[] selfMeshLight;
    public int[] selfRecoloredMaterials;
    public int[] selfRecoloredMaterialsGlow;

    private MeshRenderer meshRenderer;
    private Light meshRendererLight;
    private Material[] originalMaterials;
    private Material importedMaterial;
    private Material importedMaterialGlow;

    public override int Idx { get; set; }

    private void Start()
    {
        Idx = idx; // 인스펙터에서 지정한 값을 Idx에 저장 
        activated = false;
        lineRenderMethod.material = importedMaterialGlow;

        foreach(GameObject selfML in selfMeshLight) {
            selfML.SetActive(false);
        }
    }

    private void Update()
    {
        if (activated) LaserOn();
    }
    public override void Trigger()
    {
        activated = true;
        for (int i = 0; i<selfMesh.Length; i++){
            if(selfRecoloredMaterialsGlow[i] != -1) {
                meshRenderer = selfMesh[i].GetComponent<MeshRenderer>();
                originalMaterials = meshRenderer.sharedMaterials;
                originalMaterials[selfRecoloredMaterialsGlow[i]] = importedMaterialGlow;
                meshRenderer.sharedMaterials = originalMaterials;
            }
        }

        foreach(GameObject selfML in selfMeshLight) {
            selfML.SetActive(true);
        }
    }

    public override void Exit()
    {
        activated = false;
        lineRenderMethod.positionCount = 0;

        for (int i = 0; i<selfMesh.Length; i++){
            if(selfRecoloredMaterialsGlow[i] != -1) {
                meshRenderer = selfMesh[i].GetComponent<MeshRenderer>();
                originalMaterials = meshRenderer.sharedMaterials;
                originalMaterials[selfRecoloredMaterialsGlow[i]] = importedMaterial;
                meshRenderer.sharedMaterials = originalMaterials;
            }
        }

        foreach(GameObject selfML in selfMeshLight) {
            selfML.SetActive(false);
        }

        if(lastPressedButton!=null) {
            lastPressedButton.GetComponent<ButtonController>().Exit(); // 눌린거 꺼주기 
        }
        else{
            Debug.Log("레이저 비활성화로 비활성화할 버튼이 없음.");
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
            foreach(GameObject selfML in selfMeshLight) {
                meshRendererLight = selfML.GetComponent<Light>();
                meshRendererLight.color = importedMaterial.color;
                selfML.SetActive(false);
            }
        }
    }

    public void LaserOn()
    {
        List<Vector3> positions = new List<Vector3>(); // 그릴 점들 리스트 

        //newPosition = transform.position + transform.forward;
        newPosition = transform.position;
        newDir = transform.forward;

        positions.Add(newPosition);


        while (true)
        {
            Physics.Raycast(newPosition, newDir, out hit);
            positions.Add(hit.point + (transform.forward * 0.5f));
            //positions.Add(hit.point);
            if (hit.collider.gameObject.CompareTag("mirror"))
            {
                newPosition = hit.point;
                newDir = Vector3.Reflect(newDir, hit.normal); // 반사! 
            }
            else
            {
                //버튼에 닿았다면!! 
                if (hit.collider.gameObject.CompareTag("Button") && hit.collider.gameObject.GetComponent<ButtonController>())
                {
                    lastPressedButton = hit.collider.gameObject;
                    lastPressedButton.GetComponent<ButtonController>().Trigger(); // 버튼 누르기 
                }
                // 버튼에 닿지 않았다면~ 
                else
                {
                    // 이전에 버튼에 닿았었다면, 그 버튼 꺼주고 null 
                    if (lastPressedButton != null)
                    {
                        lastPressedButton.GetComponent<ButtonController>().Exit(); // 눌린거 꺼주기 
                        lastPressedButton = null;
                    }
                }
                break;
            }

        }

        // 레이저 한붓그리기 
        lineRenderMethod.positionCount = positions.Count; // 정점 추가 
        for (int i = 0; i < positions.Count; i++)
        {
            lineRenderMethod.SetPosition(i, positions[i]);
        }
    }

}
