using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razer : MonoBehaviour
{
    RaycastHit hit;
    public LineRenderer lineRenderer; // 얘가 선을 그어줄거야! 

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position); // 초기 위치 

        if(Physics.Raycast(transform.position,transform.forward, out hit))
        {
            lineRenderer.SetPosition(1, hit.point); // ray 맞은 정점까지 쏴  
        }
    }
}
