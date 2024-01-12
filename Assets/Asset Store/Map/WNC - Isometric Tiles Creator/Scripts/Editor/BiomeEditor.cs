using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WNC.ITC
{
    [CustomEditor(typeof(Biome))]
    public class BiomeEditor : Editor
    {
        GUIStyle s_Line;
        GUIStyle s_Header;
        GUIStyle s_SubDescriptionCentered;
        private void OnEnable()
        {
            Init();
        }
        private void OnDisable()
        {
            AssetDatabase.SaveAssetIfDirty(target);
        }
        void Init()
        {
            s_Line = new GUIStyle();
            s_Line.normal.background = EditorGUIUtility.whiteTexture;
            s_Line.margin = new RectOffset(0, 0, 0, 0);

            s_Header = new GUIStyle();
            s_Header.alignment = TextAnchor.MiddleCenter;
            s_Header.padding = new RectOffset(0, 0, 0, 0);
            s_Header.normal.textColor = Color.white;
            s_Header.fontSize = 12;
            s_Header.fontStyle = FontStyle.Bold;

            s_SubDescriptionCentered = new GUIStyle();
            s_SubDescriptionCentered.alignment = TextAnchor.MiddleCenter;
            s_SubDescriptionCentered.normal.textColor = new Color32(175, 175, 175, 255);
            s_SubDescriptionCentered.fontSize = 10;
            s_SubDescriptionCentered.fontStyle = FontStyle.Bold;
        }
        public override void OnInspectorGUI()
        {
            Rect background = new Rect(0f, 0f, Screen.width, Screen.height * 2f);
            Texture2D bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, new Color32(30, 30, 30, 255));
            bgTexture.Apply();
            GUI.DrawTexture(background, bgTexture);

            background = new Rect(0f, 0f, Screen.width, 68);
            bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, new Color32(0, 0, 0, 255));
            bgTexture.Apply();
            GUI.DrawTexture(background, bgTexture);

            Texture buttonsTexture;

            buttonsTexture = Resources.Load<Texture>("WNC_ITC/Sprites/Biome");
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

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-5);
            GUILayout.BeginVertical();
            GUILayout.Label("BIOME NAME", s_Header);
            GUILayout.Label("Please, be sure that name of biome is unique", s_SubDescriptionCentered);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("setName"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isWaterBiome"), true);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-5);
            GUILayout.BeginVertical();
            GUILayout.Label("PARTS OF TILE", s_Header);
            GUILayout.Label("Tile combines by parts, if some list" + System.Environment.NewLine + "will be empty - this part will not spawn", s_SubDescriptionCentered);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("topParts"), true);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("decorParts"), true);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("downParts"), true);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 3);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-5);
            GUILayout.BeginVertical();
            GUILayout.Label("POINTS OF INTEREST", s_Header);
            GUILayout.Label("POI's which be spawned on this biome", s_SubDescriptionCentered);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pois"), true);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 3);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-5);
            GUILayout.BeginVertical();
            GUILayout.Label("ROADS", s_Header);
            GUILayout.Label("Select [Road Preset] for this [Biome]", s_SubDescriptionCentered);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("roadPreset"), true);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 3);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginVertical();
            GUILayout.Label("FILLING", s_Header);

            GUILayout.Space(5);
            GUILayout.Label("% of this biome tiles on [Generator] which be filled", s_SubDescriptionCentered);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-5);
            GUILayout.FlexibleSpace();
            ((Biome)target).filling = GUILayout.HorizontalSlider(((Biome)target).filling, 0f, 1f, GUILayout.MaxWidth(200f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(15);
            GUILayout.Label("Fill with tree chance", s_SubDescriptionCentered);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-10);
            GUILayout.FlexibleSpace();
            ((Biome)target).treesChance = GUILayout.HorizontalSlider(((Biome)target).treesChance, 0f, 1f, GUILayout.MaxWidth(200f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(15);
            GUILayout.Label("Fill with decor chance", s_SubDescriptionCentered);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-10);
            GUILayout.FlexibleSpace();
            ((Biome)target).decorsChance = GUILayout.HorizontalSlider(((Biome)target).decorsChance, 0f, 1f, GUILayout.MaxWidth(200f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(15);
            GUILayout.Label("Fill with unique chance", s_SubDescriptionCentered);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-10);
            GUILayout.FlexibleSpace();
            ((Biome)target).uniquesChance = GUILayout.HorizontalSlider(((Biome)target).uniquesChance, 0f, 1f, GUILayout.MaxWidth(200f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(15);
            GUILayout.Label("Chance to spawn [POI]", s_SubDescriptionCentered);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-10);
            GUILayout.FlexibleSpace();
            ((Biome)target).poisChance = GUILayout.HorizontalSlider(((Biome)target).poisChance, 0f, 1f, GUILayout.MaxWidth(200f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.Space(15);

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.Space(-5);
            GUILayout.BeginVertical();
            GUILayout.Label("TREES", s_Header);
            GUILayout.Label("Tree prefabs and min/max scale", s_SubDescriptionCentered);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("trees"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("treesMinScale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("treesMaxScale"));
            GUILayout.EndVertical();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-5);
            GUILayout.BeginVertical();
            GUILayout.Label("DECORS", s_Header);
            GUILayout.Label("Decor prefabs and min/max scale", s_SubDescriptionCentered);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("decors"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("decorMinScale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("decorMaxScale"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-5);
            GUILayout.BeginVertical();
            GUILayout.Label("UNIQUES", s_Header);
            GUILayout.Label("Unique prefabs and min/max scale", s_SubDescriptionCentered);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("uniques"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("uniquesMinScale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("uniquesMaxScale"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
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
    }
}