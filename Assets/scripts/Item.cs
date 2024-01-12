using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Gravity, TimeStop, WindKey }; // 중력, 시간, 태엽 
    public Type type;
    public int value; 

}
