using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace WNC.ITC
{
    public class IsometricTilesCreator : MonoBehaviour
    {
        #region Functions for creating new components
        /// <summary>
        /// Creates new GameObject on scene with Generator component and returns it.
        /// </summary>
        public static Generator CreateGenerator()
        {
            GameObject newGenerator = new GameObject();
            newGenerator.transform.position = Vector3.zero;
            newGenerator.AddComponent(typeof(WNC.ITC.Generator));
            newGenerator.name = "[ITC] Generator";
#if UNITY_EDITOR
            UnityEditor.Selection.activeObject = newGenerator;
#endif
            return newGenerator.GetComponent<WNC.ITC.Generator>();
        }
        /// <summary>
        /// Creates new GameObject on scene with POI component and returns it.
        /// </summary>
        public static POI CreatePOI()
        {
            GameObject newPOI = new GameObject();
            newPOI.transform.position = Vector3.zero;
            newPOI.AddComponent(typeof(WNC.ITC.POI));
            newPOI.name = "[ITC] POI";
#if UNITY_EDITOR
            UnityEditor.Selection.activeObject = newPOI;
#endif
            return newPOI.GetComponent<WNC.ITC.POI>();
        }
        /// <summary>
        /// Creates new GeneratorPreset in project.
        /// </summary>
        public static GeneratorPreset CreateGeneratorPreset(string presetName)
        {
#if UNITY_EDITOR
            List<string> setNames = new List<string>();
            List<string> waterSetNames = new List<string>();
            for (int i = 0; i < WNC.ITC.Settings.Instance.biomes.Count; i++)
            {
                if (!WNC.ITC.Settings.Instance.biomes[i].isWaterBiome) setNames.Add(WNC.ITC.Settings.Instance.biomes[i].setName);
                else waterSetNames.Add(WNC.ITC.Settings.Instance.biomes[i].setName);
            }

            GeneratorPreset newGeneratorPreset = ScriptableObject.CreateInstance<WNC.ITC.GeneratorPreset>();
            newGeneratorPreset.biomes = new List<string>() { setNames[0], waterSetNames[0], setNames[0], setNames[0] };
            newGeneratorPreset.solid_biome = setNames[0];
            newGeneratorPreset.solid_water = waterSetNames[0];
            newGeneratorPreset.zones = -1;
            newGeneratorPreset.zones_water = waterSetNames[0];
            newGeneratorPreset.size_Length = WNC.ITC.Settings.Instance.minMapSize;
            newGeneratorPreset.size_Width = WNC.ITC.Settings.Instance.minMapSize;
            newGeneratorPreset.size_Length_Min = WNC.ITC.Settings.Instance.minMapSize;
            newGeneratorPreset.size_Width_Min = WNC.ITC.Settings.Instance.minMapSize;
            newGeneratorPreset.size_Length_Max = WNC.ITC.Settings.Instance.maxMapSize;
            newGeneratorPreset.size_Width_Max = WNC.ITC.Settings.Instance.maxMapSize;

            UnityEditor.AssetDatabase.CreateAsset(newGeneratorPreset, "Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Generator Presets/[ITC] " + presetName + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.EditorUtility.FocusProjectWindow();
            UnityEditor.Selection.activeObject = newGeneratorPreset;
            return newGeneratorPreset;
#else
            return null;
#endif
        }
        /// <summary>
        /// Creates new Biome in project.
        /// </summary>
        public static Biome CreateBiome(string biomeName)
        {
#if UNITY_EDITOR
            Biome newBiome = ScriptableObject.CreateInstance<WNC.ITC.Biome>();
            newBiome.setName = biomeName;
            newBiome.treesMinScale = Vector3.one;
            newBiome.treesMaxScale = Vector3.one;
            newBiome.decorMinScale = Vector3.one;
            newBiome.decorMaxScale = Vector3.one;
            newBiome.uniquesMinScale = Vector3.one;
            newBiome.uniquesMaxScale = Vector3.one;
            UnityEditor.AssetDatabase.CreateAsset(newBiome, "Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Biomes/[Biome] " + biomeName + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.EditorUtility.FocusProjectWindow();
            UnityEditor.Selection.activeObject = newBiome;
            return newBiome;
#else
            return null;
#endif
        }
        /// <summary>
        /// Creates new RoadPreset in project.
        /// </summary>
        public static RoadPreset CreateRoadPreset(string presetName)
        {
#if UNITY_EDITOR
            RoadPreset newRoadPreset = ScriptableObject.CreateInstance<WNC.ITC.RoadPreset>();
            UnityEditor.AssetDatabase.CreateAsset(newRoadPreset, "Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Road Presets/[Road] " + presetName + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.EditorUtility.FocusProjectWindow();
            UnityEditor.Selection.activeObject = newRoadPreset;
            return newRoadPreset;
#else
            return null;
#endif
        }
#endregion

        /// <summary>
        /// Returns GameObject of generated map with Generator component.
        /// </summary>
        /// <param name="offset">Noise offset for generation, if you giving null generator will randomize offset.</param>
        public static GameObject GenerateMap(GeneratorPreset preset, Generator generator = null, Vector2Int? offset = null)
        {
            if (generator == null)
            {
                generator = CreateGenerator();
                generator.generatorPreset = preset;
            }

            generator.Clear();

            // 1. Creating tilemap[,] with sizes of the map
            int size_X = preset.size_WidthRandom ? (Random.Range(preset.size_Width_Min, preset.size_Width_Max)) : preset.size_Width;
            int size_Z = preset.size_LengthRandom ? (Random.Range(preset.size_Length_Min, preset.size_Length_Max)) : preset.size_Length;
            TileInfo[,] tilemap = new TileInfo[size_X, size_Z];

            // 2. Creating heights for tilemap with noise
            float contrast = preset.height_ContrastRandom ? (Random.Range(preset.height_Contrast_Min, preset.height_Contrast_Max)) : preset.height_Contrast;
            float maxHeight = preset.height_HeightRandom ? (Random.Range(preset.height_Height_Min, preset.height_Height_Max)) : preset.height_Height;
            tilemap = NoiseForTileMap(tilemap, generator, contrast, maxHeight, offset);

            // 3. Get biomes based on the selected mixing type in the preset
            List<string> biomes = new List<string>();
            float seaLevel = 0.1f;
            string waterBiome = "";
            if (preset.mixingType == 0)
            {
                // 3-1. Naturally mixing type, we adding to biomes list all selected in preset biomes, excluding water.
                //      Also, take values seaLevel and waterBiome from preset.
                biomes.Add(preset.biomes[0]);
                biomes.Add(preset.biomes[2]);
                biomes.Add(preset.biomes[3]);
                tilemap = SetBiome(tilemap, biomes[0]);

                seaLevel = preset.biome_water_seaLevel;
                waterBiome = preset.biomes[1];
            }
            else if (preset.mixingType == 1)
            {
                // 3-2. Zones mixing type, we add to the biomes all selected for this type of mixing biomes.
                //      After that we spliting our tilemap via number of zones to selected biomes.
                //      Also, take values seaLevel and waterBiome from preset.
                biomes.AddRange(preset.GetZonesBiomes());
                int zonesCount = preset.zones_Random ? (Random.Range(preset.zones_Min, preset.zones_Max)) : preset.zones;
                tilemap = SplitInZones(tilemap, 10, biomes.ToArray());

                seaLevel = preset.zones_water_level;
                waterBiome = preset.zones_water;
            }
            else if (preset.mixingType == 2)
            {
                // 3-3. Solid mixing type, in this case we set to all tiles in our tilemap one biome.
                //      Also, take values seaLevel and waterBiome from preset.
                biomes.Add(preset.solid_biome);
                tilemap = SetBiome(tilemap, biomes[0]);

                seaLevel = preset.solid_water_level;
                waterBiome = preset.solid_water;
            }

            // 4. Insert water biome to our tilemap.
            //    Adding our water biome to biomes list to work with it in next steps.
            biomes.Add(waterBiome);
            tilemap = InsertWater(tilemap, seaLevel, maxHeight, waterBiome);

            if (preset.mixingType == 0)
            {
                // 4-1. For mixing type, we adding biome based on water, so after step [4], where we insert water to tilemap,
                //      we adding biome around water. Also, adding biome on heights.
                tilemap = InsertBiomeAroundWater(tilemap, biomes[1], preset.biome_sands_thickness, preset.biome_sands_smoothness);
                tilemap = InsertBiomeOnHeights(tilemap, biomes[2], preset.biome_snow_snowLevel, maxHeight);
            }

            // 5. Creating empty GameObjects for generator, to structurize in them spawned objects.
            CreateChildsStructure(generator, biomes.ToArray());

            // 6. Spawning tiles.
            SpawnTiles(tilemap, generator);

            // 7. Spawning global POIs.
            if (preset.filling_pois.Length > 0) tilemap = SpawnPOIs(tilemap, generator, preset.filling_pois, preset.filling_poisChance);

            // 8. Spawning POIs for biomes.
            for (int i = 0; i < biomes.Count; i++)
            {
                Biome biome = WNC.ITC.Settings.Instance.GetBiomeByName(biomes[i]);
                tilemap = SpawnPOIs(tilemap, generator, biome.pois.ToArray(), biome.poisChance, biome.setName);
            }

            if (preset.filling_roadsChance > 0f)
            {
                // 10. Creating roads between POI connectors inside tilemap array.
                tilemap = CreateRoadsBetweenPOI(tilemap, generator, preset.filling_roadsChance);

                // 11. Spawning roads. Roads means a route between points, which consists of road tiles, stairs, fences, bridges.
                SpawnRoads(tilemap, generator, preset.filling_roadsFilling, preset.filling_roadsFenceChance);
            }

            for (int i = 0; i < biomes.Count; i++)
            {
                // 12. Spawning filling objects (from filling lists in biomes) for every biome used in preset.
                tilemap = SpawnFilling(tilemap, generator, biomes[i]);
            }


            // 12. Spawning filling objects (from filling lists in biomes) for every biome used in preset.
            generator.generatorPreset = preset;
            generator.tilemap = tilemap;
            generator.mapGenerated = true;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(generator);
#endif

            return generator.gameObject;
        }
        /// <summary>
        /// Returns a TilePos[,] with genereted heights by perlin noise with random offset
        /// </summary>
        /// <param name="tilemap">Double dimension array by map sizes.</param>
        /// <param name="contrast">Landscape contrast.</param>
        public static TileInfo[,] NoiseForTileMap(TileInfo[,] tilemap, WNC.ITC.Generator generator, float contrast, float maxHeight, Vector2Int? offset = null)
        {
            Vector2Int noiseOffset = new Vector2Int();

            if (offset == null)
            {
                noiseOffset.x = Random.Range(0, 9999999);
                noiseOffset.y = Random.Range(0, 9999999);
            }
            else
            {
                noiseOffset.x = ((Vector2Int)offset).x;
                noiseOffset.y = ((Vector2Int)offset).y;
            }

            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int z = 0; z < tilemap.GetLength(1); z++)
                {
                    Vector3 pos = Vector3.zero;
                    pos.x = x * WNC.ITC.Settings.Instance.tileSize;
                    pos.z = z * WNC.ITC.Settings.Instance.tileSize;
                    pos.y = Mathf.PerlinNoise(((x + ((Vector2Int)noiseOffset).x) / 100f) * contrast, ((z + ((Vector2Int)noiseOffset).y) / 100f) * contrast);

                    tilemap[x, z] = new TileInfo(new Vector2Int(x, z), pos, "Standard");
                }
            }

            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int z = 0; z < tilemap.GetLength(1); z++)
                {
                    float tilePerlin = tilemap[x, z].pos.y;
                    float tileHeight = Mathf.FloorToInt(tilePerlin / (1f / maxHeight)) * WNC.ITC.Settings.Instance.tileSize;

                    if (z > 0)
                    {
                        if (tilemap[x, z - 1].pos.y - tileHeight > WNC.ITC.Settings.Instance.tileSize + 0.1f)
                        {
                            tileHeight = tilemap[x, z - 1].pos.y - WNC.ITC.Settings.Instance.tileSize;
                        }
                        else if (tilemap[x, z - 1].pos.y - tileHeight < -(WNC.ITC.Settings.Instance.tileSize + 0.1f))
                        {
                            tileHeight = tilemap[x, z - 1].pos.y + WNC.ITC.Settings.Instance.tileSize;
                        }
                        if (x > 0)
                        {
                            if (tilemap[x - 1, z].pos.y - tileHeight > WNC.ITC.Settings.Instance.tileSize + 0.1f)
                            {
                                tileHeight = tilemap[x - 1, z].pos.y - WNC.ITC.Settings.Instance.tileSize;
                            }
                            else if (tilemap[x - 1, z].pos.y - tileHeight < -(WNC.ITC.Settings.Instance.tileSize + 0.1f))
                            {
                                tileHeight = tilemap[x - 1, z].pos.y + WNC.ITC.Settings.Instance.tileSize;
                            }
                        }
                    }
                    else if (x > 0)
                    {
                        if (tilemap[x - 1, z].pos.y - tileHeight > WNC.ITC.Settings.Instance.tileSize + 0.1f)
                        {
                            tileHeight = tilemap[x - 1, z].pos.y - WNC.ITC.Settings.Instance.tileSize;
                        }
                        else if (tilemap[x - 1, z].pos.y - tileHeight < -(WNC.ITC.Settings.Instance.tileSize + 0.1f))
                        {
                            tileHeight = tilemap[x - 1, z].pos.y + WNC.ITC.Settings.Instance.tileSize;
                        }
                    }

                    tilemap[x, z].pos.y = tileHeight;
                }
            }

            generator.mapOffset = new Vector2Int(noiseOffset.x, noiseOffset.y);
            generator.mapSize = new Vector2Int(tilemap.GetLength(0), tilemap.GetLength(1));

            return tilemap;
        }

        /// <summary>
        /// Set one biome for all tiles from tilemap.
        /// </summary>
        /// <param name="overrideOtherBiomes">If tilemap contains tiles with assigned biome - function will override this biome.</param>
        public static TileInfo[,] SetBiome(TileInfo[,] tilemap, string biome, bool overrideOtherBiomes = true)
        {
            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int z = 0; z < tilemap.GetLength(1); z++)
                {
                    if (overrideOtherBiomes || tilemap[x, z].biome == "None")
                    {
                        tilemap[x, z].biome = biome;
                    }
                }
            }

            return tilemap;
        }

        /// <summary>
        /// Insert a water biome in the tilemap at a given height.
        /// </summary>
        /// <param name="maxHeight">Max height represented from 0 to 1.</param>
        public static TileInfo[,] InsertWater(TileInfo[,] tilemap, float seaLevel, float maxHeight, string biome = "None")
        {
            seaLevel = Mathf.FloorToInt(Mathf.Lerp(0, 1, seaLevel) / (1f / maxHeight)) * 2;

            List<TileInfo> сheckingTiles = new List<TileInfo>();
            List<TileInfo> checkingQueue = new List<TileInfo>();
            List<TileInfo> checkedTiles = new List<TileInfo>();

            //Checking tiles which lower then seaLevel
            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int z = 0; z < tilemap.GetLength(1); z++)
                {
                    if (tilemap[x, z].pos.y <= seaLevel && seaLevel > 0)
                    {
                        tilemap[x, z].distanceToWater = 0;
                        tilemap[x, z].pos.y = seaLevel;

                        if (Settings.Instance.scalableWater)
                        {
                            tilemap[x, z].biome = "None";
                        }
                        else
                        {
                            tilemap[x, z].biome = biome;
                        }

                        сheckingTiles.Add(tilemap[x, z]);
                        checkedTiles.Add(tilemap[x, z]);
                    }
                }
            }

            //Set distance to water for other tiles
            while (true)
            {
                for (int i = 0; i < сheckingTiles.Count; i++)
                {
                    List<TileInfo> neighbors = new List<TileInfo>();
                    if (сheckingTiles[i].tileCoordinates.x - 1 >= 0)
                    {
                        neighbors.Add(tilemap[сheckingTiles[i].tileCoordinates.x - 1, сheckingTiles[i].tileCoordinates.y]);
                    }
                    if (сheckingTiles[i].tileCoordinates.y - 1 >= 0)
                    {
                        neighbors.Add(tilemap[сheckingTiles[i].tileCoordinates.x, сheckingTiles[i].tileCoordinates.y - 1]);
                    }
                    if (сheckingTiles[i].tileCoordinates.y + 1 < tilemap.GetLength(1))
                    {
                        neighbors.Add(tilemap[сheckingTiles[i].tileCoordinates.x, сheckingTiles[i].tileCoordinates.y + 1]);
                    }
                    if (сheckingTiles[i].tileCoordinates.x + 1 < tilemap.GetLength(0))
                    {
                        neighbors.Add(tilemap[сheckingTiles[i].tileCoordinates.x + 1, сheckingTiles[i].tileCoordinates.y]);
                    }
                    foreach (TileInfo neighbor in neighbors)
                    {
                        if (!checkedTiles.Contains(neighbor))
                        {
                            if (neighbor.distanceToWater > сheckingTiles[i].distanceToWater + 1 || neighbor.distanceToWater == -1)
                            {
                                neighbor.distanceToWater = сheckingTiles[i].distanceToWater + 1;
                            }
                            checkedTiles.Add(neighbor);
                            checkingQueue.Add(neighbor);
                        }
                    }
                }
                сheckingTiles = checkingQueue;
                checkingQueue = new List<TileInfo>();
                if (сheckingTiles.Count == 0) break;
            }

            return tilemap;
        }

        /// <summary>
        /// Insert a biome around the water biome to the tilemap, you can specify the thickness around the water and the smoothness of the transition with bordering normal biomes.
        /// </summary>
        /// <param name="thickness">Thickness of this biome around water biome.</param> 
        /// <param name="smoothness">Smoothness of the transition with bordering normal biomes.</param> 
        public static TileInfo[,] InsertBiomeAroundWater(TileInfo[,] tilemap, string biome, float thickness, float smoothness)
        {
            int sandsSize = (int)(((float)(20f)) * thickness);
            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int z = 0; z < tilemap.GetLength(1); z++)
                {
                    if (tilemap[x, z].distanceToWater > 0 && tilemap[x, z].distanceToWater < sandsSize)
                    {
                        float chance = 100f;
                        if (sandsSize - tilemap[x, z].distanceToWater <= (float)sandsSize * smoothness)
                            chance = (100f / ((float)sandsSize * smoothness)) * (sandsSize - tilemap[x, z].distanceToWater);

                        if (Random.Range(0, 100) < chance)
                            tilemap[x, z].biome = biome;
                    }
                }
            }

            return tilemap;
        }

        /// <summary>
        /// Insert in tilemap a biome located on the tops of the map, embedded at a given height.
        /// </summary>
        /// <param name="maxHeight">Max height represented from 0 to 1.</param> 
        public static TileInfo[,] InsertBiomeOnHeights(TileInfo[,] tilemap, string biome, float heights, float maxHeight)
        {
            heights = Mathf.FloorToInt(Mathf.Lerp(1, 0, heights) / (1f / maxHeight)) * WNC.ITC.Settings.Instance.tileSize;

            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int z = 0; z < tilemap.GetLength(1); z++)
                {
                    if (tilemap[x, z].pos.y >= heights)
                    {
                        tilemap[x, z].biome = biome;
                    }
                }
            }

            return tilemap;
        }

        /// <summary>
        /// Split the tilemap into the specified number of zones, each of which is assigned a random biome from the given array.
        /// </summary>
        /// <param name="zonesCount">The number of zones into which the map will be divided.</param> 
        /// <param name="biomes">List of biomes to be used.</param> 
        public static TileInfo[,] SplitInZones(TileInfo[,] tilemap, int zonesCount, string[] biomes)
        {
            List<Vector2Int> zones = new List<Vector2Int>();
            List<string> zoneBiomes = new List<string>();

            for (int i = 0; i < zonesCount; i++)
            {
                zones.Add(new Vector2Int(Random.Range(0, tilemap.GetLength(0)), Random.Range(0, tilemap.GetLength(1))));
                zoneBiomes.Add(biomes[Random.Range(0, biomes.Length)]);
            }

            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int z = 0; z < tilemap.GetLength(1); z++)
                {
                    int biomeID = -1;
                    float minDistance = float.PositiveInfinity;

                    for (int i = 0; i < zones.Count; i++)
                    {
                        float distance = Mathf.Sqrt(Mathf.Pow((zones[i].x - x), 2) + Mathf.Pow((zones[i].y - z), 2));
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            biomeID = i;
                        }
                    }

                    tilemap[x, z].biome = zoneBiomes[biomeID];
                }
            }

            return tilemap;
        }

        /// <summary>
        /// Split the tilemap into the specified number of zones, each of which is assigned a random biome from the given array. Alternative method, breaks into zones of rectangular shapes.
        /// </summary>
        /// <param name="zonesCount">The number of zones into which the map will be divided.</param> 
        /// <param name="biomes">List of biomes to be used.</param> 
        public static TileInfo[,] SplitInZonesAlternative(TileInfo[,] tilemap, int zonesCount, string[] biomes)
        {
            List<Vector2Int> zones = new List<Vector2Int>();
            List<string> zoneBiomes = new List<string>();
            List<int> zoneSizes = new List<int>();

            for (int i = 0; i < zonesCount; i++)
            {
                zones.Add(new Vector2Int(Random.Range(0, tilemap.GetLength(0)), Random.Range(0, tilemap.GetLength(1))));
                zoneBiomes.Add(biomes[Random.Range(0, biomes.Length)]);
                float maxZoneSize = ((tilemap.GetLength(0) + tilemap.GetLength(1)) * 2f) / 10f;
                zoneSizes.Add(Random.Range((int)(maxZoneSize / 2f), (int)maxZoneSize));
            }

            for (int i = 0; i < zones.Count; i++)
            {
                for (int x = zones[i].x; x < zones[i].x + zoneSizes[i]; x++)
                {
                    for (int z = zones[i].y; z < zones[i].y + zoneSizes[i]; z++)
                    {
                        if (x < tilemap.GetLength(0) && z < tilemap.GetLength(1))
                        {
                            tilemap[x, z].biome = zoneBiomes[i];
                        }
                    }
                }
            }

            return tilemap;
        }

        /// <summary>
        /// Creates empty GameObjects in child objects of the generator to structure and sort future spawned objects
        /// </summary>
        public static void CreateChildsStructure(WNC.ITC.Generator generator, string[] biomes)
        {
            for (int i = 0; i < biomes.Length; i++)
            {
                Transform newBiomeTransform = new GameObject().transform;
                newBiomeTransform.parent = generator.transform;
                newBiomeTransform.localPosition = Vector3.zero;
                newBiomeTransform.localEulerAngles = Vector3.zero;
                newBiomeTransform.gameObject.name = "[Biome] - " + biomes[i];
                Transform biomeTiles = new GameObject().transform;
                biomeTiles.parent = newBiomeTransform;
                biomeTiles.localPosition = Vector3.zero;
                biomeTiles.localEulerAngles = Vector3.zero;
                biomeTiles.gameObject.name = "[Tiles]";
                Transform biomePois = new GameObject().transform;
                biomePois.parent = newBiomeTransform;
                biomePois.localPosition = Vector3.zero;
                biomePois.localEulerAngles = Vector3.zero;
                biomePois.gameObject.name = "[POI]";
                Transform biomeRoads = new GameObject().transform;
                biomeRoads.parent = newBiomeTransform;
                biomeRoads.localPosition = Vector3.zero;
                biomeRoads.localEulerAngles = Vector3.zero;
                biomeRoads.gameObject.name = "[Roads]";
                Transform biomeTrees = new GameObject().transform;
                biomeTrees.parent = newBiomeTransform;
                biomeTrees.localPosition = Vector3.zero;
                biomeTrees.localEulerAngles = Vector3.zero;
                biomeTrees.gameObject.name = "[Trees]";
                Transform biomeDecors = new GameObject().transform;
                biomeDecors.parent = newBiomeTransform;
                biomeDecors.localPosition = Vector3.zero;
                biomeDecors.localEulerAngles = Vector3.zero;
                biomeDecors.gameObject.name = "[Decors]";
                Transform biomeUniques = new GameObject().transform;
                biomeUniques.parent = newBiomeTransform;
                biomeUniques.localPosition = Vector3.zero;
                biomeUniques.localEulerAngles = Vector3.zero;
                biomeUniques.gameObject.name = "[Uniques]";
            }
        }

        /// <summary>
        /// Spawns all tiles for the tilemap.
        /// </summary>
        public static void SpawnTiles(TileInfo[,] tilemap, WNC.ITC.Generator generator)
        {
            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int z = 0; z < tilemap.GetLength(1); z++)
                {
                    GenerateTile(generator, tilemap[x, z].biome, generator.GetBiomeTransform(tilemap[x, z].biome).GetChild(0), tilemap[x, z].pos);
                }
            }
        }

        /// <summary>
        /// Returns GameObject of spawned tile
        /// </summary>
        /// <param name="biomeName">Name of [Biome] from which tile be generated.</param>
        /// <param name="parent">Parent for tile.</param>
        /// <param name="localPosition">Local position for tile.</param>
        public static GameObject GenerateTile(WNC.ITC.Generator generator, string biomeName, Transform parent, Vector3 localPosition)
        {
            //Get [Biome] by name
            WNC.ITC.Biome biome = WNC.ITC.Settings.Instance.biomes.Single(x => x.setName == biomeName);

            //Creating empty GameObject
            GameObject tile = new GameObject();
            tile.transform.parent = parent;
            tile.transform.localPosition = localPosition;
            tile.transform.localEulerAngles = Vector3.zero;
            tile.name = "Tile " + biomeName;

            //Spawning Down part of tile
            if (biome.downParts != null && biome.downParts.Count > 0)
            {
                GameObject downPart = SpawnGameObject(biome.downParts[Random.Range(0, biome.downParts.Count)], tile.transform);
                downPart.transform.localEulerAngles = new Vector3(0f, 90f * Random.Range(0, 4), 0f);
            }

            //Spawning Top part of tile
            if (biome.topParts != null && biome.topParts.Count > 0)
            {
                GameObject topPart = SpawnGameObject(biome.topParts[Random.Range(0, biome.topParts.Count)], tile.transform);
                topPart.transform.localEulerAngles = new Vector3(0f, 90f * Random.Range(0, 4), 0f);
            }

            //Spawning Decor part of tile
            if (biome.decorParts != null && biome.decorParts.Count > 0)
            {
                GameObject decorPart = SpawnGameObject(biome.decorParts[Random.Range(0, biome.decorParts.Count)], tile.transform);
                decorPart.transform.localEulerAngles = new Vector3(0f, 90f * Random.Range(0, 4), 0f);
            }

            //Raise Tiles Height (You can set this setting in [Settings] component
            if (!biome.isWaterBiome && WNC.ITC.Settings.Instance.raiseTilesHeight)
            {
                int blocksCount = Mathf.FloorToInt(tile.transform.localPosition.y / WNC.ITC.Settings.Instance.tileSize);
                for (int i = 0; i < blocksCount; i++)
                {
                    GameObject downPart = SpawnGameObject(biome.downParts[Random.Range(0, biome.downParts.Count)], tile.transform);
                    downPart.transform.localPosition = new Vector3(0f, -(i + 1) * WNC.ITC.Settings.Instance.tileSize, 0f);
                    downPart.transform.localEulerAngles = new Vector3(0f, 90f * Random.Range(0, 4), 0f);
                }
            }

            if (!biome.isWaterBiome) generator.tiles.Add(tile);
            else generator.waterTiles.Add(tile);

            return tile;
        }

        /// <summary>
        /// Calculates positions and spawns POI from the given array of POI Prefabs, with the specified spawn chance. If the biome is null, then POI will spawn on any biome, except water type biomes.
        /// </summary>
        /// <param name="pois">Array of POI Prafabs, from which poi will spawn randomly.</param> 
        /// <param name="chance">Chance to spawn POI [(free suitable tiles count) * chance * chance].</param> 
        /// <param name="biome">Name of Biome on which POI will spawn. If you pass null to this parameter, the POI will spawn on any biome, except water type biomes.</param> 
        public static TileInfo[,] SpawnPOIs(TileInfo[,] tilemap, WNC.ITC.Generator generator, GameObject[] pois, float chance, string? biome = null)
        {
            List<TileInfo> tilemapList = null;
            if (biome == null) tilemapList = tilemap.Cast<TileInfo>().Where(x => !x.blocked && !x.connector && !x.road).ToList(); //If biome string null we take all tiles, we use it for Global [POI] from Generator Preset
            else tilemapList = tilemap.Cast<TileInfo>().Where(x => x.biome == biome && !x.blocked && !x.connector && !x.road).ToList(); //Otherway we get tiles only of one biome

            int count = (int)((float)tilemapList.Count / Mathf.Lerp(100f, 4f, chance));

            for (int i = 0; i < count; i++)
            {
                int tileID = Random.Range(0, tilemapList.Count);
                int x = tilemapList[tileID].tileCoordinates.x;
                int z = tilemapList[tileID].tileCoordinates.y;

                List<int> checkedPOIs = new List<int>();
                List<int> suitPOI_ID = new List<int>();
                List<int> suitPOI_rotation = new List<int>();

                int xCoord = x;
                int zCoord = z;
                for (int t = 0; t < pois.Length; t++)
                {
                    int poiID = Random.Range(0, pois.Length);
                    while (checkedPOIs.Contains(poiID)) poiID = Random.Range(0, pois.Length);
                    checkedPOIs.Add(poiID);

                    POI poi = pois[poiID].GetComponent<WNC.ITC.POI>();
                    //GameObject poi_go = pois[poiID].gameObject;

                    for (int v = 0; v < 4; v++)
                    {
                        bool canSpawn = true;
                        for (int p = 0; p < poi.tilesPattern.Count; p++)
                        {
                            if (canSpawn)
                            {
                                if (v == 0)
                                {
                                    xCoord = x + (int)(poi.tilesPattern[p].x / 2f);
                                    zCoord = z + (int)(poi.tilesPattern[p].z / 2f);
                                }
                                else if (v == 1)
                                {
                                    xCoord = x + (int)(poi.tilesPattern[p].z / 2f);
                                    zCoord = z - (int)(poi.tilesPattern[p].x / 2f);
                                }
                                else if (v == 2)
                                {
                                    xCoord = x - (int)(poi.tilesPattern[p].x / 2f);
                                    zCoord = z - (int)(poi.tilesPattern[p].z / 2f);
                                }
                                else if (v == 3)
                                {
                                    xCoord = x - (int)(poi.tilesPattern[p].z / 2f);
                                    zCoord = z + (int)(poi.tilesPattern[p].x / 2f);
                                }

                                if (xCoord > 0 && xCoord < tilemap.GetLength(0) && zCoord > 0 && zCoord < tilemap.GetLength(1) && !tilemap[xCoord, zCoord].blocked && (biome == null || tilemap[xCoord, zCoord].biome == biome))
                                {
                                    float h = tilemap[xCoord, zCoord].pos.y - tilemap[x, z].pos.y;
                                    float h_pattern = poi.tilesPattern[p].y - poi.tilesPattern[0].y;
                                    if (h != h_pattern && p != 0)
                                    {
                                        canSpawn = false;
                                    }
                                }
                                else
                                {
                                    canSpawn = false;
                                }
                            }
                        }
                        if (canSpawn)
                        {
                            suitPOI_ID.Add(poiID);
                            suitPOI_rotation.Add(v);
                            break;
                        }
                    }
                }

                if (suitPOI_ID.Count > 0)
                {
                    int randomPOI = Random.Range(0, suitPOI_ID.Count);

                    POI poi = pois[suitPOI_ID[randomPOI]].GetComponent<WNC.ITC.POI>();
                    GameObject poi_go = pois[suitPOI_ID[randomPOI]];

                    Vector3 tilePos = tilemap[x, z].pos;
                    //if (Settings.Instance.alignBySealevel) tilePos.y -= seaLevel + 1.75f;
                    GameObject spawnedPOI = SpawnGameObject(poi_go);
                    POI spawnedPoi = spawnedPOI.GetComponent<POI>();
                    if (biome == null) spawnedPOI.transform.parent = generator.GetBiomeTransform("");
                    else spawnedPOI.transform.parent = generator.GetBiomeTransform(biome).GetChild(1);
                    spawnedPOI.transform.localEulerAngles = new Vector3(0f, 90f * suitPOI_rotation[randomPOI], 0f);
                    spawnedPOI.transform.localPosition = tilePos;
                    spawnedPOI.name += " " + (suitPOI_rotation[randomPOI] * 90).ToString() + "°";

                    generator.pointsOfInterest.Add(spawnedPoi);

                    for (int p = 0; p < poi.tilesPattern.Count; p++)
                    {
                        if (suitPOI_rotation[randomPOI] == 0)
                        {
                            xCoord = x + (int)(poi.tilesPattern[p].x / 2f);
                            zCoord = z + (int)(poi.tilesPattern[p].z / 2f);
                        }
                        else if (suitPOI_rotation[randomPOI] == 1)
                        {
                            xCoord = x + (int)(poi.tilesPattern[p].z / 2f);
                            zCoord = z - (int)(poi.tilesPattern[p].x / 2f);
                        }
                        else if (suitPOI_rotation[randomPOI] == 2)
                        {
                            xCoord = x - (int)(poi.tilesPattern[p].x / 2f);
                            zCoord = z - (int)(poi.tilesPattern[p].z / 2f);
                        }
                        else if (suitPOI_rotation[randomPOI] == 3)
                        {
                            xCoord = x - (int)(poi.tilesPattern[p].z / 2f);
                            zCoord = z + (int)(poi.tilesPattern[p].x / 2f);
                        }

                        tilemapList.Remove(tilemap[xCoord, zCoord]);
                        if (poi.connectorTiles.Contains(poi.tilesPattern[p]))
                        {
                            tilemap[xCoord, zCoord].connector = true;
                        }
                        else
                        {
                            tilemap[xCoord, zCoord].blocked = true;
                        }
                    }
                }
            }

            return tilemap;
        }

        /// <summary>
        /// Spawns on the tilemap given POI, on the specified biome, provided that there is a free and suitable place for it on the map. If the biome is null, then POI will spawn on any biome, except water type biomes.
        /// </summary>
        /// <param name="poiToSpawn">POI Prefab to spawn.</param> 
        /// <param name="biome">Name of Biome on which POI will spawn. If you pass null to this parameter, the POI will spawn on any biome, except water type biomes.</param> 
        public static TileInfo[,] InsertPOI(TileInfo[,] tilemap, WNC.ITC.Generator generator, GameObject poiToSpawn, string? biome = null)
        {
            List<TileInfo> tilemapList = null;
            if (biome == null) tilemapList = tilemap.Cast<TileInfo>().Where(x => !x.blocked && !x.connector && !x.road).ToList(); //If biome string null we take all tiles, we use it for Global [POI] from Generator Preset
            else tilemapList = tilemap.Cast<TileInfo>().Where(x => x.biome == biome && !x.blocked && !x.connector && !x.road).ToList(); //Otherway we get tiles only of one biome

            for (int i = 0; i < tilemapList.Count; i++)
            {
                TileInfo temp = tilemapList[i];
                int randomIndex = Random.Range(i, tilemapList.Count);
                tilemapList[i] = tilemapList[randomIndex];
                tilemapList[randomIndex] = temp;
            }

            for (int i = 0; i < tilemapList.Count; i++)
            {
                int tileID = Random.Range(0, tilemapList.Count);
                int x = tilemapList[tileID].tileCoordinates.x;
                int z = tilemapList[tileID].tileCoordinates.y;

                List<int> suitPOI_rotation = new List<int>();

                int xCoord = x;
                int zCoord = z;

                POI poi = poiToSpawn.GetComponent<WNC.ITC.POI>();

                for (int v = 0; v < 4; v++)
                {
                    bool canSpawn = true;
                    for (int p = 0; p < poi.tilesPattern.Count; p++)
                    {
                        if (canSpawn)
                        {
                            if (v == 0)
                            {
                                xCoord = x + (int)(poi.tilesPattern[p].x / 2f);
                                zCoord = z + (int)(poi.tilesPattern[p].z / 2f);
                            }
                            else if (v == 1)
                            {
                                xCoord = x + (int)(poi.tilesPattern[p].z / 2f);
                                zCoord = z - (int)(poi.tilesPattern[p].x / 2f);
                            }
                            else if (v == 2)
                            {
                                xCoord = x - (int)(poi.tilesPattern[p].x / 2f);
                                zCoord = z - (int)(poi.tilesPattern[p].z / 2f);
                            }
                            else if (v == 3)
                            {
                                xCoord = x - (int)(poi.tilesPattern[p].z / 2f);
                                zCoord = z + (int)(poi.tilesPattern[p].x / 2f);
                            }

                            if (xCoord > 0 && xCoord < tilemap.GetLength(0) && zCoord > 0 && zCoord < tilemap.GetLength(1) && !tilemap[xCoord, zCoord].blocked && (biome == null || tilemap[xCoord, zCoord].biome == biome))
                            {
                                float h = tilemap[xCoord, zCoord].pos.y - tilemap[x, z].pos.y;
                                float h_pattern = poi.tilesPattern[p].y - poi.tilesPattern[0].y;
                                if (h != h_pattern && p != 0)
                                {
                                    canSpawn = false;
                                }
                            }
                            else
                            {
                                canSpawn = false;
                            }
                        }
                    }
                    if (canSpawn)
                    {
                        suitPOI_rotation.Add(v);
                        break;
                    }
                }

                if (suitPOI_rotation.Count > 0)
                {
                    int randomPOI = Random.Range(0, suitPOI_rotation.Count);

                    GameObject poi_go = poiToSpawn;

                    Vector3 tilePos = tilemap[x, z].pos;
                    GameObject spawnedPOI = SpawnGameObject(poi_go);
                    POI spawnedPoi = spawnedPOI.GetComponent<POI>();
                    if (biome == null) spawnedPOI.transform.parent = generator.GetBiomeTransform("");
                    else spawnedPOI.transform.parent = generator.GetBiomeTransform(biome).GetChild(1);
                    spawnedPOI.transform.localEulerAngles = new Vector3(0f, 90f * suitPOI_rotation[randomPOI], 0f);
                    spawnedPOI.transform.localPosition = tilePos;
                    spawnedPOI.name += " " + (suitPOI_rotation[randomPOI] * 90).ToString() + "°";

                    generator.pointsOfInterest.Add(spawnedPoi);

                    for (int p = 0; p < poi.tilesPattern.Count; p++)
                    {
                        if (suitPOI_rotation[randomPOI] == 0)
                        {
                            xCoord = x + (int)(poi.tilesPattern[p].x / 2f);
                            zCoord = z + (int)(poi.tilesPattern[p].z / 2f);
                        }
                        else if (suitPOI_rotation[randomPOI] == 1)
                        {
                            xCoord = x + (int)(poi.tilesPattern[p].z / 2f);
                            zCoord = z - (int)(poi.tilesPattern[p].x / 2f);
                        }
                        else if (suitPOI_rotation[randomPOI] == 2)
                        {
                            xCoord = x - (int)(poi.tilesPattern[p].x / 2f);
                            zCoord = z - (int)(poi.tilesPattern[p].z / 2f);
                        }
                        else if (suitPOI_rotation[randomPOI] == 3)
                        {
                            xCoord = x - (int)(poi.tilesPattern[p].z / 2f);
                            zCoord = z + (int)(poi.tilesPattern[p].x / 2f);
                        }

                        tilemapList.Remove(tilemap[xCoord, zCoord]);
                        if (poi.connectorTiles.Contains(poi.tilesPattern[p]))
                        {
                            tilemap[xCoord, zCoord].connector = true;
                        }
                        else
                        {
                            tilemap[xCoord, zCoord].blocked = true;
                        }
                    }

                    break;
                }
            }

            return tilemap;
        }

        /// <summary>
        /// Adds roads to the tilemap that are laid between the connectors of POI.
        /// </summary>
        /// <param name="chance">Chance to spawn a road between any pair of connectors on tilemap.</param> 
        public static TileInfo[,] CreateRoadsBetweenPOI(TileInfo[,] tilemap, WNC.ITC.Generator generator, float chance)
        {
            //Roads
            List<TileInfo> roadPointsA = new List<TileInfo>();
            List<TileInfo> roadPointsB = new List<TileInfo>();

            List<TileInfo> connectors = tilemap.Cast<TileInfo>().Where(x => x.connector).ToList();
            int roadsCount = Mathf.FloorToInt(((float)connectors.Count / 2f) * chance);

            for (int i = 0; i < roadsCount; i++)
            {
                roadPointsA.Add(connectors[Random.Range(0, connectors.Count)]);
                connectors.Remove(roadPointsA[i]);
                roadPointsB.Add(connectors[Random.Range(0, connectors.Count)]);
                connectors.Remove(roadPointsB[i]);
            }

            int[,] pathWeights = new int[tilemap.GetLength(0), tilemap.GetLength(1)];
            for (int i = 0; i < tilemap.GetLength(0); i++)
            {
                for (int a = 0; a < tilemap.GetLength(1); a++)
                {
                    pathWeights[i, a] = 0;
                }
            }
            foreach (TileInfo tile in tilemap)
            {
                if (tile.distanceToWater == 0 || tile.biome == "None" || tile.blocked || tile.connector) pathWeights[tile.tileCoordinates.x, tile.tileCoordinates.y] = -1;
            }

            for (int i = 0; i < roadPointsA.Count; i++)
            {
                tilemap = CreateRoad(tilemap, generator, roadPointsA[i], roadPointsB[i]);
            }

            return tilemap;
        }

        /// <summary>
        /// Adds roads to the tilemap that are laid between given tiles.
        /// </summary>
        public static TileInfo[,] CreateRoad(TileInfo[,] tilemap, WNC.ITC.Generator generator, TileInfo startPos, TileInfo endPos)
        {
            int[,] pathWeights = new int[tilemap.GetLength(0), tilemap.GetLength(1)];
            for (int i = 0; i < tilemap.GetLength(0); i++)
            {
                for (int a = 0; a < tilemap.GetLength(1); a++)
                {
                    pathWeights[i, a] = 0;
                }
            }
            foreach (TileInfo tile in tilemap)
            {
                if (tile.distanceToWater == 0 || tile.biome == "None" || tile.blocked || tile.connector) pathWeights[tile.tileCoordinates.x, tile.tileCoordinates.y] = -1;
            }

            int[,] thisPathWeights = pathWeights.Clone() as int[,];

            List<TileInfo> tilesToCheck = new List<TileInfo> { startPos };
            List<TileInfo> tilesQueue = new List<TileInfo>();
            List<TileInfo> checkedTiles = new List<TileInfo>();
            bool pathFinded = false;

            int step = 0;
            while (true)
            {
                if (tilesToCheck.Count == 0) break;

                tilesQueue = new List<TileInfo>();

                foreach (TileInfo tileToCheck in tilesToCheck)
                {
                    if (tileToCheck != startPos)
                        thisPathWeights[tileToCheck.tileCoordinates.x, tileToCheck.tileCoordinates.y] = step;

                    List<Vector2Int> neighbors = new List<Vector2Int>();
                    neighbors.Add(new Vector2Int(-1, 0));
                    neighbors.Add(new Vector2Int(0, -1));
                    neighbors.Add(new Vector2Int(1, 0));
                    neighbors.Add(new Vector2Int(0, 1));

                    foreach (Vector2Int neighbor in neighbors)
                    {
                        int neighborX = tileToCheck.tileCoordinates.x + neighbor.x;
                        int neighborZ = tileToCheck.tileCoordinates.y + neighbor.y;

                        if (neighborX >= 0 && neighborX < tilemap.GetLength(0) && neighborZ >= 0 && neighborZ < tilemap.GetLength(1))
                        {
                            if (tilemap[neighborX, neighborZ] == endPos)
                            {
                                pathFinded = true;
                                break;
                            }
                            else if (thisPathWeights[neighborX, neighborZ] != -1 && !tilesQueue.Contains(tilemap[neighborX, neighborZ]) && !checkedTiles.Contains(tilemap[neighborX, neighborZ]))
                            {
                                tilesQueue.Add(tilemap[neighborX, neighborZ]);
                                checkedTiles.Add(tilemap[neighborX, neighborZ]);
                            }
                        }
                    }

                    if (pathFinded) break;
                }

                if (pathFinded) break;
                else tilesToCheck = tilesQueue;

                step++;
            }

            if (!pathFinded) return tilemap;

            TileInfo pathTile = endPos;
            List<TileInfo> checkedPathTiles = new List<TileInfo>();
            bool pathEnded = false;

            while (true)
            {
                if (pathTile != endPos)
                {
                    tilemap[pathTile.tileCoordinates.x, pathTile.tileCoordinates.y].road = true;
                }
                checkedPathTiles.Add(pathTile);

                List<Vector2Int> neighbors = new List<Vector2Int>();
                neighbors.Add(new Vector2Int(-1, 0));
                neighbors.Add(new Vector2Int(0, -1));
                neighbors.Add(new Vector2Int(1, 0));
                neighbors.Add(new Vector2Int(0, 1));

                int minNeighborValue = int.MaxValue;
                Vector2Int nextNeighbor = Vector2Int.zero;

                foreach (Vector2Int neighbor in neighbors)
                {
                    int neighborX = pathTile.tileCoordinates.x + neighbor.x;
                    int neighborZ = pathTile.tileCoordinates.y + neighbor.y;

                    if (neighborX >= 0 && neighborX < tilemap.GetLength(0) && neighborZ >= 0 && neighborZ < tilemap.GetLength(1))
                    {
                        if (tilemap[neighborX, neighborZ] == startPos)
                        {
                            pathEnded = true;
                            break;
                        }
                        int thisNeighborValue = thisPathWeights[neighborX, neighborZ];
                        if (thisNeighborValue > 0 && !checkedPathTiles.Contains(tilemap[neighborX, neighborZ]))
                        {
                            if (thisNeighborValue < minNeighborValue)
                            {
                                minNeighborValue = thisNeighborValue;
                                nextNeighbor = neighbor;
                            }
                        }
                    }
                }

                if (pathEnded) break;
                if (nextNeighbor == Vector2Int.zero) break;

                pathTile = tilemap[pathTile.tileCoordinates.x + nextNeighbor.x, pathTile.tileCoordinates.y + nextNeighbor.y];
            }

            return tilemap;
        }

        /// <summary>
        /// Spawns all roads that are in the tilemap.
        /// </summary>
        /// <param name="roadFilling">Chance to spawn of road tile.</param> 
        /// <param name="fenceChance">Chance to spawn of fence around road.</param> 
        public static void SpawnRoads(TileInfo[,] tilemap, WNC.ITC.Generator generator, float roadFilling, float fenceChance)
        {
            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int z = 0; z < tilemap.GetLength(1); z++)
                {
                    if (tilemap[x, z].road)
                    {
                        if (Random.Range(0f, 1f) < roadFilling)
                        {
                            Biome biome = Settings.Instance.GetBiomeByName(tilemap[x, z].biome);

                            if (biome != null && biome.roadPreset != null)
                            {
                                float yEulers = 0f;
                                float yOffset = 0f;
                                bool spawned = true;

                                GameObject road = null;

                                bool right = false;
                                bool left = false;
                                bool up = false;
                                bool down = false;

                                bool fencePossible = true;

                                List<Vector3> verticalLaddersEuls = new List<Vector3>();

                                if (x + 1 < tilemap.GetLength(0) && (tilemap[x + 1, z].road || tilemap[x + 1, z].connector)) right = true;
                                if (x - 1 >= 0 && (tilemap[x - 1, z].road || tilemap[x - 1, z].connector)) left = true;
                                if (z + 1 < tilemap.GetLength(1) && (tilemap[x, z + 1].road || tilemap[x, z + 1].connector)) up = true;
                                if (z - 1 >= 0 && (tilemap[x, z - 1].road || tilemap[x, z - 1].connector)) down = true;

                                if (up && down && right && left)
                                {
                                    bool rightUp = (int)tilemap[x + 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool leftUp = (int)tilemap[x - 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool upUp = (int)tilemap[x, z + 1].pos.y > (int)tilemap[x, z].pos.y;
                                    bool downUp = (int)tilemap[x, z - 1].pos.y > (int)tilemap[x, z].pos.y;

                                    if (rightUp) verticalLaddersEuls.Add(new Vector3(0f, 270f, 0f));
                                    if (upUp) verticalLaddersEuls.Add(new Vector3(0f, 180f, 0f));
                                    if (downUp) verticalLaddersEuls.Add(new Vector3(0f, 0f, 0f));
                                    if (leftUp) verticalLaddersEuls.Add(new Vector3(0f, 90f, 0f));

                                    road = SpawnGameObject(biome.roadPreset.crossroad[Random.Range(0, biome.roadPreset.crossroad.Length)]);
                                    yEulers = 90f * Random.Range(0, 4);
                                }
                                else if (up && down && right)
                                {
                                    bool rightUp = (int)tilemap[x + 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool upUp = (int)tilemap[x, z + 1].pos.y > (int)tilemap[x, z].pos.y;
                                    bool downUp = (int)tilemap[x, z - 1].pos.y > (int)tilemap[x, z].pos.y;

                                    if (rightUp) verticalLaddersEuls.Add(new Vector3(0f, 270f, 0f));
                                    if (upUp) verticalLaddersEuls.Add(new Vector3(0f, 180f, 0f));
                                    if (downUp) verticalLaddersEuls.Add(new Vector3(0f, 0f, 0f));

                                    road = SpawnGameObject(biome.roadPreset.tripleCrossroad[Random.Range(0, biome.roadPreset.tripleCrossroad.Length)]);
                                    yEulers = 180f;
                                }
                                else if (up && left && right)
                                {
                                    bool rightUp = (int)tilemap[x + 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool leftUp = (int)tilemap[x - 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool upUp = (int)tilemap[x, z + 1].pos.y > (int)tilemap[x, z].pos.y;

                                    if (rightUp) verticalLaddersEuls.Add(new Vector3(0f, 270f, 0f));
                                    if (upUp) verticalLaddersEuls.Add(new Vector3(0f, 180f, 0f));
                                    if (leftUp) verticalLaddersEuls.Add(new Vector3(0f, 90f, 0f));

                                    road = SpawnGameObject(biome.roadPreset.tripleCrossroad[Random.Range(0, biome.roadPreset.tripleCrossroad.Length)]);
                                    yEulers = 90f;
                                }
                                else if (up && left && down)
                                {
                                    bool leftUp = (int)tilemap[x - 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool upUp = (int)tilemap[x, z + 1].pos.y > (int)tilemap[x, z].pos.y;
                                    bool downUp = (int)tilemap[x, z - 1].pos.y > (int)tilemap[x, z].pos.y;

                                    if (upUp) verticalLaddersEuls.Add(new Vector3(0f, 180f, 0f));
                                    if (downUp) verticalLaddersEuls.Add(new Vector3(0f, 0f, 0f));
                                    if (leftUp) verticalLaddersEuls.Add(new Vector3(0f, 90f, 0f));

                                    road = SpawnGameObject(biome.roadPreset.tripleCrossroad[Random.Range(0, biome.roadPreset.tripleCrossroad.Length)]);
                                    yEulers = 0f;
                                }
                                else if (right && left && down)
                                {
                                    bool leftUp = (int)tilemap[x - 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool rightUp = (int)tilemap[x + 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool downUp = (int)tilemap[x, z - 1].pos.y > (int)tilemap[x, z].pos.y;

                                    if (rightUp) verticalLaddersEuls.Add(new Vector3(0f, 270f, 0f));
                                    if (downUp) verticalLaddersEuls.Add(new Vector3(0f, 0f, 0f));
                                    if (leftUp) verticalLaddersEuls.Add(new Vector3(0f, 90f, 0f));

                                    road = SpawnGameObject(biome.roadPreset.tripleCrossroad[Random.Range(0, biome.roadPreset.tripleCrossroad.Length)]);
                                    yEulers = 270f;
                                }
                                else if (right && left)
                                {
                                    int thisHeight = (int)tilemap[x, z].pos.y;
                                    int rightHeight = (int)tilemap[x + 1, z].pos.y;
                                    int leftHeight = (int)tilemap[x - 1, z].pos.y;

                                    if (rightHeight == thisHeight && leftHeight > thisHeight)
                                    {
                                        road = SpawnGameObject(biome.roadPreset.ladders[Random.Range(0, biome.roadPreset.ladders.Length)]);
                                        yEulers = 90f;
                                        yOffset = 1f;
                                        fencePossible = false;
                                    }
                                    else if (leftHeight == thisHeight && rightHeight > thisHeight)
                                    {
                                        road = SpawnGameObject(biome.roadPreset.ladders[Random.Range(0, biome.roadPreset.ladders.Length)]);
                                        yEulers = -90f;
                                        yOffset = 1f;
                                        fencePossible = false;
                                    }
                                    else if (leftHeight > thisHeight && rightHeight > thisHeight)
                                    {
                                        road = SpawnGameObject(biome.roadPreset.bridges[Random.Range(0, biome.roadPreset.bridges.Length)]);
                                        yEulers = -90f;
                                        fencePossible = false;
                                    }
                                    else if (rightHeight > thisHeight && leftHeight < thisHeight)
                                    {
                                        verticalLaddersEuls.Add(new Vector3(0f, -90f, 0f));
                                    }
                                    else if (leftHeight > thisHeight && rightHeight < thisHeight)
                                    {
                                        verticalLaddersEuls.Add(new Vector3(0f, 90f, 0f));
                                    }
                                    else
                                    {
                                        road = SpawnGameObject(biome.roadPreset.straight[Random.Range(0, biome.roadPreset.straight.Length)]);
                                        yEulers = 0f;
                                    }
                                }
                                else if (up && down)
                                {
                                    int thisHeight = (int)tilemap[x, z].pos.y;
                                    int upHeight = (int)tilemap[x, z + 1].pos.y;
                                    int downHeight = (int)tilemap[x, z - 1].pos.y;

                                    if (upHeight == thisHeight & downHeight > thisHeight)
                                    {
                                        road = SpawnGameObject(biome.roadPreset.ladders[Random.Range(0, biome.roadPreset.ladders.Length)]);
                                        yEulers = 00f;
                                        yOffset = 1f;
                                        fencePossible = false;
                                    }
                                    else if (downHeight == thisHeight && upHeight > thisHeight)
                                    {
                                        road = SpawnGameObject(biome.roadPreset.ladders[Random.Range(0, biome.roadPreset.ladders.Length)]);
                                        yEulers = 180f;
                                        yOffset = 1f;
                                        fencePossible = false;
                                    }
                                    else if (downHeight > thisHeight && upHeight > thisHeight)
                                    {
                                        road = SpawnGameObject(biome.roadPreset.bridges[Random.Range(0, biome.roadPreset.bridges.Length)]);
                                        yEulers = 0f;
                                        fencePossible = false;
                                    }
                                    else if (upHeight > thisHeight && downHeight < thisHeight)
                                    {
                                        verticalLaddersEuls.Add(new Vector3(0f, 180f, 0f));
                                    }
                                    else if (downHeight > thisHeight && upHeight < thisHeight)
                                    {
                                        verticalLaddersEuls.Add(new Vector3(0f, 0f, 0f));
                                    }
                                    else
                                    {
                                        road = SpawnGameObject(biome.roadPreset.straight[Random.Range(0, biome.roadPreset.straight.Length)]);
                                        yEulers = 90f;
                                    }
                                }
                                else if (right && down)
                                {
                                    bool rightUp = (int)tilemap[x + 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool downUp = (int)tilemap[x, z - 1].pos.y > (int)tilemap[x, z].pos.y;

                                    if (rightUp) verticalLaddersEuls.Add(new Vector3(0f, 270f, 0f));
                                    if (downUp) verticalLaddersEuls.Add(new Vector3(0f, 0f, 0f));

                                    road = SpawnGameObject(biome.roadPreset.turn[Random.Range(0, biome.roadPreset.turn.Length)]);
                                    yEulers = 180;
                                }
                                else if (right && up)
                                {
                                    bool rightUp = (int)tilemap[x + 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool upUp = (int)tilemap[x, z + 1].pos.y > (int)tilemap[x, z].pos.y;

                                    if (rightUp) verticalLaddersEuls.Add(new Vector3(0f, 270f, 0f));
                                    if (upUp) verticalLaddersEuls.Add(new Vector3(0f, 180f, 0f));

                                    road = SpawnGameObject(biome.roadPreset.turn[Random.Range(0, biome.roadPreset.turn.Length)]);
                                    yEulers = 90f;
                                }
                                else if (left && up)
                                {
                                    bool leftUp = (int)tilemap[x - 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool upUp = (int)tilemap[x, z + 1].pos.y > (int)tilemap[x, z].pos.y;

                                    if (upUp) verticalLaddersEuls.Add(new Vector3(0f, 180f, 0f));
                                    if (leftUp) verticalLaddersEuls.Add(new Vector3(0f, 90f, 0f));

                                    road = SpawnGameObject(biome.roadPreset.turn[Random.Range(0, biome.roadPreset.turn.Length)]);
                                    yEulers = 0f;
                                }
                                else if (left && down)
                                {
                                    bool leftUp = (int)tilemap[x - 1, z].pos.y > (int)tilemap[x, z].pos.y;
                                    bool downUp = (int)tilemap[x, z - 1].pos.y > (int)tilemap[x, z].pos.y;

                                    if (downUp) verticalLaddersEuls.Add(new Vector3(0f, 0f, 0f));
                                    if (leftUp) verticalLaddersEuls.Add(new Vector3(0f, 90f, 0f));

                                    road = SpawnGameObject(biome.roadPreset.turn[Random.Range(0, biome.roadPreset.turn.Length)]);
                                    yEulers = 270f;
                                }
                                else if (up)
                                {
                                    road = SpawnGameObject(biome.roadPreset.ending[Random.Range(0, biome.roadPreset.ending.Length)]);
                                    yEulers = 270f;
                                }
                                else if (down)
                                {
                                    road = SpawnGameObject(biome.roadPreset.ending[Random.Range(0, biome.roadPreset.ending.Length)]);
                                    yEulers = 90f;
                                }
                                else if (right)
                                {
                                    road = SpawnGameObject(biome.roadPreset.ending[Random.Range(0, biome.roadPreset.ending.Length)]);
                                    yEulers = 00f;
                                }
                                else if (left)
                                {
                                    road = SpawnGameObject(biome.roadPreset.ending[Random.Range(0, biome.roadPreset.ending.Length)]);
                                    yEulers = 180f;
                                }
                                else
                                {
                                    spawned = false;
                                }

                                if (spawned)
                                {
                                    Vector3 tilePos = tilemap[x, z].pos;
                                    //if (Settings.Instance.alignBySealevel) tilePos.y -= seaLevel + 1.75f;

                                    if (road != null)
                                    {
                                        road.transform.parent = generator.GetBiomeTransform(biome.setName).GetChild(5);
                                        road.transform.localPosition = tilePos + new Vector3(0f, yOffset, 0f);
                                        road.transform.localEulerAngles = new Vector3(0f, yEulers, 0f);
                                        generator.roads.Add(road);
                                    }

                                    if (fencePossible)
                                    {
                                        List<Vector3> fenceEulers = new List<Vector3>();
                                        if (!up && Random.Range(0f, 1f) < fenceChance) fenceEulers.Add(new Vector3(0f, 0f, 0f));
                                        if (!right && Random.Range(0f, 1f) < fenceChance) fenceEulers.Add(new Vector3(0f, 90f, 0f));
                                        if (!down && Random.Range(0f, 1f) < fenceChance) fenceEulers.Add(new Vector3(0f, 180f, 0f));
                                        if (!left && Random.Range(0f, 1f) < fenceChance) fenceEulers.Add(new Vector3(0f, 270f, 0f));
                                        foreach (Vector3 fenceEul in fenceEulers)
                                        {
                                            GameObject ladder = SpawnGameObject(biome.roadPreset.fences[Random.Range(0, biome.roadPreset.fences.Length)]);
                                            ladder.transform.parent = generator.GetBiomeTransform(biome.setName).GetChild(5);
                                            ladder.transform.localPosition = tilePos;
                                            ladder.transform.localEulerAngles = fenceEul;
                                            generator.roads.Add(ladder);
                                        }
                                    }
                                    foreach (Vector3 verLadderEul in verticalLaddersEuls)
                                    {
                                        GameObject ladder = SpawnGameObject(biome.roadPreset.verticalLadders[Random.Range(0, biome.roadPreset.verticalLadders.Length)]);
                                        ladder.transform.parent = generator.GetBiomeTransform(biome.setName).GetChild(5);
                                        ladder.transform.localPosition = tilePos + new Vector3(0f, 1f, 0f);
                                        ladder.transform.localEulerAngles = verLadderEul;
                                        generator.roads.Add(ladder);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Spawns all filling objects for the specified biome according to the settings specified in this biome.
        /// </summary>
        /// <param name="biome">Name of biome which presented in given tilemap, to be filled.</param> 
        public static TileInfo[,] SpawnFilling(TileInfo[,] tilemap, WNC.ITC.Generator generator, string biome)
        {
            List<TileInfo> tilemapList = tilemap.Cast<TileInfo>().Where(x => x.biome == biome && !x.blocked && !x.connector && !x.road).ToList();
            Biome thisBiome = Settings.Instance.GetBiomeByName(biome);

            int fillingCount = (int)((float)tilemapList.Count * (thisBiome.filling));

            for (int t = 0; t < fillingCount; t++)
            {
                int tileID = Random.Range(0, tilemapList.Count);

                float treesChance = thisBiome.treesChance;
                float decorChance = thisBiome.decorsChance;
                float uniquesChance = thisBiome.uniquesChance;

                float spawnType = Random.Range(0f, treesChance + decorChance + uniquesChance);
                if (spawnType < treesChance && thisBiome.trees.Count > 0)
                {
                    Vector3 pos = tilemapList[tileID].pos + new Vector3(0f, Settings.Instance.tileSize / 2, 0f);
                    //if (Settings.Instance.alignBySealevel) pos.y -= seaLevel + 1.75f;
                    GameObject newTree = SpawnGameObject(thisBiome.trees[Random.Range(0, thisBiome.trees.Count)]);
                    newTree.transform.parent = generator.GetBiomeTransform(biome).GetChild(2);
                    newTree.transform.localPosition = pos;
                    newTree.transform.localEulerAngles = new Vector3(0f, 90f * Random.Range(0, 4), 0f);
                    newTree.transform.localScale = Vector3.Lerp(thisBiome.treesMinScale, thisBiome.treesMaxScale, Random.Range(0f, 1f));
                    generator.trees.Add(newTree);
                    tilemap[tilemapList[tileID].tileCoordinates.x, tilemapList[tileID].tileCoordinates.y].blocked = true;
                }
                else if (spawnType < treesChance + decorChance && thisBiome.decors.Count > 0)
                {
                    Vector3 pos = tilemapList[tileID].pos + new Vector3(0f, Settings.Instance.tileSize / 2, 0f);
                    //if (Settings.Instance.alignBySealevel) pos.y -= seaLevel + 1.75f;
                    GameObject newDecor = SpawnGameObject(thisBiome.decors[Random.Range(0, thisBiome.decors.Count)]);
                    newDecor.transform.parent = generator.GetBiomeTransform(biome).GetChild(3);
                    newDecor.transform.localPosition = pos;
                    newDecor.transform.localEulerAngles = new Vector3(0f, 90f * Random.Range(0, 4), 0f);
                    newDecor.transform.localScale = Vector3.Lerp(thisBiome.decorMinScale, thisBiome.decorMaxScale, Random.Range(0f, 1f));
                    generator.decors.Add(newDecor);
                    tilemap[tilemapList[tileID].tileCoordinates.x, tilemapList[tileID].tileCoordinates.y].blocked = true;
                }
                else if (spawnType < treesChance + decorChance + uniquesChance && thisBiome.uniques.Count > 0)
                {
                    Vector3 pos = tilemapList[tileID].pos + new Vector3(0f, Settings.Instance.tileSize / 2, 0f);
                    //if (Settings.Instance.alignBySealevel) pos.y -= seaLevel + 1.75f;
                    GameObject newUnique = SpawnGameObject(thisBiome.uniques[Random.Range(0, thisBiome.uniques.Count)]);
                    newUnique.transform.parent = generator.GetBiomeTransform(biome).GetChild(4);
                    newUnique.transform.localPosition = pos;
                    newUnique.transform.localEulerAngles = new Vector3(0f, 90f * Random.Range(0, 4), 0f);
                    newUnique.transform.localScale = Vector3.Lerp(thisBiome.uniquesMinScale, thisBiome.uniquesMaxScale, Random.Range(0f, 1f));
                    generator.uniques.Add(newUnique);
                    tilemap[tilemapList[tileID].tileCoordinates.x, tilemapList[tileID].tileCoordinates.y].blocked = true;
                }
            }

            return tilemap;
        }

        public static GameObject SpawnGameObject(GameObject gameObjectToSpawn, Transform objectParent = null)
        {
            GameObject spawnedGameObject = null;

#if UNITY_EDITOR
            spawnedGameObject = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(gameObjectToSpawn, objectParent);
#else
            spawnedGameObject = Instantiate(gameObjectToSpawn, objectParent);
#endif

            return spawnedGameObject;
        }
    }
}