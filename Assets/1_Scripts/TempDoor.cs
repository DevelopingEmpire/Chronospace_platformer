using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDoor : InteractionObject
{
    //[Header("Interaction Target")]
    //public GameObject[] targetObjects;
    public int buttonID; // 인스펙터에서 정해주기  

    public void Activate()
    {
        //Debug.Log("Activate");
        transform.Translate(Vector3.up * Time.deltaTime);
    }

    public void Disactivate()
    {
        transform.Translate(Vector3.down * Time.deltaTime);
    }
}
