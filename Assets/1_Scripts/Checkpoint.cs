using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Checkpoint : MonoBehaviour
{
    public Vector3 checkpointLocation;
    // Start is called before the first frame update
    void Start()
    {
        checkpointLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            Debug.Log("체크포인트 오브젝트에 도달했음");
        }
    }
}
