using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public float rotationSpeed = 5.0f;
    public GameObject camTargetObj;
    float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseY = Input.GetAxis("Mouse Y");
        transform.position = camTargetObj.transform.position;
        transform.Rotate((Vector3.left * mouseY * rotationSpeed));
    }
}
