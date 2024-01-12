using UnityEditor;
using UnityEngine;

namespace WNC.ITC
{
    public class FAQ : EditorWindow
    {
        #region GUI Styles
        GUIStyle s_ButtonBigPicture;
        GUIStyle s_LabelCentered;
        GUIStyle s_ButtonCreator;
        GUIStyle s_Header;
        GUIStyle s_Line;
        GUIStyle s_ButtonPH;
        GUIStyle s_SubDescriptionCentered;
        #endregion
        public static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(WNC.ITC.FAQ));
            var texture = Resources.Load<Texture>("WNC_ITC/Sprites/LogoWhiteSmall");
            window.titleContent = new GUIContent("ITC - FAQ", texture);
            window.maxSize = new Vector2(350, 550);
            window.minSize = new Vector2(350, 550);
        }

        private void OnGUI()
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

            buttonsTexture = Resources.Load<Texture>("WNC_ITC/Sprites/FAQ");
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

            EditorGUI.BeginDisabledGroup(true);
            GUILayout.Label("HOW TO GENERATE MAP?", s_Header);
            GUILayout.Space(3);
            GUILayout.Label("1] Create [Generator] on scene", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("2] In [Generator] component select [Generator Preset]", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("3] Push [Generate] button in [Generator] component", s_SubDescriptionCentered);
            GUILayout.Space(3);

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.Label("HOW TO GENERATE MAP FROM CODE?", s_Header);
            GUILayout.Space(3);
            GUILayout.Label("WNC.ITC.IsometricTilesCreator.GenerateMap(...)", s_SubDescriptionCentered);
            GUILayout.Space(3);

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.Label("HOW TO CREATE MAP PRESET?", s_Header);
            GUILayout.Space(3);
            GUILayout.Label("1] RMB in 'Project' tab" + System.Environment.NewLine + "Create > Wand and Circles >" + System.Environment.NewLine + "Isometric Tiles Creator > New [Generator Preset]", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("2] Edit your new [Generator Preset] as you wish", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("3] Use [Generator Preset] to generate [Generator]", s_SubDescriptionCentered);

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.Label("HOW TO CREATE POI?", s_Header);
            GUILayout.Space(3);
            GUILayout.Label("1] Create new [POI]", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("2] Edit [POI] grid with [Edit Mode] as you need", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("3] Fill [POI] with any objects in grid borders", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("4] Save [POI] as prefab", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("5] Add [POI] to POI list in [Tiles Set]", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("NOTE: POI is component which fit to map by grid" + System.Environment.NewLine + "which you can edit with [POI] [Edit Mode]." + System.Environment.NewLine + "[POI] attached to [Tiles Set] and will only spawn" + System.Environment.NewLine + "on tiles of this type", s_SubDescriptionCentered);

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.Label("HOW TO CREATE CUSTOM BIOME?", s_Header);
            GUILayout.Space(3);
            GUILayout.Label("1] Create new [Tiles Set]", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("2] Fill all fields you need with prefabs", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("3] Add your new [Tiles Set] to list in [Settings]", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("4] Choose your [Tiles Set] in [Generator Preset]", s_SubDescriptionCentered);
            GUILayout.Space(3);
            GUILayout.Label("NOTE: You can use Tiles Set by different ways," + System.Environment.NewLine + "look on included sets, how we use Water set and on" + System.Environment.NewLine + "other differents between demo sets", s_SubDescriptionCentered);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            HorizontalLine(new Color32(120, 120, 120, 120), 1);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
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

            s_LabelCentered = new GUIStyle();
            s_LabelCentered.alignment = TextAnchor.MiddleCenter;
            s_LabelCentered.normal.textColor = new Color32(175, 175, 175, 255);
            s_LabelCentered.fontSize = 12;
            s_LabelCentered.fontStyle = FontStyle.Bold;

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

            s_ButtonCreator = new GUIStyle(GUI.skin.button);
            s_ButtonCreator.fixedWidth = 250;
            s_ButtonCreator.fixedHeight = 40;
            s_ButtonCreator.padding = new RectOffset(3, 3, 3, 3);
            s_ButtonCreator.alignment = TextAnchor.MiddleCenter;
        }
    }
}