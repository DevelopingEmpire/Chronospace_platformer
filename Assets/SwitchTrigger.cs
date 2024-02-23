using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    [Header("Interaction Object")]
    public GameObject targetObject;
    public string targetObjectElement;
    public float[] targetObjectMode;

    [Header("Timer")]
    public bool hasTimer = false;
    public float timerValue = 0f;

    void Start()
    {
        //targetObject.GetComponent
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
