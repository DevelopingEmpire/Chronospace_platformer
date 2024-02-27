using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [Header("Interaction Target")]
    public GameObject[] targetObject;

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
    public GameObject selfMesh;
    public int selfRecoloredMaterials;
    public int selfRecoloredMaterialsGlow;
    //other vars are granted from switch

    void Start()
    {
        // Initialize the arrays
        originalLoc = new Vector3[targetObject.Length];
        originalRot = new Vector3[targetObject.Length];
        originalScale = new Vector3[targetObject.Length];

        for (int i = 0; i < targetObject.Length; i++)
        {
            if (targetObject[i] != null)
            {
                originalLoc[i] = targetObject[i].transform.localPosition;
                originalRot[i] = targetObject[i].transform.localRotation.eulerAngles;
                originalScale[i] = targetObject[i].transform.localScale;
            }
        }
    }

    // Update is called once per frame
    private IEnumerator SetActiveCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            for (int i = 0; i < targetObject.Length; i++)
            {
                if (targetObject[i] != null)
                {
                    targetObject[i].transform.localPosition = Vector3.Lerp(targetObject[i].transform.localPosition, originalLoc[i] + transitionLoc[i], elapsedTime / duration);

                    if (transitionRotToggle)
                    {
                        targetObject[i].transform.localRotation = Quaternion.Slerp(targetObject[i].transform.localRotation, Quaternion.Euler(originalRot[i] + transitionRot[i]), elapsedTime / duration);
                    }

                    if (transitionScaleToggle)
                    {
                        targetObject[i].transform.localScale = Vector3.Lerp(targetObject[i].transform.localScale, transitionScale[i], elapsedTime / duration);
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final values are set
        for (int i = 0; i < targetObject.Length; i++)
        {
            if (targetObject[i] != null)
            {
                targetObject[i].transform.localPosition = originalLoc[i] + transitionLoc[i];

                if (transitionRotToggle)
                {
                    targetObject[i].transform.localRotation = Quaternion.Euler(originalRot[i] + transitionRot[i]);
                }

                if (transitionScaleToggle)
                {
                    targetObject[i].transform.localScale = transitionScale[i];
                }
            }
        }
    }

    public void SetActive()
    {
        StartCoroutine(SetActiveCoroutine());
    }

    private IEnumerator SetDisactiveCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            for (int i = 0; i < targetObject.Length; i++)
            {
                if (targetObject[i] != null)
                {
                    targetObject[i].transform.localPosition = Vector3.Lerp(targetObject[i].transform.localPosition, originalLoc[i], elapsedTime / duration);

                    if (transitionRotToggle)
                    {
                        targetObject[i].transform.localRotation = Quaternion.Slerp(targetObject[i].transform.localRotation, Quaternion.Euler(originalRot[i]), elapsedTime / duration);
                    }

                    if (transitionScaleToggle)
                    {
                        targetObject[i].transform.localScale = Vector3.Lerp(targetObject[i].transform.localScale, originalScale[i], elapsedTime / duration);
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final values are set
        for (int i = 0; i < targetObject.Length; i++)
        {
            if (targetObject[i] != null)
            {
                targetObject[i].transform.localPosition = originalLoc[i];

                if (transitionRotToggle)
                {
                    targetObject[i].transform.localRotation = Quaternion.Euler(originalRot[i]);
                }

                if (transitionScaleToggle)
                {
                    targetObject[i].transform.localScale = originalScale[i];
                }
            }
        }
    }

    public void SetDisactive()
    {
        StartCoroutine(SetDisactiveCoroutine());
    }
}
