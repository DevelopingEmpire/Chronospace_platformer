using System.Collections.Generic;
using UnityEngine;

namespace WNC.ITC
{
    [CreateAssetMenu(fileName = "[ITC] Generator Preset", menuName = "Wand and Circles/Isometric Tiles Creator/New [Generator Preset]", order = 0)]
    public class GeneratorPreset : ScriptableObject
    {
        //SIZE
        public bool size_WidthRandom = false;
        public int size_Width = 0;
        public int size_Width_Min = 0;
        public int size_Width_Max = 0;
        public bool size_LengthRandom = false;
        public int size_Length = 0;
        public int size_Length_Min = 0;
        public int size_Length_Max = 0;

        //HEIGHTS
        public bool height_HeightRandom = false;
        public float height_Height = 15f;
        public float height_Height_Min = 0f;
        public float height_Height_Max = 30f;
        public bool height_ContrastRandom;
        public float height_Contrast = 5f;
        public float height_Contrast_Min = 0f;
        public float height_Contrast_Max = 10f;

        //FILLING
        public GameObject[] filling_pois;
        public float filling_poisChance = 0f;
        public float filling_roadsChance = 0f;
        public float filling_roadsFilling = 0f;
        public float filling_roadsFenceChance = 0f;

        //MIXING: 0 - naturally, 1 - zones, 2 - solid
        public int mixingType = 2;

        public List<string> biomes;
        public float biome_water_seaLevel = 0.25f;
        public float biome_sands_thickness = 0.25f;
        public float biome_sands_smoothness = 0f;
        public float biome_snow_snowLevel = 0.25f;

        public bool zones_Random;
        public int zones = 5;
        public int zones_Min = 5;
        public int zones_Max = 10;
        public int zones_biomes;
        public string zones_water;
        public float zones_water_level = 0.25f;

        public string solid_biome;
        public string solid_water;
        public float solid_water_level = 0.25f;

        /// <summary>
        /// Array of biomes which be used for zones mixing represented as int value, this function interprets this into a string list and returns it
        /// </summary>
        public List<string> GetZonesBiomes()
        {
            List<string> zonesBiomes = new List<string>();

            List<Biome> normalBiomes = WNC.ITC.Settings.Instance.GetBiomes(true, false);
            int curValue = zones_biomes;
            for (int i = normalBiomes.Count - 1; i >= 0; i--)
            {
                if (curValue == -1)
                {
                    zonesBiomes.Add(normalBiomes[i].setName);
                }
                else
                {
                    int sizeSetValue = 1;
                    for (int s = 0; s < i; s++)
                    {
                        sizeSetValue *= 2;
                    }

                    if (curValue >= sizeSetValue)
                    {
                        zonesBiomes.Add(normalBiomes[i].setName);
                        curValue -= sizeSetValue;
                    }
                }
            }

            return zonesBiomes;
        }
    }
}