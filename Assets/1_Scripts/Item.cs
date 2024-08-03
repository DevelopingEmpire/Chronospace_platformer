using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Gravity, TimeStop, Magneticgrav, Shield, WindKey, Null }; // 중력, 시간, 태엽 
    public Type type;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime); // 빙글빙글 회전 효과
    }

}
