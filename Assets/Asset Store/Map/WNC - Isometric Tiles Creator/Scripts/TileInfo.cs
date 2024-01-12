using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WNC.ITC
{
    public class TileInfo
    {
        public Vector2Int tileCoordinates;
        public Vector3 pos;
        public string biome = "None";
        public int distanceToWater = -1;
        public bool blocked = false;
        public bool connector = false;
        public bool road = false;
        public TileInfo(Vector2Int tileCoordinates, Vector3 pos, string biome)
        {
            this.tileCoordinates = tileCoordinates;
            this.pos = pos;
            this.biome = biome;
        }
    }
}