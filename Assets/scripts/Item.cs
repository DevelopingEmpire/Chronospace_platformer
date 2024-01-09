using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Gravity, TimeStop, WindKey }; // Áß·Â, ½Ã°£, ÅÂ¿± 
    public Type type;
    public int value; 

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime); // ºù±Ûºù±Û È¸Àü È¿°ú
    }
}
