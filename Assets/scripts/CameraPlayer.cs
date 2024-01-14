using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public float rotationSpeed = 5.0f;
    public Vector3 camOffset;
    public GameObject camTargetObj;
    float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = camTargetObj.transform.position + camOffset;
    }

    // Update is called once per frame
    void Update()
    {
        mouseY = Input.GetAxis("Mouse Y");
//        transform.position = camTargetObj.transform.position + camOffset;
        transform.Rotate((Vector3.left * mouseY * rotationSpeed));
    }
}
