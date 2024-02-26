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

    void Start()
    {
        activated = false;
        if (targetObject != null)
        {
            targetFuncScript = targetObject.GetComponent<InteractionObject>();
        }
    }

    public void Activate()
    {
        if (!activated) 
        {
            if (targetFuncScript != null)
            {
                targetFuncScript.SetActive();
                activated = true;
            }
        }
        else
        {
            if (targetFuncScript != null)
            {
                targetFuncScript.SetDisactive();
                activated = false;
            }
        }

        //Input.GetKeyDown("a") == true
    }
}
