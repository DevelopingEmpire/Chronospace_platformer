using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Gravity, TimeStop, WindKey }; // �߷�, �ð�, �¿� 
    public Type type;
    public int value; 

}
