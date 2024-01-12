using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WNC.ITC
{
    [CustomEditor(typeof(RoadPreset))]
    public class RoadPresetEditor : Editor
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
            s_Header.fontSize = 18;
            s_Header.fontStyle = FontStyle.Bold;

            s_SubDescriptionCentered = new GUIStyle();
            s_SubDescriptionCentered.alignment = TextAnchor.MiddleCenter;
            s_SubDescriptionCentered.normal.textColor = new Color32(175, 175, 175, 255);
            s_SubDescriptionCentered.fontSize = 10;
            s_SubDescriptionCentered.fontStyle = FontStyle.Bold;
        }
        public override void OnInspectorGUI()
        {
            Rect background = new Rect(0f, 0f, Screen.width, Screen.height);
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

            buttonsTexture = Resources.Load<Texture>("WNC_ITC/Sprites/RoadPreset");
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
            GUILayout.Space(-10);
            GUILayout.BeginVertical();
            GUILayout.Label("Select Prefabs for [Road Preset]", s_SubDescriptionCentered);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("straight"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("turn"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ending"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tripleCrossroad"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crossroad"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fences"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ladders"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalLadders"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bridges"), true);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            EditorUtility.SetDirty(target);
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