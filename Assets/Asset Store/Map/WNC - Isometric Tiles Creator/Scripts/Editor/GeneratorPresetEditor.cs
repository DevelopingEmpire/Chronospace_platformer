using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace WNC.ITC
{
    [CustomEditor(typeof(GeneratorPreset))]
    public class GeneratorPresetEditor : Editor
    {
        public static Generator mapForGenerator;
        GUIStyle style_HeaderButton;
        GUIStyle style_HeaderText;
        GUIStyle s_ButtonBigPicture;
        GUIStyle s_LabelCentered;
        GUIStyle s_LabelLeft;
        GUIStyle s_DescriptionCentered;

        GUIStyle s_Header;
        GUIStyle s_SubHeader;
        GUIStyle s_Label;
        GUIStyle s_Line;
        GUIStyle s_ButtonPH;
        GUIStyle s_ButtonSmall;
        GUIStyle s_IntInput;
        GUIStyle s_Popup;
        void InitializeStyles()
        {
            style_HeaderButton = new GUIStyle(GUI.skin.button);
            style_HeaderButton.fixedWidth = 24;
            style_HeaderButton.fixedHeight = 24;
            style_HeaderButton.padding = new RectOffset(2, 2, 2, 2);
            style_HeaderButton.alignment = TextAnchor.MiddleCenter;

            style_HeaderText = new GUIStyle();
            style_HeaderText.alignment = TextAnchor.MiddleCenter;
            style_HeaderText.padding = new RectOffset(0, 0, 0, 0);
            style_HeaderText.normal.textColor = Color.white;
            style_HeaderText.fontSize = 12;
            style_HeaderText.fontStyle = FontStyle.Bold;

            s_Line = new GUIStyle();
            s_Line.normal.background = EditorGUIUtility.whiteTexture;
            s_Line.margin = new RectOffset(0, 0, 0, 0);

            s_Header = new GUIStyle();
            s_Header.alignment = TextAnchor.MiddleCenter;
            s_Header.normal.textColor = Color.white;
            s_Header.fontSize = 22;
            s_Header.fontStyle = FontStyle.Bold;

            s_SubHeader = new GUIStyle();
            s_SubHeader.alignment = TextAnchor.MiddleCenter;
            s_SubHeader.padding = new RectOffset(0, 0, 0, 0);
            s_SubHeader.normal.textColor = Color.white;
            s_SubHeader.fontSize = 18;
            s_SubHeader.fontStyle = FontStyle.Bold;

            s_Label = new GUIStyle();
            s_Label.alignment = TextAnchor.MiddleLeft;
            s_Label.padding = new RectOffset(0, 0, 0, 0);
            s_Label.normal.textColor = new Color32(175, 175, 175, 255);
            s_Label.fontSize = 12;
            s_Label.fontStyle = FontStyle.Bold;

            s_LabelCentered = new GUIStyle();
            s_LabelCentered.alignment = TextAnchor.MiddleCenter;
            s_LabelCentered.normal.textColor = new Color32(175, 175, 175, 255);
            s_LabelCentered.fontSize = 10;
            s_LabelCentered.fontStyle = FontStyle.Bold;

            s_LabelLeft = new GUIStyle();
            s_LabelLeft.alignment = TextAnchor.MiddleLeft;
            s_LabelLeft.normal.textColor = new Color32(175, 175, 175, 255);
            s_LabelLeft.fontSize = 10;
            s_LabelLeft.fontStyle = FontStyle.Bold;

            s_DescriptionCentered = new GUIStyle();
            s_DescriptionCentered.alignment = TextAnchor.MiddleCenter;
            s_DescriptionCentered.normal.textColor = new Color32(175, 175, 175, 255);
            s_DescriptionCentered.fontSize = 6;
            s_DescriptionCentered.fontStyle = FontStyle.Bold;

            s_ButtonBigPicture = new GUIStyle(GUI.skin.button);
            s_ButtonBigPicture.padding = new RectOffset(3, 3, 3, 3);
            s_ButtonBigPicture.fixedWidth = 90f;
            s_ButtonBigPicture.fixedHeight = 90f;
            s_ButtonBigPicture.alignment = TextAnchor.MiddleCenter;

            s_ButtonPH = new GUIStyle(GUI.skin.button);
            s_ButtonPH.padding = new RectOffset(5, 5, 5, 5);
            s_ButtonPH.fixedWidth = 100f;
            s_ButtonPH.fixedHeight = 50f;
            s_ButtonPH.alignment = TextAnchor.MiddleCenter;

            s_ButtonSmall = new GUIStyle(GUI.skin.button);
            s_ButtonSmall.fixedWidth = 24;
            s_ButtonSmall.fixedHeight = 24;
            s_ButtonSmall.padding = new RectOffset(3, 3, 3, 3);
            s_ButtonSmall.alignment = TextAnchor.MiddleCenter;

            s_IntInput = new GUIStyle(GUI.skin.button);
            s_IntInput.padding = new RectOffset(0, 0, 0, 0);
            s_IntInput.fixedWidth = 40;
            s_IntInput.fixedHeight = 40;
            s_IntInput.fontSize = 14;
            s_IntInput.fontStyle = FontStyle.Bold;
            s_IntInput.alignment = TextAnchor.MiddleCenter;

            s_Popup = new GUIStyle(GUI.skin.button);
            s_Popup.stretchWidth = true;
            s_Popup.alignment = TextAnchor.MiddleCenter;
            s_Popup.normal.textColor = Color.white;
            s_Popup.hover.textColor = Color.white;
            s_Popup.focused.textColor = Color.white;
            s_Popup.active.textColor = Color.white;
        }
        public override void OnInspectorGUI()
        {
            InitializeStyles();

            Color32 backgroundColor = GUI.backgroundColor;

            Rect background = new Rect(0f, 0f, Screen.width, 999999f);
            Texture2D bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, new Color32(30, 30, 30, 255));
            bgTexture.Apply();
            GUI.DrawTexture(background, bgTexture);

            background = new Rect(0f, 0f, Screen.width, 68);
            bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, new Color32(0, 0, 0, 255));
            bgTexture.Apply();
            GUI.DrawTexture(background, bgTexture);

            Texture texture;

            texture = Resources.Load<Texture>("WNC_ITC/Sprites/GeneratorPreset");
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(texture);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(Color.white, 3);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);


            if (Settings.Instance.generatorTabInPreset)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(-15);
                GUILayout.FlexibleSpace();
                GUILayout.Label("GENERATOR", style_HeaderText);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Space(-15);
                HorizontalLine(new Color32(120, 120, 120, 255), 1);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(-15);
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                GUILayout.Space(5);
                GUILayout.Label("Select [Generator]", s_LabelCentered);
                mapForGenerator = (Generator)EditorGUILayout.ObjectField(mapForGenerator, typeof(Generator), GUILayout.MaxWidth(200), GUILayout.MinHeight(20));
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                GUILayout.Space(5);
                if (mapForGenerator != null) EditorGUI.BeginDisabledGroup(true);
                GUILayout.Label("Create [Generator]", s_LabelCentered);
                if (GUILayout.Button("Create [Generator]", GUILayout.MaxWidth(200), GUILayout.MinHeight(20)))
                {
                    Generator newMap = IsometricTilesCreator.CreateGenerator();
                    mapForGenerator = newMap;
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Space(-15);
                HorizontalLine(new Color32(120, 120, 120, 255), 1);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                GUILayout.Space(-15);
                GUILayout.FlexibleSpace();
                if (mapForGenerator == null) EditorGUI.BeginDisabledGroup(true);
                if (GUILayout.Button("Generate", GUILayout.MaxWidth(130), GUILayout.MinHeight(20)))
                {
                    mapForGenerator.generatorPreset = ((GeneratorPreset)target);
                    mapForGenerator.Generate();
                }
                if (GUILayout.Button("Refresh", GUILayout.MaxWidth(130), GUILayout.MinHeight(20)))
                {
                    mapForGenerator.Refresh();
                }
                if (GUILayout.Button("Clear", GUILayout.MaxWidth(130), GUILayout.MinHeight(20)))
                {
                    mapForGenerator.Clear();
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Space(-15);
                HorizontalLine(new Color32(120, 120, 120, 255), 3);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("SIZE", style_HeaderText);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 255), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label("WIDTH", s_LabelCentered, GUILayout.MaxWidth(100f));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            texture = Resources.Load<Texture>("WNC_ITC/Sprites/itc_width");
            GUILayout.Label(texture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(100));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            texture = EditorGUIUtility.isProSkin ? Resources.Load<Texture>("WNC_ITC/Sprites/IconFixedPro") : Resources.Load<Texture>("WNC_ITC/Sprites/IconFixed");
            if (!((GeneratorPreset)target).size_WidthRandom) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            if (GUILayout.Button(texture, style_HeaderButton))
            {
                ((GeneratorPreset)target).size_WidthRandom = false;
            }
            GUI.backgroundColor = backgroundColor;
            texture = EditorGUIUtility.isProSkin ? Resources.Load<Texture>("WNC_ITC/Sprites/IconRandomBigPro") : Resources.Load<Texture>("WNC_ITC/Sprites/IconRandomBig");
            if (((GeneratorPreset)target).size_WidthRandom) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            if (GUILayout.Button(texture, style_HeaderButton))
            {
                ((GeneratorPreset)target).size_WidthRandom = true;
            }
            GUI.backgroundColor = backgroundColor;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            if (!((GeneratorPreset)target).size_WidthRandom)
            {
                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).size_Width = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).size_Width, Settings.Instance.minMapSize, Settings.Instance.maxMapSize, GUILayout.MinWidth(70f));
                GUILayout.Label(((GeneratorPreset)target).size_Width.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).size_Width_Min = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).size_Width_Min, Settings.Instance.minMapSize, Settings.Instance.maxMapSize, GUILayout.MinWidth(40f));
                GUILayout.Label(((GeneratorPreset)target).size_Width_Min.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).size_Width_Max = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).size_Width_Max, Settings.Instance.minMapSize, Settings.Instance.maxMapSize, GUILayout.MinWidth(40f));
                GUILayout.Label(((GeneratorPreset)target).size_Width_Max.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label("LENGTH", s_LabelCentered, GUILayout.MaxWidth(100f));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            texture = Resources.Load<Texture>("WNC_ITC/Sprites/itc_length");
            GUILayout.Label(texture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(100));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            texture = EditorGUIUtility.isProSkin ? Resources.Load<Texture>("WNC_ITC/Sprites/IconFixedPro") : Resources.Load<Texture>("WNC_ITC/Sprites/IconFixed");
            if (!((GeneratorPreset)target).size_LengthRandom) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            if (GUILayout.Button(texture, style_HeaderButton))
            {
                ((GeneratorPreset)target).size_LengthRandom = false;
            }
            GUI.backgroundColor = backgroundColor;
            texture = EditorGUIUtility.isProSkin ? Resources.Load<Texture>("WNC_ITC/Sprites/IconRandomBigPro") : Resources.Load<Texture>("WNC_ITC/Sprites/IconRandomBig");
            if (((GeneratorPreset)target).size_LengthRandom) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            if (GUILayout.Button(texture, style_HeaderButton))
            {
                ((GeneratorPreset)target).size_LengthRandom = true;
            }
            GUI.backgroundColor = backgroundColor;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            if (!((GeneratorPreset)target).size_LengthRandom)
            {
                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).size_Length = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).size_Length, Settings.Instance.minMapSize, Settings.Instance.maxMapSize, GUILayout.MinWidth(70f));
                GUILayout.Label(((GeneratorPreset)target).size_Length.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).size_Length_Min = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).size_Length_Min, Settings.Instance.minMapSize, Settings.Instance.maxMapSize, GUILayout.MinWidth(40f));
                GUILayout.Label(((GeneratorPreset)target).size_Length_Min.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).size_Length_Max = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).size_Length_Max, Settings.Instance.minMapSize, Settings.Instance.maxMapSize, GUILayout.MinWidth(40f));
                GUILayout.Label(((GeneratorPreset)target).size_Length_Max.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 255), 3);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("HEIGHTS", style_HeaderText);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 255), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label("HEIGHT", s_LabelCentered, GUILayout.MaxWidth(100f));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            texture = Resources.Load<Texture>("WNC_ITC/Sprites/itc_height");
            GUILayout.Label(texture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(100));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            texture = EditorGUIUtility.isProSkin ? Resources.Load<Texture>("WNC_ITC/Sprites/IconFixedPro") : Resources.Load<Texture>("WNC_ITC/Sprites/IconFixed");
            if (!((GeneratorPreset)target).height_HeightRandom) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            if (GUILayout.Button(texture, style_HeaderButton))
            {
                ((GeneratorPreset)target).height_HeightRandom = false;
            }
            GUI.backgroundColor = backgroundColor;
            texture = EditorGUIUtility.isProSkin ? Resources.Load<Texture>("WNC_ITC/Sprites/IconRandomBigPro") : Resources.Load<Texture>("WNC_ITC/Sprites/IconRandomBig");
            if (((GeneratorPreset)target).height_HeightRandom) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            if (GUILayout.Button(texture, style_HeaderButton))
            {
                ((GeneratorPreset)target).height_HeightRandom = true;
            }
            GUI.backgroundColor = backgroundColor;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            if (!((GeneratorPreset)target).height_HeightRandom)
            {
                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).height_Height = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).height_Height, 0f, 30f, GUILayout.MinWidth(70f));
                GUILayout.Label(((GeneratorPreset)target).height_Height.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).height_Height_Min = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).height_Height_Min, 0f, 30f, GUILayout.MinWidth(70f));
                GUILayout.Label(((GeneratorPreset)target).height_Height_Min.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).height_Height_Max = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).height_Height_Max, 0f, 30f, GUILayout.MinWidth(70f));
                GUILayout.Label(((GeneratorPreset)target).height_Height_Max.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label("CONTRAST", s_LabelCentered, GUILayout.MaxWidth(100f));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            texture = Resources.Load<Texture>("WNC_ITC/Sprites/itc_contrast");
            GUILayout.Label(texture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(100));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            texture = EditorGUIUtility.isProSkin ? Resources.Load<Texture>("WNC_ITC/Sprites/IconFixedPro") : Resources.Load<Texture>("WNC_ITC/Sprites/IconFixed");
            if (!((GeneratorPreset)target).height_ContrastRandom) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            if (GUILayout.Button(texture, style_HeaderButton))
            {
                ((GeneratorPreset)target).height_ContrastRandom = false;
            }
            GUI.backgroundColor = backgroundColor;
            texture = EditorGUIUtility.isProSkin ? Resources.Load<Texture>("WNC_ITC/Sprites/IconRandomBigPro") : Resources.Load<Texture>("WNC_ITC/Sprites/IconRandomBig");
            if (((GeneratorPreset)target).height_ContrastRandom) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            if (GUILayout.Button(texture, style_HeaderButton))
            {
                ((GeneratorPreset)target).height_ContrastRandom = true;
            }
            GUI.backgroundColor = backgroundColor;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            if (!((GeneratorPreset)target).height_ContrastRandom)
            {
                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).height_Contrast = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).height_Contrast, 0f, 10f, GUILayout.MinWidth(70f));
                GUILayout.Label(((GeneratorPreset)target).height_Contrast.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).height_Contrast_Min = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).height_Contrast_Min, 0f, 10f, GUILayout.MinWidth(70f));
                GUILayout.Label(((GeneratorPreset)target).height_Contrast_Min.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                ((GeneratorPreset)target).height_Contrast_Max = (int)GUILayout.HorizontalSlider(((GeneratorPreset)target).height_Contrast_Max, 0f, 10f, GUILayout.MinWidth(70f));
                GUILayout.Label(((GeneratorPreset)target).height_Contrast_Max.ToString(), s_LabelCentered, GUILayout.MaxWidth(25));
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 255), 3);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("GLOBAL FILLING", style_HeaderText);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 255), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            GUILayout.FlexibleSpace();

            texture = Resources.Load<Texture>("WNC_ITC/Sprites/itc_pois");
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label("      POI", s_LabelCentered, GUILayout.MaxWidth(100f));
            GUILayout.Label(texture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(100));
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label("List of [POI] prefabs", s_LabelLeft, GUILayout.MinWidth(300f));
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(18);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("filling_pois"), true); // True means show children
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label("Chance to spawn [POI]", s_LabelLeft, GUILayout.MinWidth(300f));
            GUILayout.EndHorizontal();
            ((GeneratorPreset)target).filling_poisChance = GUILayout.HorizontalSlider(((GeneratorPreset)target).filling_poisChance, 0f, 1f, GUILayout.MaxWidth(300f));
            GUILayout.Space(20);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 255), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            GUILayout.FlexibleSpace();

            texture = Resources.Load<Texture>("WNC_ITC/Sprites/itc_roads");
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label("    ROADS", s_LabelCentered, GUILayout.MaxWidth(100f));
            GUILayout.Label(texture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(100));
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label("Chance to spawn road between [POI]'s", s_LabelLeft, GUILayout.MinWidth(300f));
            GUILayout.EndHorizontal();
            ((GeneratorPreset)target).filling_roadsChance = GUILayout.HorizontalSlider(((GeneratorPreset)target).filling_roadsChance, 0f, 1f, GUILayout.MaxWidth(300f));
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label("How much road prefabs will be spawned", s_LabelLeft, GUILayout.MinWidth(300f));
            GUILayout.EndHorizontal();
            ((GeneratorPreset)target).filling_roadsFilling = GUILayout.HorizontalSlider(((GeneratorPreset)target).filling_roadsFilling, 0f, 1f, GUILayout.MaxWidth(300f));
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label("Chance to spawn fence at roads", s_LabelLeft, GUILayout.MinWidth(300f));
            GUILayout.EndHorizontal();
            ((GeneratorPreset)target).filling_roadsFenceChance = GUILayout.HorizontalSlider(((GeneratorPreset)target).filling_roadsFenceChance, 0f, 1f, GUILayout.MaxWidth(300f));
            GUILayout.Space(20);

            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 255), 3);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("BIOMES MIXING", style_HeaderText);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 255), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            texture = Resources.Load<Texture>("WNC_ITC/Sprites/itc_naturally");
            if (((GeneratorPreset)target).mixingType == 0) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            else GUI.backgroundColor = new Color32(255, 155, 0, 0);
            if (GUILayout.Button(texture, s_ButtonPH))
            {
                ((GeneratorPreset)target).mixingType = 0;
            }
            GUI.backgroundColor = new Color32(255, 255, 255, 255);
            GUILayout.Label("NATURALLY", s_LabelCentered);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            texture = Resources.Load<Texture>("WNC_ITC/Sprites/itc_zones");
            if (((GeneratorPreset)target).mixingType == 1) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            else GUI.backgroundColor = new Color32(255, 155, 0, 0);
            if (GUILayout.Button(texture, s_ButtonPH))
            {
                ((GeneratorPreset)target).mixingType = 1;
            }
            GUI.backgroundColor = new Color32(255, 255, 255, 255);
            GUILayout.Label("ZONES", s_LabelCentered);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            texture = Resources.Load<Texture>("WNC_ITC/Sprites/itc_solid");
            if (((GeneratorPreset)target).mixingType == 2) GUI.backgroundColor = new Color32(255, 155, 0, 255);
            else GUI.backgroundColor = new Color32(255, 155, 0, 0);
            if (GUILayout.Button(texture, s_ButtonPH))
            {
                ((GeneratorPreset)target).mixingType = 2;
            }
            GUI.backgroundColor = new Color32(255, 255, 255, 255);
            GUILayout.Label("SOLID", s_LabelCentered);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 255), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            List<string> setNames = new List<string>();
            List<string> waterSetNames = new List<string>();
            for (int i = 0; i < Settings.Instance.biomes.Count; i++)
            {
                if (!Settings.Instance.biomes[i].isWaterBiome) setNames.Add(Settings.Instance.biomes[i].setName);
                else waterSetNames.Add(Settings.Instance.biomes[i].setName);
            }
            if (((GeneratorPreset)target).mixingType == 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(-15);
                GUILayout.FlexibleSpace();
                for (int i = 0; i < 4; i++)
                {
                    GUILayout.BeginVertical();

                    GUILayout.Label("BIOME " + (i + 1), s_LabelCentered);

                    GUI.backgroundColor = new Color32(255, 155, 0, 0);

                    if (i != 1)
                    {
                        ((GeneratorPreset)target).biomes[i] = setNames[EditorGUILayout.Popup(setNames.IndexOf(((GeneratorPreset)target).biomes[i]), setNames.ToArray(), s_Popup, GUILayout.MaxWidth(100f))];
                    }
                    else if (i == 1 && !Settings.Instance.scalableWater)
                    {
                        ((GeneratorPreset)target).biomes[i] = waterSetNames[EditorGUILayout.Popup(waterSetNames.IndexOf(((GeneratorPreset)target).biomes[i]), waterSetNames.ToArray(), s_Popup, GUILayout.MaxWidth(100f))];
                    }
                    else
                    {
                        EditorGUILayout.LabelField("!SETTINGS", s_Popup, GUILayout.MaxWidth(100f));
                    }

                    GUI.backgroundColor = new Color32(255, 255, 255, 255);

                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    texture = Resources.Load<Texture>("WNC_ITC/Sprites/itc_biome_" + i.ToString());
                    GUILayout.Label(texture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(100));
                    GUILayout.EndHorizontal();

                    if (i == 0)
                    {

                    }
                    else if (i == 1)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(5f);
                        GUILayout.Label("HEIGHT LEVEL", s_LabelCentered, GUILayout.MaxWidth(100f));
                        GUILayout.EndHorizontal();
                        GUILayout.Space(-5f);
                        ((GeneratorPreset)target).biome_water_seaLevel = GUILayout.HorizontalSlider(((GeneratorPreset)target).biome_water_seaLevel, 0f, 0.4f, GUILayout.MaxWidth(100f));
                        GUILayout.Space(12f);
                    }
                    else if (i == 2)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(5f);
                        GUILayout.Label("THICKNESS", s_LabelCentered, GUILayout.MaxWidth(100f));
                        GUILayout.EndHorizontal();
                        GUILayout.Space(-5f);
                        ((GeneratorPreset)target).biome_sands_thickness = GUILayout.HorizontalSlider(((GeneratorPreset)target).biome_sands_thickness, 0f, 1f, GUILayout.MaxWidth(100f));
                        GUILayout.Space(12f);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(5f);
                        GUILayout.Label("SMOOTHNESS", s_LabelCentered, GUILayout.MaxWidth(100f));
                        GUILayout.EndHorizontal();
                        GUILayout.Space(-5f);
                        ((GeneratorPreset)target).biome_sands_smoothness = GUILayout.HorizontalSlider(((GeneratorPreset)target).biome_sands_smoothness, 0f, 1f, GUILayout.MaxWidth(100f));
                        GUILayout.Space(12f);
                    }
                    else if (i == 3)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(5f);
                        GUILayout.Label("HEIGHT LEVEL", s_LabelCentered, GUILayout.MaxWidth(100f));
                        GUILayout.EndHorizontal();
                        GUILayout.Space(-5f);
                        ((GeneratorPreset)target).biome_snow_snowLevel = GUILayout.HorizontalSlider(((GeneratorPreset)target).biome_snow_snowLevel, 0f, 0.4f, GUILayout.MaxWidth(100f));
                        GUILayout.Space(12f);
                    }

                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();
                }

                GUILayout.EndHorizontal();
            }
            else if (((GeneratorPreset)target).mixingType == 1)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();
                GUILayout.Space(7);
                GUILayout.Label("CHOOSE BIOMES", s_LabelCentered);
                ((GeneratorPreset)target).zones_biomes = EditorGUILayout.MaskField(((GeneratorPreset)target).zones_biomes, setNames.ToArray(), GUILayout.MaxWidth(100));

                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();
                GUILayout.Label("ZONES COUNT", s_LabelCentered);
                texture = EditorGUIUtility.isProSkin ? Resources.Load<Texture>("WNC_ITC/Sprites/IconFixedPro") : Resources.Load<Texture>("WNC_ITC/Sprites/IconFixed");
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                if (!((GeneratorPreset)target).zones_Random) GUI.backgroundColor = new Color32(255, 155, 0, 255);
                if (GUILayout.Button(new GUIContent(texture), s_ButtonSmall))
                {
                    ((GeneratorPreset)target).zones_Random = false;
                }
                GUI.backgroundColor = new Color32(255, 255, 255, 255);

                texture = EditorGUIUtility.isProSkin ? Resources.Load<Texture>("WNC_ITC/Sprites/IconRandomBigPro") : Resources.Load<Texture>("WNC_ITC/Sprites/IconBigRandom");
                if (((GeneratorPreset)target).zones_Random) GUI.backgroundColor = new Color32(255, 155, 0, 255);
                if (GUILayout.Button(new GUIContent(texture), s_ButtonSmall))
                {
                    ((GeneratorPreset)target).zones_Random = true;
                }
                GUI.backgroundColor = new Color32(255, 255, 255, 255);
                GUILayout.EndVertical();

                if (!((GeneratorPreset)target).zones_Random)
                {
                    GUILayout.BeginVertical();
                    int.TryParse(GUILayout.TextField(((GeneratorPreset)target).zones.ToString(), s_IntInput), out ((GeneratorPreset)target).zones);
                    GUILayout.Label("FIXED", s_LabelCentered);
                    GUILayout.EndVertical();
                }
                else
                {
                    GUILayout.BeginVertical();
                    int.TryParse(GUILayout.TextField(((GeneratorPreset)target).zones_Min.ToString(), s_IntInput), out ((GeneratorPreset)target).zones_Min);
                    GUILayout.Label("MIN", s_LabelCentered);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                    int.TryParse(GUILayout.TextField(((GeneratorPreset)target).zones_Max.ToString(), s_IntInput), out ((GeneratorPreset)target).zones_Max);
                    GUILayout.Label("MAX", s_LabelCentered);
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();
                GUILayout.Space(7);
                GUILayout.Label("WATER BIOME", s_LabelCentered);
                if (!Settings.Instance.scalableWater)
                    ((GeneratorPreset)target).zones_water = waterSetNames[EditorGUILayout.Popup(waterSetNames.IndexOf(((GeneratorPreset)target).zones_water), waterSetNames.ToArray(), GUILayout.MaxWidth(100))];
                else
                    EditorGUILayout.LabelField("!SETTINGS", s_Popup, GUILayout.MaxWidth(100f));
                GUILayout.Label("SEA LEVEL", s_LabelCentered);
                GUILayout.Space(-5f);
                ((GeneratorPreset)target).zones_water_level = GUILayout.HorizontalSlider(((GeneratorPreset)target).zones_water_level, 0f, 0.4f, GUILayout.MaxWidth(100));
                GUILayout.Space(12f);
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else if (((GeneratorPreset)target).mixingType == 2)
            {

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();
                GUILayout.Space(7);
                GUILayout.Label("SOLID BIOME", s_LabelCentered);
                ((GeneratorPreset)target).solid_biome = setNames[EditorGUILayout.Popup(setNames.IndexOf(((GeneratorPreset)target).solid_biome), setNames.ToArray(), GUILayout.MaxWidth(100))];
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();
                GUILayout.Space(7);
                GUILayout.Label("WATER BIOME", s_LabelCentered);

                if (!Settings.Instance.scalableWater)
                    ((GeneratorPreset)target).solid_water = waterSetNames[EditorGUILayout.Popup(waterSetNames.IndexOf(((GeneratorPreset)target).solid_water), waterSetNames.ToArray(), GUILayout.MaxWidth(100))];
                else
                    EditorGUILayout.LabelField("!SETTINGS", s_Popup, GUILayout.MaxWidth(100f));

                GUILayout.Label("SEA LEVEL", s_LabelCentered);
                GUILayout.Space(-5f);
                ((GeneratorPreset)target).solid_water_level = GUILayout.HorizontalSlider(((GeneratorPreset)target).solid_water_level, 0f, 0.4f, GUILayout.MaxWidth(100));
                GUILayout.Space(12f);
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(2);
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
        private void OnDisable()
        {
            AssetDatabase.SaveAssetIfDirty(target);
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