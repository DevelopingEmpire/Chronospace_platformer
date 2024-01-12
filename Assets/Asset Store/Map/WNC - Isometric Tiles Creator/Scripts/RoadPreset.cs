using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WNC.ITC
{
    [CreateAssetMenu(fileName = "[ITC] Road Preset", menuName = "Wand and Circles/Isometric Tiles Creator/New [Road Preset]", order = 2)]
    public class RoadPreset : ScriptableObject
    {
        public GameObject[] straight, turn, ending, tripleCrossroad, crossroad, fences, ladders, verticalLadders, bridges;
    }
}