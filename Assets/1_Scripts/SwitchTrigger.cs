using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    [Header("Interaction Object")]
    public GameObject[] targetObjects;
    public string targetObjectElement = "InteractionObject";
    public InteractionObject targetFuncScript;
    public bool activated;

    [Header("Timer")]
    public bool hasTimer = false;
    public float timerValue = 5f;

    [Header("Material Changes")]
    public Color selfColor;
    public GameObject selfMesh; //childObj
    public GameObject selfMeshLight; //childObjLight
    public int[] selfRecoloredMaterials;
    public int[] selfRecoloredMaterialsGlow;
    public Material selfRefMaterial;
    public Material selfRefMaterialGlow;
    //private
    private MeshRenderer meshRenderer;
    private Light meshRendererLight;
    private MeshRenderer meshRendererTarget;
    private Light meshRendererLightTarget;
    private Material[] originalMaterials;
    private Material[] originalMaterialsTarget;
    private Material newMaterial;
    private Material newGlowMaterial;
    private GameObject[] targetGameObjects;
    private GameObject targetLightObject;
    private int recoloredMaterialsTarget;
    private int recoloredMaterialsGlowTarget;

    void Start()
    {
        activated = false;
        RecolorMaterials();
        foreach (GameObject targetObject in targetObjects)
        {
            if (targetObject != null)
            {
                targetFuncScript = targetObject.GetComponent<InteractionObject>();
                RecolorTargetObject(targetObject);
            }
        }
        targetFuncScript = null;
    }

    public void Activate()
    {
        if (!activated)
        {
            OnSwitchController();
            /*if(hasTimer)
            {
                Invoke("OffSwitchController", timerValue);
            }*/
        }
        else
        {
            OffSwitchController();
        }
    }

    private void OnSwitchController()
    {
        foreach (GameObject targetObject in targetObjects)
        {
            if (targetObject != null)
            {
                //오브젝트 내 메시 오브젝트 지정
                targetFuncScript = targetObject.GetComponent<InteractionObject>();
                targetGameObjects = targetFuncScript.selfMesh;
                int i = 0;
                foreach (GameObject targetGameObject in targetGameObjects)
                {
                    MeshRenderer targetRenderer = targetGameObject.GetComponent<MeshRenderer>();
                    //발광할 머티리얼 찾아서 활성화
                    if (targetRenderer != null && targetFuncScript.selfRecoloredMaterialsGlow[i]>0)
                    {
                        originalMaterialsTarget = targetRenderer.sharedMaterials;
                        recoloredMaterialsGlowTarget = targetFuncScript.selfRecoloredMaterialsGlow[i];
                        originalMaterialsTarget[recoloredMaterialsGlowTarget] = newGlowMaterial;
                        targetRenderer.sharedMaterials = originalMaterialsTarget;
                    }
                    i++;
                }
                //조명 활성화
                targetLightObject = (targetFuncScript.selfMeshLight);
                targetLightObject.SetActive(true);
                //실제 작동
                targetFuncScript.Activate();
            }
        }

        meshRenderer = selfMesh.GetComponent<MeshRenderer>();
        foreach (int index in selfRecoloredMaterialsGlow)
        {
            if (index >= 0 && index < originalMaterials.Length)
            {
                originalMaterials[index] = newGlowMaterial;
            }
        }
        //originalMaterialsTarget[selfRecoloredMaterialsGlowTarget] = newGlowMaterial;
        meshRenderer.sharedMaterials = originalMaterials;
        selfMeshLight.SetActive(true);
        
        //스위치 작동 활성화 기록
        activated = true;
    }

    private void OffSwitchController()
    {
        if (targetObjects != null && targetObjects.Length > 0)
        {
            foreach (GameObject targetObject in targetObjects)
            {
                //오브젝트 내 메시 오브젝트 지정
                targetFuncScript = targetObject.GetComponent<InteractionObject>();
                targetGameObjects = targetFuncScript.selfMesh;
                int i = 0;
                foreach (GameObject targetGameObject in targetGameObjects)
                {
                    MeshRenderer targetRenderer = targetGameObject.GetComponent<MeshRenderer>();
                    //발광할 머티리얼 찾아서 활성화
                    if (targetRenderer != null && targetFuncScript.selfRecoloredMaterialsGlow[i] > 0)
                    {
                        originalMaterialsTarget = targetRenderer.sharedMaterials;
                        recoloredMaterialsGlowTarget = targetFuncScript.selfRecoloredMaterialsGlow[i];
                        originalMaterialsTarget[recoloredMaterialsGlowTarget] = newMaterial;
                        targetRenderer.sharedMaterials = originalMaterialsTarget;
                    }
                    i++;
                }
                //조명 비활성화
                targetLightObject = (targetFuncScript.selfMeshLight);
                targetLightObject.SetActive(false);
                //실제 작동
                targetFuncScript.Disactivate();
            }
        }
        
        foreach (int index in selfRecoloredMaterialsGlow)
        {
            if (index >= 0 && index < originalMaterials.Length)
            {
                originalMaterials[index] = newMaterial;
            }
        }
        //originalMaterialsTarget[recoloredMaterialsGlowTarget] = newMaterial;
        meshRenderer.sharedMaterials = originalMaterials;
        selfMeshLight.SetActive(false);
        
        // Other actions when deactivated
        activated = false;
    }

   

    void RecolorMaterials()
    {
        if (selfMesh != null)
        {
            meshRenderer = selfMesh.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                originalMaterials = meshRenderer.sharedMaterials;

                // Recolor main materials
                foreach (int index in selfRecoloredMaterials)
                {
                    if (index >= 0 && index < originalMaterials.Length)
                    {
                        newMaterial = new Material(selfRefMaterial);
                        newMaterial.color = selfColor;
                        originalMaterials[index] = newMaterial;
                    }
                }
                // Recolor glow materials
                foreach (int index in selfRecoloredMaterialsGlow)
                {
                    if (index >= 0 && index < originalMaterials.Length)
                    {
                        newGlowMaterial = new Material(selfRefMaterialGlow);
                        newGlowMaterial.color = selfColor;
                        newGlowMaterial.SetColor("_EmissionColor", selfColor); // Set emission color
                        originalMaterials[index] = newMaterial;
                    }
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

        meshRendererLight = selfMeshLight.GetComponent<Light>();
        meshRendererLight.color = selfColor;
        selfMeshLight.SetActive(false);
    }

    void RecolorTargetObject(GameObject targetObjectEach)
    {
        //targetGameObjects = targetObjectEach;
        //targetFuncScript = targetObjectEach.GetComponent<InteractionObject>();
        targetGameObjects = (targetFuncScript.selfMesh);
        targetLightObject = (targetFuncScript.selfMeshLight);

        int i = 0;
        foreach(GameObject targetObject in targetGameObjects)
        {
            meshRendererTarget = targetObject.GetComponent<MeshRenderer>();
            if (meshRendererTarget != null)
            {
                originalMaterialsTarget = meshRendererTarget.sharedMaterials;

                recoloredMaterialsTarget = targetFuncScript.selfRecoloredMaterials[i];
                recoloredMaterialsGlowTarget = targetFuncScript.selfRecoloredMaterialsGlow[i];

                if (recoloredMaterialsTarget >= 0 && (recoloredMaterialsTarget <= originalMaterialsTarget.Length))
                {
                    originalMaterialsTarget[recoloredMaterialsTarget] = newMaterial;
                }
                if (recoloredMaterialsGlowTarget >= 0 && (recoloredMaterialsGlowTarget <= originalMaterialsTarget.Length))
                {
                    originalMaterialsTarget[recoloredMaterialsGlowTarget] = newMaterial;
                }
                meshRendererTarget.sharedMaterials = originalMaterialsTarget;
            }
            i++;
        }
        meshRendererLightTarget = targetLightObject.GetComponent<Light>();

        if (meshRendererLightTarget != null)
        {
            meshRendererLightTarget.color = selfColor;
            targetLightObject.SetActive(false);
        }

    }
}
