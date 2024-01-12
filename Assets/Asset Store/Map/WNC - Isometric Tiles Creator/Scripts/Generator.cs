using System.Collections.Generic;
using UnityEngine;

namespace WNC.ITC
{
    public class Generator : MonoBehaviour
    {
        public WNC.ITC.GeneratorPreset generatorPreset;
        public Vector2Int mapSize;
        public Vector2Int mapOffset;
        public bool mapGenerated = false;
        public WNC.ITC.TileInfo[,] tilemap;
        public List<WNC.ITC.POI> pointsOfInterest;
        public List<GameObject> trees;
        public List<GameObject> decors;
        public List<GameObject> uniques;
        public List<GameObject> waterTiles;
        public List<GameObject> tiles;
        public List<GameObject> roads;

        /// <summary>
        /// Generating map by preset
        /// </summary>
        public void Generate()
        {
            Clear();

            WNC.ITC.IsometricTilesCreator.GenerateMap(generatorPreset,this);
        }

        /// <summary>
        /// Regenerating map with same noise offset
        /// </summary>
        public void Refresh()
        {
            Vector2Int offset = mapOffset;
            Clear();
            WNC.ITC.IsometricTilesCreator.GenerateMap(generatorPreset, this, offset);
        }
        /// <summary>
        /// Creates new generator nearby and generate map in them. 
        /// </summary>
        public void GenerateLeft()
        {
            GameObject newGenerator = WNC.ITC.IsometricTilesCreator.GenerateMap(generatorPreset, null, new Vector2Int(mapOffset.x - mapSize.x, mapOffset.y));
            newGenerator.transform.position = transform.position + new Vector3(-(mapSize.x * WNC.ITC.Settings.Instance.tileSize), 0, 0);
        }
        /// <summary>
        /// Creates new generator nearby and generate map in them. 
        /// </summary>
        public void GenerateRight()
        {
            GameObject newGenerator = WNC.ITC.IsometricTilesCreator.GenerateMap(generatorPreset, null, new Vector2Int(mapOffset.x + mapSize.x, mapOffset.y));
            newGenerator.transform.position = transform.position + new Vector3((mapSize.x * WNC.ITC.Settings.Instance.tileSize), 0, 0);
        }
        /// <summary>
        /// Creates new generator nearby and generate map in them. 
        /// </summary>
        public void GenerateDown()
        {
            GameObject newGenerator = WNC.ITC.IsometricTilesCreator.GenerateMap(generatorPreset, null, new Vector2Int(mapOffset.x, mapOffset.y - mapSize.y));
            newGenerator.transform.position = transform.position + new Vector3(0, 0, -(mapSize.y * WNC.ITC.Settings.Instance.tileSize));
        }
        /// <summary>
        /// Creates new generator nearby and generate map in them. 
        /// </summary>
        public void GenerateUp()
        {
            GameObject newGenerator = WNC.ITC.IsometricTilesCreator.GenerateMap(generatorPreset, null, new Vector2Int(mapOffset.x, mapOffset.y + mapSize.y));
            newGenerator.transform.position = transform.position + new Vector3(0, 0, (mapSize.y * WNC.ITC.Settings.Instance.tileSize));
        }
        /// <summary>
        /// Clear Generator from spawned prefabs and stored information
        /// </summary>
        public void Clear()
        {
            mapGenerated = false;

            mapSize = Vector2Int.zero;
            mapOffset = Vector2Int.zero;

            for (int i = transform.childCount - 1; i >= 0; i--) DestroyImmediate(transform.GetChild(i).gameObject);

            tilemap = null;
            pointsOfInterest = new List<WNC.ITC.POI>();
            trees = new List<GameObject>();
            decors = new List<GameObject>();
            uniques = new List<GameObject>();
            waterTiles = new List<GameObject>();
            tiles = new List<GameObject>();
            roads = new List<GameObject>();
        }
        /// <summary>
        /// Return a parent of all objects of biome
        /// </summary>
        public Transform GetBiomeTransform(string biome)
        {
            string nameToFind = "[Biome] - " + biome;

            for (int i = 0; i<transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.gameObject.name == nameToFind) return child;
            }

            return transform;
        }
    }
}
