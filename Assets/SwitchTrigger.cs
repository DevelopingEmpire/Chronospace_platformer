using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    [Header("Interaction Object")]
    public GameObject targetObject;
    public string targetObjectElement = "InteractionObject";
    InteractionObject targetFuncScript;
    bool activated;

    [Header("Timer")]
    public bool hasTimer = false;
    public float timerValue = 0f;

    [Header("Material Changes")]
    public Color selfColor;
    public GameObject selfMesh;
    public int[] selfRecoloredMaterials;
    public int[] selfRecoloredMaterialsGlow;
    public Material selfRefMaterial;
    public Material selfRefMaterialGlow;
    //private
    private MeshRenderer meshRenderer;
    private MeshRenderer meshRendererTarget;
    private Material[] originalMaterials;
    private Material[] originalMaterialsTarget;
    private Material newMaterial;
    private Material newGlowMaterial;
    private GameObject selfMeshTarget;
    private int selfRecoloredMaterialsTarget;
    private int selfRecoloredMaterialsGlowTarget;

    void Start()
    {
        activated = false;
        if (targetObject != null)
        {
            targetFuncScript = targetObject.GetComponent<InteractionObject>();
        }
        RecolorMaterials();
        RecolorTargetObject();
    }

    public void Activate()
    {
        if (!activated) 
        {
            if (targetFuncScript != null)
            {
                foreach (int index in selfRecoloredMaterialsGlow)
                {
                    if (index >= 0 && index < originalMaterials.Length)
                    {
                        originalMaterials[index] = newGlowMaterial;
                    }
                }
                originalMaterialsTarget[selfRecoloredMaterialsTarget] = newGlowMaterial;
                meshRenderer.sharedMaterials = originalMaterials;
                meshRendererTarget.sharedMaterials = originalMaterialsTarget;
                targetFuncScript.SetActive();
                activated = true;
            }
        }
        else
        {
            if (targetFuncScript != null)
            {
                foreach (int index in selfRecoloredMaterialsGlow)
                {
                    if (index >= 0 && index < originalMaterials.Length)
                    {
                        originalMaterials[index] = newMaterial;
                    }
                }
                originalMaterialsTarget[selfRecoloredMaterialsTarget] = newMaterial;
                meshRenderer.sharedMaterials = originalMaterials;
                meshRendererTarget.sharedMaterials = originalMaterialsTarget;
                targetFuncScript.SetDisactive();
                activated = false;
            }
        }

        //Input.GetKeyDown("a") == true
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
    }

    void RecolorTargetObject()
    {
        selfMeshTarget = (targetFuncScript.selfMesh);
        meshRendererTarget = selfMeshTarget.GetComponent<MeshRenderer>();
        if (meshRendererTarget != null)
        {
            originalMaterialsTarget = meshRendererTarget.sharedMaterials;
            selfRecoloredMaterialsTarget = targetFuncScript.selfRecoloredMaterials;
            selfRecoloredMaterialsGlowTarget = targetFuncScript.selfRecoloredMaterialsGlow;
            if (selfRecoloredMaterialsTarget >= 0 && (selfRecoloredMaterialsTarget <= originalMaterialsTarget.Length))
            {
                originalMaterialsTarget[selfRecoloredMaterialsTarget] = newMaterial;
            }
            if (selfRecoloredMaterialsGlowTarget >= 0 && (selfRecoloredMaterialsGlowTarget <= originalMaterialsTarget.Length))
            {
                originalMaterialsTarget[selfRecoloredMaterialsTarget] = newGlowMaterial;
            }

            meshRendererTarget.sharedMaterials = originalMaterialsTarget;
        }
    }
}
