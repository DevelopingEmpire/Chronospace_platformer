using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WNC.ITC
{
    [CustomEditor(typeof(POI))]
    public class POIEditor : Editor
    {
        GUIStyle s_Line;
        GUIStyle s_Header;
        GUIStyle s_Button;
        GUIStyle s_DescriptionCentered;
        GUIStyle s_SubDescriptionCentered;

        private void OnDestroy()
        {
            POI poi = ((POI)target);

            if (poi == null)
            {
                if (poi.controlsParent != null) DestroyImmediate(poi.controlsParent.gameObject);
            }
        }
        private void OnDisable()
        {
            POI poi = ((POI)target);
            Init();

            if (!poi.lockGridVisibility)
            {
                if (poi.controlsParent != null) DestroyImmediate(poi.controlsParent.gameObject);
                poi.poiTiles = new List<GameObject>();
                poi.editorSelected = false;
                poi.editMode = false;
            }
        }
        private void OnEnable()
        {
            POI poi = ((POI)target);
            if (poi.gameObject.scene.name != null)
            {
                Init();

                if (poi.tilesPattern.Count == 0)
                {
                    poi.tilesPattern.Add(Vector3.zero);
                }

                if (poi.connectorTiles == null) poi.connectorTiles = new List<Vector3>();

                bool spawnControls = false;
                if (poi.poiTiles.Count == 0) spawnControls = true;
                else if (poi.poiTiles.Count != poi.tilesPattern.Count) spawnControls = true;

                if (spawnControls)
                {
                    RefreshControls();
                }

                poi.editorSelected = true;
            }
        }
        void Init()
        {
            s_Button = new GUIStyle();
            s_Button.alignment = TextAnchor.MiddleCenter;

            s_Line = new GUIStyle();
            s_Line.normal.background = EditorGUIUtility.whiteTexture;
            s_Line.margin = new RectOffset(0, 0, 0, 0);

            s_Header = new GUIStyle();
            s_Header.alignment = TextAnchor.MiddleCenter;
            s_Header.normal.textColor = Color.white;
            s_Header.fontSize = 14;
            s_Header.fontStyle = FontStyle.Bold;

            s_DescriptionCentered = new GUIStyle();
            s_DescriptionCentered.alignment = TextAnchor.MiddleCenter;
            s_DescriptionCentered.normal.textColor = new Color32(175, 175, 175, 255);
            s_DescriptionCentered.fontSize = 12;
            s_DescriptionCentered.fontStyle = FontStyle.Bold;

            s_SubDescriptionCentered = new GUIStyle();
            s_SubDescriptionCentered.alignment = TextAnchor.MiddleCenter;
            s_SubDescriptionCentered.normal.textColor = new Color32(175, 175, 175, 255);
            s_SubDescriptionCentered.fontSize = 10;
            s_SubDescriptionCentered.fontStyle = FontStyle.Bold;


            POI poi = ((POI)target);
            if (poi.tilesPattern == null)
            {
                poi.tilesPattern = new List<Vector3>();
            }
            if (poi.poiTiles == null)
            {
                poi.poiTiles = new List<GameObject>();
            }
        }
        void RefreshControls()
        { 
            POI poi = ((POI)target);
            List<Vector3> addersPoses = new List<Vector3>();

            Material material_poi_tile = Resources.Load<Material>("WNC_ITC/Materials/WNC_ITC_POI_Tile");
            Material material_poi_adder = Resources.Load<Material>("WNC_ITC/Materials/WNC_ITC_POI_Adder");
            Material material_poi_connector = Resources.Load<Material>("WNC_ITC/Materials/WNC_ITC_POI_Connector");

            if (poi.controlsParent != null) DestroyImmediate(poi.controlsParent.gameObject);
            poi.controlsParent = new GameObject().transform;
            poi.controlsParent.gameObject.name = "[POI] Controls";
            poi.controlsParent.position = poi.transform.position;
            poi.controlsParent.rotation = poi.transform.rotation;
            poi.controlsParent.gameObject.hideFlags = HideFlags.HideInHierarchy;
            EditorUtility.SetDirty(poi);

            poi.poiTiles = new List<GameObject>();

            foreach (Vector3 localTilePos in poi.tilesPattern)
            {
                float tileSize = 2f;

                GameObject newTile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newTile.name = "[WNC] ITC POI Tile";
                newTile.GetComponent<MeshRenderer>().enabled = true;
                if(poi.connectorTiles != null && poi.connectorTiles.Contains(localTilePos))
                    newTile.GetComponent<MeshRenderer>().material = material_poi_connector;
                else
                    newTile.GetComponent<MeshRenderer>().material = material_poi_tile;
                newTile.transform.localScale = Vector3.one * tileSize;
                newTile.transform.parent = poi.controlsParent;
                newTile.transform.localPosition = localTilePos;
                newTile.hideFlags = HideFlags.HideInHierarchy;

                GameObject tileAdder = null;
                bool canAddHere = true;
                foreach (Vector3 tilePos in poi.tilesPattern)
                {
                    Vector2 tile2d = new Vector2(tilePos.x, tilePos.z);
                    Vector3 addPos = localTilePos + new Vector3(-(tileSize / 1f), 0f, 0f);
                    Vector2 checkPos = new Vector2(addPos.x, addPos.z);
                    if (tile2d == checkPos)
                    {
                        canAddHere = false;
                        break;
                    }
                }
                if (canAddHere)
                {
                    tileAdder = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    tileAdder.GetComponent<MeshRenderer>().enabled = true;
                    tileAdder.GetComponent<MeshRenderer>().material = material_poi_adder;
                    tileAdder.transform.localScale = Vector3.one * tileSize * 0.9f;
                    tileAdder.transform.parent = newTile.transform;
                    tileAdder.transform.localPosition = new Vector3(-(tileSize / 2f), 0f, 0f);
                    tileAdder.name = "[WNC] ITC POI Tile Adder";
                    tileAdder.hideFlags = HideFlags.HideInHierarchy;
                    addersPoses.Add(newTile.transform.TransformPoint(tileAdder.transform.localPosition));
                }
                canAddHere = true;
                foreach (Vector3 tilePos in poi.tilesPattern)
                {
                    Vector2 tile2d = new Vector2(tilePos.x, tilePos.z);
                    Vector3 addPos = localTilePos + new Vector3((tileSize / 1f), 0f, 0f);
                    Vector2 checkPos = new Vector2(addPos.x, addPos.z);
                    if (tile2d == checkPos)
                    {
                        canAddHere = false;
                        break;
                    }
                }
                if (canAddHere)
                {
                    tileAdder = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    tileAdder.GetComponent<MeshRenderer>().enabled = true;
                    tileAdder.GetComponent<MeshRenderer>().material = material_poi_adder;
                    tileAdder.transform.localScale = Vector3.one * tileSize * 0.9f;
                    tileAdder.transform.parent = newTile.transform;
                    tileAdder.transform.localPosition = new Vector3((tileSize / 2f), 0f, 0f);
                    tileAdder.name = "[WNC] ITC POI Tile Adder";
                    tileAdder.hideFlags = HideFlags.HideInHierarchy;
                    addersPoses.Add(newTile.transform.TransformPoint(tileAdder.transform.localPosition));
                }
                canAddHere = true;
                foreach (Vector3 tilePos in poi.tilesPattern)
                {
                    Vector2 tile2d = new Vector2(tilePos.x, tilePos.z);
                    Vector3 addPos = localTilePos + new Vector3(0f, 0f, -(tileSize / 1f));
                    Vector2 checkPos = new Vector2(addPos.x, addPos.z);
                    if (tile2d == checkPos)
                    {
                        canAddHere = false;
                        break;
                    }
                }
                if (canAddHere)
                {
                    tileAdder = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    tileAdder.GetComponent<MeshRenderer>().enabled = true;
                    tileAdder.GetComponent<MeshRenderer>().material = material_poi_adder;
                    tileAdder.transform.localScale = Vector3.one * tileSize * 0.9f;
                    tileAdder.transform.parent = newTile.transform;
                    tileAdder.transform.localPosition = new Vector3(0f, 0f, -(tileSize / 2f));
                    tileAdder.name = "[WNC] ITC POI Tile Adder";
                    tileAdder.hideFlags = HideFlags.HideInHierarchy;
                    addersPoses.Add(newTile.transform.TransformPoint(tileAdder.transform.localPosition));
                }
                canAddHere = true;
                foreach (Vector3 tilePos in poi.tilesPattern)
                {
                    Vector2 tile2d = new Vector2(tilePos.x, tilePos.z);
                    Vector3 addPos = localTilePos + new Vector3(0f, 0f, (tileSize / 1f));
                    Vector2 checkPos = new Vector2(addPos.x, addPos.z);
                    if (tile2d == checkPos)
                    {
                        canAddHere = false;
                        break;
                    }
                }
                if (canAddHere)
                {
                    tileAdder = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    tileAdder.GetComponent<MeshRenderer>().enabled = true;
                    tileAdder.GetComponent<MeshRenderer>().material = material_poi_adder;
                    tileAdder.transform.localScale = Vector3.one * tileSize * 0.9f;
                    tileAdder.transform.parent = newTile.transform;
                    tileAdder.transform.localPosition = new Vector3(0f, 0f, (tileSize / 2f));
                    tileAdder.name = "[WNC] ITC POI Tile Adder";
                    tileAdder.hideFlags = HideFlags.HideInHierarchy;
                    addersPoses.Add(newTile.transform.TransformPoint(tileAdder.transform.localPosition));
                }
                canAddHere = true;
                poi.poiTiles.Add(newTile);
            }

            poi.adders = addersPoses;
        }
        private void OnSceneGUI()
        {
            float tileSize = 2f;
            POI poi = ((POI)target);
            if (poi.editMode)
            {
                Handles.color = new Color32(255, 145, 0, 255);
                Event e = Event.current;

                RaycastHit hit;
                Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(r, out hit))
                {
                    if (hit.collider.gameObject.name == "[WNC] ITC POI Tile" || hit.collider.gameObject.name == "[WNC] ITC POI Tile Adder")
                    {
                        poi.raycastedPOIcollider = hit.collider.gameObject.transform.position;
                        poi.raycastedHit = hit.point;
                        if (hit.collider.gameObject.name == "[WNC] ITC POI Tile")
                        {
                            poi.edit_rayType = 0;
                        }
                        else if (hit.collider.gameObject.name == "[WNC] ITC POI Tile Adder")
                        {
                            poi.edit_rayType = 1;
                        }
                    }
                    else
                    {
                        poi.edit_rayType = -1;
                    }
                }
                else
                {
                    poi.edit_rayType = -1;
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.W)
                {
                    r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                    if (Physics.Raycast(r, out hit))
                    {
                        if (hit.collider.gameObject.name == "[WNC] ITC POI Tile")
                        {
                            EditorUtility.SetDirty(target);

                            Vector3 localPos = hit.collider.gameObject.transform.localPosition;
                            if (localPos != Vector3.zero)
                            {
                                bool isConnector = poi.connectorTiles.Contains(localPos);
                                if (isConnector) poi.connectorTiles.Remove(localPos);
                                poi.tilesPattern.Remove(localPos);
                                localPos.y += 2f;
                                poi.tilesPattern.Add(localPos);
                                if (isConnector) poi.connectorTiles.Add(localPos);
                                RefreshControls();
                            }
                        }
                    }
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.S)
                {
                    r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                    if (Physics.Raycast(r, out hit))
                    {
                        if (hit.collider.gameObject.name == "[WNC] ITC POI Tile")
                        {
                            EditorUtility.SetDirty(target);

                            Vector3 localPos = hit.collider.gameObject.transform.localPosition;
                            if (localPos != Vector3.zero)
                            {
                                bool isConnector = poi.connectorTiles.Contains(localPos);
                                if (isConnector) poi.connectorTiles.Remove(localPos);
                                poi.tilesPattern.Remove(localPos);
                                localPos.y -= 2f;
                                poi.tilesPattern.Add(localPos);
                                if (isConnector) poi.connectorTiles.Add(localPos);
                                RefreshControls();
                            }
                        }
                    }
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.D)
                {
                    r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                    if (Physics.Raycast(r, out hit))
                    {
                        if (hit.collider.gameObject.name == "[WNC] ITC POI Tile")
                        {
                            EditorUtility.SetDirty(target);

                            Vector3 localPos = hit.collider.gameObject.transform.localPosition;
                            if (localPos != Vector3.zero)
                            {
                                if (poi.connectorTiles.Contains(localPos))
                                    poi.connectorTiles.Remove(localPos);
                                poi.tilesPattern.Remove(localPos);
                                RefreshControls();
                            }
                        }
                    }
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.C)
                {
                    r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                    if (Physics.Raycast(r, out hit))
                    {
                        if (hit.collider.gameObject.name == "[WNC] ITC POI Tile")
                        {
                            EditorUtility.SetDirty(target);

                            Vector3 localPos = hit.collider.gameObject.transform.localPosition;
                            if (poi.connectorTiles.Contains(localPos))
                                poi.connectorTiles.Remove(localPos);
                            else
                                poi.connectorTiles.Add(localPos);
                            RefreshControls();
                        }
                    }
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.A)
                {
                    r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                    if (Physics.Raycast(r, out hit))
                    {
                        if (hit.collider.gameObject.name == "[WNC] ITC POI Tile Adder")
                        {
                            EditorUtility.SetDirty(target);

                            Vector3 localPos = hit.collider.gameObject.transform.localPosition * tileSize;
                            localPos.x += hit.collider.gameObject.transform.parent.localPosition.x;
                            localPos.y += hit.collider.gameObject.transform.parent.localPosition.y;
                            localPos.z += hit.collider.gameObject.transform.parent.localPosition.z;

                            if (e.button == 0)
                            {
                                if (!poi.tilesPattern.Contains(localPos))
                                {
                                    poi.tilesPattern.Add(localPos);
                                    RefreshControls();
                                }
                            }
                        }
                    }
                }
            }
        }
        public override void OnInspectorGUI()
        {
            POI poi = ((POI)target);

            if (poi.gameObject.scene.name == null)
            {
                poi.editMode = false;
                poi.lockGridVisibility = false;
                if (poi.controlsParent != null) DestroyImmediate(poi.controlsParent.gameObject);
            }
            else
            {
                if (poi.controlsParent != null)
                {
                    poi.controlsParent.position = poi.transform.position;
                    poi.controlsParent.rotation = poi.transform.rotation;
                }
            }

            Init();
            DrawBackground(((POI)target).editMode ? 360 : 170);

            Rect background = new Rect(0f, 0f, Screen.width, 68);
            Texture2D bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, new Color32(0, 0, 0, 255));
            bgTexture.Apply();
            GUI.DrawTexture(background, bgTexture);

            Texture buttonsTexture;
            buttonsTexture = Resources.Load<Texture>("WNC_ITC/Sprites/PointOfInterest");
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(buttonsTexture);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(255, 255, 255, 255), 3);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            GUILayout.FlexibleSpace();
            if (((POI)target).editMode) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            if (GUILayout.Button("Edit Mode", GUILayout.MaxWidth(230), GUILayout.MinHeight(24)))
            {
                poi.editMode = !poi.editMode;
            }
            GUI.backgroundColor = new Color32(255, 255, 255, 255);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            if(((POI)target).editMode)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(-10);
                GUILayout.Label("1) Open Scene tab to edit POI grid", s_SubDescriptionCentered);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Space(-10);
                GUILayout.Label("2) Point your mouse over the cube, and click:", s_SubDescriptionCentered);
                GUILayout.EndHorizontal();
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();
                buttonsTexture = Resources.Load<Texture>("WNC_ITC/Sprites/PoiCubeG");
                GUILayout.Label(buttonsTexture, s_DescriptionCentered);
                GUILayout.Space(3);
                GUILayout.Label("[A] - Add Tile", s_SubDescriptionCentered);
                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();
                buttonsTexture = Resources.Load<Texture>("WNC_ITC/Sprites/PoiCubeY");
                GUILayout.Label(buttonsTexture, s_DescriptionCentered);
                GUILayout.Space(3);
                GUILayout.Label("[D] - Delete Tile", s_SubDescriptionCentered);
                GUILayout.Space(3);
                GUILayout.Label("[W] - Up", s_SubDescriptionCentered);
                GUILayout.Space(3);
                GUILayout.Label("[S] - Down", s_SubDescriptionCentered);
                GUILayout.Space(3);
                GUILayout.Label("[C] - Connector", s_SubDescriptionCentered);
                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
            }

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            GUILayout.FlexibleSpace();
            if (((POI)target).lockGridVisibility) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            if (GUILayout.Button("Lock Grid Visibility", GUILayout.MaxWidth(230), GUILayout.MinHeight(24)))
            {
                poi.lockGridVisibility = !poi.lockGridVisibility;
            }
            GUI.backgroundColor = new Color32(255, 255, 255, 255);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
        }
        private Texture2D MakeTex(int width, int height, Color color)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
        static Texture2D ToTexture2D(Texture texture)
        {
            return Texture2D.CreateExternalTexture(
                texture.width,
                texture.height,
                TextureFormat.RGB24,
                false, false,
                texture.GetNativeTexturePtr());
        }
        void HorizontalLine(Color color, int size)
        {
            GUILayout.BeginHorizontal();
            s_Line.fixedHeight = size;
            var c = GUI.color;
            GUI.color = color;
            GUILayout.Box(GUIContent.none, s_Line);
            GUI.color = c;
            GUILayout.EndHorizontal();
        }
        void DrawBackground(int height)
        {
            Rect background = new Rect(0f, 0f, Screen.width, height);
            Texture2D bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, new Color32(30, 30, 30, 255));
            bgTexture.Apply();
            GUI.DrawTexture(background, bgTexture);
        }
    }
}