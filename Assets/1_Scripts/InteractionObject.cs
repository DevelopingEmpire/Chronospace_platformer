using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening; // dot ween 사용 

public class InteractionObject : MonoBehaviour
{
    [Header("Interaction Target")]
    public GameObject[] targetObjects;

    [Header("Target Transition")]
    public float duration = 1.0f;
    public Vector3[] transitionLoc;
    public bool transitionRotToggle;
    public Vector3[] transitionRot;
    public bool transitionScaleToggle;
    public Vector3[] transitionScale;
    private Vector3[] originalLoc;
    private Vector3[] originalRot;
    private Vector3[] originalScale;

    [Header("Material Changes")]
    public GameObject[] selfMesh;
    public GameObject selfMeshLight;
    public int[] selfRecoloredMaterials;
    public int[] selfRecoloredMaterialsGlow;
    //other vars are granted from switch

    void Start()
    {
        // Initialize the arrays
        originalLoc = new Vector3[targetObjects.Length];
        originalRot = new Vector3[targetObjects.Length];
        originalScale = new Vector3[targetObjects.Length];

        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
            {
                originalLoc[i] = targetObjects[i].transform.localPosition;
                originalRot[i] = targetObjects[i].transform.localRotation.eulerAngles;
                originalScale[i] = targetObjects[i].transform.localScale;
            }
        }
    }

    // Update is called once per frame

    public void Activate()
    {
        //Debug.Log("Activate");
        for (int i = 0; i < targetObjects.Length; i++)
        {
            targetObjects[i].transform.DOLocalMove(originalLoc[i] + transitionLoc[i], duration);
            targetObjects[i].transform.DOLocalRotate(originalRot[i] + transitionRot[i], duration);
            targetObjects[i].transform.DOScale(transitionScale[i], duration);
        }
    }

    public void Disactivate()
    {
        for (int i = 0; i < targetObjects.Length; i++)
        {
            targetObjects[i].transform.DOLocalMove(originalLoc[i], duration);
            targetObjects[i].transform.DOLocalRotate(originalRot[i], duration);
            targetObjects[i].transform.DOScale(originalScale[i], duration);
        }
    }
}
