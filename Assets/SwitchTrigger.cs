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
    InteractionObject targetFuncScript;
    bool activated;

    [Header("Timer")]
    public bool hasTimer = false;
    public float timerValue = 0f;

    [Header("Material Changes")]
    public Color selfColor;
    public GameObject selfMesh;
    public GameObject selfMeshLight;
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
    private GameObject selfMeshTarget;
    private GameObject selfMeshLightTarget;
    private int selfRecoloredMaterialsTarget;
    private int selfRecoloredMaterialsGlowTarget;

    void Start()
    {
        activated = false;
        if (targetObjects != null)
        {
            targetFuncScript = targetObjects[0].GetComponent<InteractionObject>();
        }
        RecolorMaterials();
        foreach (GameObject targetObject in targetObjects)
        {
            if (targetObject != null)
            {
                RecolorTargetObject(targetObject);
            }
        }
    }

    public void Activate()
    {
        if (!activated)
        {
            if (targetObjects != null && targetObjects.Length > 0)
            {
                foreach (GameObject targetObject in targetObjects)
                {
                    targetFuncScript = targetObject.GetComponent<InteractionObject>();
                    if (targetObject != null)
                    {
                        // Update the materials for each target object
                        Renderer targetRenderer = targetObject.GetComponent<Renderer>();
                        if (targetRenderer != null)
                        {
                            Material[] originalMaterials = targetRenderer.sharedMaterials;
                            // Make changes to originalMaterials using newGlowMaterial
                            targetRenderer.sharedMaterials = originalMaterials;
                        }
                    }
                    targetFuncScript.Activate();
                }
                // Other actions when activated
                activated = true;
            }
        }
        else
        {
            if (targetObjects != null && targetObjects.Length > 0)
            {
                foreach (GameObject targetObject in targetObjects)
                {
                    targetFuncScript = targetObject.GetComponent<InteractionObject>();
                    if (targetObject != null)
                    {
                        // Update the materials for each target object
                        Renderer targetRenderer = targetObject.GetComponent<Renderer>();
                        if (targetRenderer != null)
                        {
                            Material[] originalMaterials = targetRenderer.sharedMaterials;
                            // Make changes to originalMaterials using newMaterial
                            targetRenderer.sharedMaterials = originalMaterials;
                        }
                    }
                    targetFuncScript.Disactivate();
                }
                // Other actions when deactivated
                activated = false;
            }
        }
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
        selfMeshLight.Activate(false);
    }

    void RecolorTargetObject(GameObject targetObjectEach)
    {
        selfMeshTarget = targetObjectEach;
        selfMeshTarget = (targetFuncScript.selfMesh);
        selfMeshLightTarget = (targetFuncScript.selfMeshLight);

        meshRendererTarget = selfMeshTarget.GetComponent<MeshRenderer>();
        meshRendererLightTarget = selfMeshLightTarget.GetComponent<Light>();
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
                originalMaterialsTarget[selfRecoloredMaterialsGlowTarget] = newMaterial;
            }
            meshRendererTarget.sharedMaterials = originalMaterialsTarget;
        }
        if (meshRendererLightTarget != null)
        {
            meshRendererLightTarget.color = selfColor;
            selfMeshLightTarget.Activate(false);
        }

    }
}
