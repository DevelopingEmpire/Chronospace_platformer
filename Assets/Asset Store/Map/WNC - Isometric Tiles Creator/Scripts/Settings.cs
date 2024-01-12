using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WNC.ITC
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Wand and Circles/Isometric Tiles Creator/New [Settings]", order = 2)]
    public class Settings : ScriptableObject
    {
        private static WNC.ITC.Settings instance;
        public static WNC.ITC.Settings Instance
        {
            get
            {
                if(instance != null)
                {
                    return instance;
                }
                else
                {
                    instance = Resources.Load<WNC.ITC.Settings>("WNC_ITC/Settings/Settings");
                    return instance;
                }
            }
        }

        public List<WNC.ITC.Biome> biomes;
        public GameObject scalableWaterTile;
        public bool scalableWater = false;
        public float tileSize = 2f;
        public bool alignBySealevel;
        public int minMapSize, maxMapSize;
        public bool raiseTilesHeight = false;
        public bool generatorTabInPreset = false;

        /// <summary>
        /// Returns Biome component from  biomes list of this setting file by biome name
        /// </summary>
        public WNC.ITC.Biome GetBiomeByName(string biome)
        {
            return biomes.Single(x => x.setName == biome);
        }
        /// <summary>
        /// Returns list of biomes from biomes list of this setting file
        /// </summary>
        public List<WNC.ITC.Biome> GetBiomes(bool normalBiomes, bool waterBiomes)
        {
            if (normalBiomes && waterBiomes) return biomes;
            else if (normalBiomes) return biomes.Where(x => !x.isWaterBiome).ToList();
            else return biomes.Where(x => x.isWaterBiome).ToList();
        }
    }
}