using System.Collections.Generic;
using UnityEngine;

namespace WNC.ITC
{
    [CreateAssetMenu(fileName = "[ITC] Biome", menuName = "Wand and Circles/Isometric Tiles Creator/New [Biome]", order = 1)]
    public class Biome : ScriptableObject
    {
        public string setName;

        public bool isWaterBiome;

        public List<GameObject> topParts, downParts, decorParts, trees, decors, uniques, pois;

        public WNC.ITC.RoadPreset roadPreset;

        public float filling;

        public float poisChance;

        public float treesChance;
        public Vector3 treesMinScale, treesMaxScale;

        public float decorsChance;
        public Vector3 decorMinScale, decorMaxScale;

        public float uniquesChance;
        public Vector3 uniquesMinScale, uniquesMaxScale;
    }
}