using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WNC.ITC
{
    public class IsometricTilesCreatorMenu : EditorWindow
    {
        GUIStyle s_ButtonBigPicture;
        GUIStyle s_LabelCentered;
        GUIStyle s_ButtonCreator;

        GUIStyle s_Header;
        GUIStyle s_SubHeader;
        GUIStyle s_Label;
        GUIStyle s_Line;
        GUIStyle s_ButtonPH;
        GUIStyle s_ButtonSmall;
        GUIStyle s_ButtonArrow;
        GUIStyle s_IntInput;
        GUIStyle s_IntInputSmall;
        GUIStyle s_Toggle;
        GUIStyle s_ToggleSmall;
        GUIStyle s_ToggleColor;
        GUIStyle s_Popup;
        GUIStyle s_SelectedStyle;
        GUIStyle s_PageSelector;
        GUIStyle s_ButtonSubHeader;
        GUIStyle s_DescriptionLabel;
        GUIStyle s_ObjectField;
        GUIStyle s_SubDescriptionCentered;

        [MenuItem("Tools/Wand and Circles/[Isometric Tiles Creator]")]
        public static void Open_ITCMenu()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(IsometricTilesCreatorMenu));
            var texture = Resources.Load<Texture>("WNC_ITC/Sprites/LogoWhiteSmall");
            window.titleContent = new GUIContent("Isometric Tiles Creator", texture);
            window.maxSize = new Vector2(250, 435);
            window.minSize = new Vector2(250, 435);
        }
        void OnGUI()
        {
            InitializeStyles();
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

            buttonsTexture = Resources.Load<Texture>("WNC_ITC/Sprites/IsometricTilesCreator");
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
            GUILayout.FlexibleSpace();
            buttonsTexture = Resources.Load<Texture>("WNC_ITC/Sprites/IconYoutubePro");
            if (GUILayout.Button(new GUIContent("FAQ", buttonsTexture), GUILayout.MaxWidth(80), GUILayout.MaxHeight(32)))
            {
                Application.OpenURL("https://youtu.be/RFJq1yoPMkY");
            }
            buttonsTexture = Resources.Load<Texture>("WNC_ITC/Sprites/IconPdfPro");
            if (GUILayout.Button(new GUIContent("FAQ", buttonsTexture), GUILayout.MaxWidth(80), GUILayout.MaxHeight(32)))
            {
                if (AssetDatabase.IsValidFolder("Assets/Wand and Circles/WNC - Isometric Tiles Creator/Documentation"))
                {
                    Object settingsAsset = AssetDatabase.LoadAssetAtPath("Assets/Wand and Circles/WNC - Isometric Tiles Creator/Documentation/WNC - Isometric Tiles Creator [Documentation].pdf", typeof(Object));
                    Selection.activeObject = settingsAsset;
                }
                else
                {
                    Debug.LogWarning("[Wand and Circles - Isometric Tiles Creator] - Can't find documentation by path: Assets/Wand and Circles/WNC - Isometric Tiles Creator/Documentation/WNC - Isometric Tiles Creator [Documentation].pdf");
                }
            }
            buttonsTexture = Resources.Load<Texture>("WNC_ITC/Sprites/IconUnityPro");
            if (GUILayout.Button(new GUIContent("FAQ", buttonsTexture), GUILayout.MaxWidth(80), GUILayout.MaxHeight(32)))
            {
                FAQ.ShowWindow();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.Label("SCENE", s_LabelCentered);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create [Generator] on scene", GUILayout.MaxWidth(220), GUILayout.MinHeight(24)))
            {
                IsometricTilesCreator.CreateGenerator();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create [POI] on scene", GUILayout.MaxWidth(220), GUILayout.MinHeight(24)))
            {
                IsometricTilesCreator.CreatePOI();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.Label("PRESETS", s_LabelCentered);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create new [Generator Preset]", GUILayout.MaxWidth(220), GUILayout.MinHeight(24)))
            {
                if (AssetDatabase.IsValidFolder("Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Generator Presets"))
                {
                    int id = 0;
                    while(true)
                    {
                        if ((GeneratorPreset)AssetDatabase.LoadAssetAtPath("Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Generator Presets/[ITC] Generator Preset #" + id.ToString() + ".asset", typeof(GeneratorPreset)) == null) break;
                        else id++;
                    }
                    IsometricTilesCreator.CreateGeneratorPreset("Generator Preset #" + id.ToString());
                }
                else
                {
                    Debug.LogWarning("[Wand and Circles - Isometric Tiles Creator] - Can't create new Generator Preset because Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Generator Presets folder doesn't exist");
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create new [Biome]", GUILayout.MaxWidth(220), GUILayout.MinHeight(24)))
            {
                if (AssetDatabase.IsValidFolder("Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Biomes"))
                {
                    int id = 0;
                    while (true)
                    {
                        if (AssetDatabase.LoadAssetAtPath("Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Biomes/[Biome] Biome #" + id.ToString() + ".asset", typeof(Biome)) == null) break;
                        else id++;
                    }
                    IsometricTilesCreator.CreateBiome("Biome #" + id.ToString());
                }
                else
                {
                    Debug.LogWarning("[Wand and Circles - Isometric Tiles Creator] - Can't create new Biome because Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Biomes folder doesn't exist");
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create new [Road Preset]", GUILayout.MaxWidth(220), GUILayout.MinHeight(24)))
            {
                if (AssetDatabase.IsValidFolder("Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Road Presets"))
                {
                    int id = 0;
                    while (true)
                    {
                        if (AssetDatabase.LoadAssetAtPath("Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Road Presets/[Road] Road Preset #" + id.ToString() + ".asset", typeof(RoadPreset)) == null) break;
                        else id++;
                    }
                    IsometricTilesCreator.CreateRoadPreset("Road Preset #" + id.ToString());
                }
                else
                {
                    Debug.LogWarning("[Wand and Circles - Isometric Tiles Creator] - Can't create new RoadPreset because Assets/Wand and Circles/WNC - Isometric Tiles Creator/Presets/Road Presets folder doesn't exist");
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.Label("SETTINGS", s_LabelCentered);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Select [Settings]", GUILayout.MaxWidth(220), GUILayout.MinHeight(24)))
            {
                if (AssetDatabase.IsValidFolder("Assets/Wand and Circles/WNC - Isometric Tiles Creator/Editor/Resources/WNC_ITC/Settings"))
                {
                    Object settingsAsset = AssetDatabase.LoadAssetAtPath("Assets/Wand and Circles/WNC - Isometric Tiles Creator/Editor/Resources/WNC_ITC/Settings/Settings.asset", typeof(Settings));
                    Selection.activeObject = settingsAsset;
                }
                else
                {
                    Debug.LogWarning("[Wand and Circles - Isometric Tiles Creator] - Can't find settings by path: Assets/Wand and Circles/WNC - Isometric Tiles Creator/Editor/Resources/WNC_ITC/Settings/Settings.asset");
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.Label("WAND AND CIRCLES", s_LabelCentered);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("WANDANDCIRCLES.COM", GUILayout.MaxWidth(220), GUILayout.MinHeight(24)))
            {
                Application.OpenURL("https://wandandcircles.com/");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("ASSET STORE PAGE", GUILayout.MaxWidth(220), GUILayout.MinHeight(24)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/slug/243843");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        void DrawBackground()
        {
            Rect background = new Rect(0f, 0f, Screen.width, 10000f);
            Texture2D bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, new Color32(30, 30, 30, 255));
            bgTexture.Apply();
            GUI.DrawTexture(background, bgTexture);
        }
        void DrawHead()
        {
            Texture2D whiteTexture = new Texture2D(1, 1);
            whiteTexture.SetPixel(0, 0, Color.white);
            whiteTexture.Apply();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            Texture buttonsTexture;
            buttonsTexture = Resources.Load<Texture>("WND_LogoWhiteSmall");
            GUILayout.FlexibleSpace();
            GUILayout.Label(buttonsTexture);
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Space(6);
            GUILayout.Label("WNC - Isometric Tiles Creator", s_Header);
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15f);
            HorizontalLine(Color.white, 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(1);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15f);
            HorizontalLine(Color.white, 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(1);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15f);
            HorizontalLine(Color.white, 1);
            GUILayout.EndHorizontal();
        }
        void InitializeStyles()
        {
            s_SubDescriptionCentered = new GUIStyle();
            s_SubDescriptionCentered.alignment = TextAnchor.MiddleCenter;
            s_SubDescriptionCentered.normal.textColor = new Color32(175, 175, 175, 255);
            s_SubDescriptionCentered.fontSize = 10;
            s_SubDescriptionCentered.fontStyle = FontStyle.Bold;

            s_Line = new GUIStyle();
            s_Line.normal.background = EditorGUIUtility.whiteTexture;
            s_Line.margin = new RectOffset(0, 0, 0, 0);


            s_Header = new GUIStyle();
            s_Header.alignment = TextAnchor.MiddleCenter;
            s_Header.padding = new RectOffset(0, 0, 0, 0);
            s_Header.normal.textColor = Color.white;
            s_Header.fontSize = 12;
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
            s_LabelCentered.fontSize = 12;
            s_LabelCentered.fontStyle = FontStyle.Bold;

            s_DescriptionLabel = new GUIStyle();
            s_DescriptionLabel.alignment = TextAnchor.MiddleLeft;
            s_DescriptionLabel.padding = new RectOffset(0, 0, 0, 0);
            s_DescriptionLabel.normal.textColor = new Color32(200, 200, 200, 255);
            s_DescriptionLabel.fontSize = 10;
            s_DescriptionLabel.fontStyle = FontStyle.Italic;

            s_ButtonBigPicture = new GUIStyle(GUI.skin.button);
            s_ButtonBigPicture.padding = new RectOffset(3, 3, 3, 3);
            s_ButtonBigPicture.fixedWidth = 90f;
            s_ButtonBigPicture.fixedHeight = 90f;
            s_ButtonBigPicture.alignment = TextAnchor.MiddleCenter;

            s_ButtonPH = new GUIStyle(GUI.skin.button);
            s_ButtonPH.padding = new RectOffset(5, 5, 5, 5);
            s_ButtonPH.fixedWidth = 90f;
            s_ButtonPH.fixedHeight = 64f;
            s_ButtonPH.alignment = TextAnchor.MiddleCenter;

            s_ButtonSmall = new GUIStyle(GUI.skin.button);
            s_ButtonSmall.fixedWidth = 30;
            s_ButtonSmall.fixedHeight = 30;
            s_ButtonSmall.padding = new RectOffset(3, 3, 3, 3);
            s_ButtonSmall.alignment = TextAnchor.MiddleCenter;

            s_ButtonCreator = new GUIStyle(GUI.skin.button);
            s_ButtonCreator.fixedWidth = 250;
            s_ButtonCreator.fixedHeight = 40;
            s_ButtonCreator.padding = new RectOffset(3, 3, 3, 3);
            s_ButtonCreator.alignment = TextAnchor.MiddleCenter;

            s_ButtonSubHeader = new GUIStyle(GUI.skin.button);
            s_ButtonSubHeader.fixedWidth = 26;
            s_ButtonSubHeader.fixedHeight = 26;
            s_ButtonSubHeader.padding = new RectOffset(3, 3, 3, 3);
            s_ButtonSubHeader.alignment = TextAnchor.MiddleCenter;

            s_ButtonArrow = new GUIStyle(GUI.skin.button);
            s_ButtonArrow.fixedWidth = 15;
            s_ButtonArrow.fixedHeight = 18;
            s_ButtonArrow.alignment = TextAnchor.MiddleCenter;

            s_IntInput = new GUIStyle(GUI.skin.button);
            s_IntInput.padding = new RectOffset(0, 0, 0, 0);
            s_IntInput.fixedWidth = 40;
            s_IntInput.fixedHeight = 40;
            s_IntInput.fontSize = 14;
            s_IntInput.fontStyle = FontStyle.Bold;
            s_IntInput.alignment = TextAnchor.MiddleCenter;

            s_IntInputSmall = new GUIStyle(GUI.skin.button);
            s_IntInputSmall.padding = new RectOffset(0, 0, 0, 0);
            s_IntInputSmall.fixedWidth = 26;
            s_IntInputSmall.fixedHeight = 26;
            s_IntInputSmall.fontSize = 12;
            s_IntInputSmall.fontStyle = FontStyle.Bold;
            s_IntInputSmall.alignment = TextAnchor.MiddleCenter;

            s_ToggleColor = new GUIStyle(GUI.skin.toggle);
            s_ToggleColor.fixedWidth = 32;
            s_ToggleColor.fixedHeight = 32;

            s_Popup = new GUIStyle(GUI.skin.button);
            s_Popup.fixedWidth = 80f;
            s_Popup.alignment = TextAnchor.MiddleCenter;

            s_SelectedStyle = new GUIStyle(GUI.skin.button);
            s_SelectedStyle.fixedWidth = 44;
            s_SelectedStyle.fixedHeight = 18;
            s_SelectedStyle.alignment = TextAnchor.MiddleCenter;

            s_PageSelector = new GUIStyle(GUI.skin.button);
            s_PageSelector.fixedHeight = 20;
            s_PageSelector.fontSize = 12;
            s_PageSelector.fontStyle = FontStyle.Bold;
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
    }
}