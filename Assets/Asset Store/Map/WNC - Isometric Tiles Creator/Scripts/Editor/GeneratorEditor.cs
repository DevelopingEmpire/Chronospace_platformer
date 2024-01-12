using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WNC.ITC
{
    [CustomEditor(typeof(Generator))]
    public class GeneratorEditor : Editor
    {
        GUIStyle s_Line;
        GUIStyle s_Header;
        GUIStyle s_SubDescriptionCentered;
        private void OnEnable()
        {
            Init();
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
            Rect background = new Rect(0f, 0f, Screen.width, 200);
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

            buttonsTexture = Resources.Load<Texture>("WNC_ITC/Sprites/Generator");
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
            GUILayout.Label("[ITC] Generator Preset", s_Header);
            GUILayout.Label("Choose Generator Preset to generate", s_SubDescriptionCentered);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("generatorPreset"), true);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-14);
            GUILayout.FlexibleSpace();
            if (((Generator)target).generatorPreset == null)
                EditorGUI.BeginDisabledGroup(true);
            if (GUILayout.Button("Generate", GUILayout.MaxWidth(75), GUILayout.MinHeight(24)))
            {
                ((Generator)target).Generate();
            }
            EditorGUI.EndDisabledGroup();

            if (((Generator)target).generatorPreset == null)
                EditorGUI.BeginDisabledGroup(true);
            if (GUILayout.Button("Refresh", GUILayout.MaxWidth(74), GUILayout.MinHeight(24)))
            {
                ((Generator)target).Refresh();
            }
            EditorGUI.EndDisabledGroup();

            if (((Generator)target).generatorPreset == null && !((Generator)target).mapGenerated)
                EditorGUI.BeginDisabledGroup(true);
            if (GUILayout.Button("Clear", GUILayout.MaxWidth(74), GUILayout.MinHeight(24)))
            {
                ((Generator)target).Clear();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();


            if (((Generator)target).generatorPreset == null || ((Generator)target).mapOffset.x == 0 && ((Generator)target).mapOffset.y == 0)
                EditorGUI.BeginDisabledGroup(true);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-14);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("-X", GUILayout.MaxWidth(55), GUILayout.MinHeight(24)))
            {
                ((Generator)target).GenerateLeft();
            }
            if (GUILayout.Button("X+", GUILayout.MaxWidth(55), GUILayout.MinHeight(24)))
            {
                ((Generator)target).GenerateRight();
            }
            if (GUILayout.Button("-Z", GUILayout.MaxWidth(55), GUILayout.MinHeight(24)))
            {
                ((Generator)target).GenerateDown();
            }
            if (GUILayout.Button("Z+", GUILayout.MaxWidth(55), GUILayout.MinHeight(24)))
            {
                ((Generator)target).GenerateUp();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();

            if (((Generator)target).mapGenerated)
            {
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Space(-15);
                HorizontalLine(new Color32(255, 255, 255, 255), 3);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                GUILayout.Space(-5);
                GUILayout.BeginVertical();
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.Label("INFO", s_Header);
                GUILayout.Label("The variables of this generated map " + System.Environment.NewLine + "that you may need for further use", s_SubDescriptionCentered);
                Color32 bgColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color32(120, 120, 120, 120);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("mapSize"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("mapOffset"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pointsOfInterest"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("trees"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("decors"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("uniques"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("waterTiles"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("tiles"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("roads"), true);
                GUI.backgroundColor = bgColor;
                EditorGUI.EndDisabledGroup();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
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