using System.Collections.Generic;
using UnityEngine;

namespace WNC.ITC
{
    public class POI : MonoBehaviour
    {
        public List<Vector3> tilesPattern;
        public List<Vector3> connectorTiles;
        public List<GameObject> poiTiles;
        public Transform controlsParent;
        public bool editorSelected = false;
        public bool editMode = false;
        public bool lockGridVisibility = false;
        public int edit_rayType = -1;
        public Vector3 raycastedPOIcollider;
        public Vector3 raycastedHit;
        public List<Vector3> adders;
        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (editMode)
            {
                if (tilesPattern != null)
                {
                    Gizmos.color = new Color32(255, 255, 255, 50);
                    foreach (Vector3 localPos in tilesPattern) Gizmos.DrawWireCube(transform.TransformPoint(localPos), Vector3.one * 2f);
                    foreach (Vector3 localPos in adders) Gizmos.DrawWireCube(transform.TransformPoint(localPos), Vector3.one * 2f);

                    if (edit_rayType == 0)
                    {
                        GUIStyle asd = new GUIStyle();
                        asd.normal.textColor = new Color32(255, 200, 0, 255);
                        asd.alignment = TextAnchor.UpperLeft;
                        UnityEditor.Handles.Label(raycastedHit, "          [D] - Delete Tile" + System.Environment.NewLine + "          [W] - Up" + System.Environment.NewLine + "          [S] - Down", asd);
                        asd.normal.textColor = new Color32(105, 245, 255, 255);
                        UnityEditor.Handles.Label(raycastedHit,  System.Environment.NewLine + System.Environment.NewLine + System.Environment.NewLine + "          [C] - Connector", asd);

                        Gizmos.color = new Color32(255, 255, 255, 255);
                        Gizmos.DrawWireCube(raycastedPOIcollider, Vector3.one * 2f);
                    }
                    else if (edit_rayType == 1)
                    {
                        GUIStyle asd = new GUIStyle();
                        asd.normal.textColor = new Color32(150, 225, 150, 255);
                        asd.alignment = TextAnchor.UpperLeft;
                        UnityEditor.Handles.Label(raycastedHit, "          [A] - Add Tile", asd);

                        Gizmos.color = new Color32(255, 255, 255, 255);
                        Gizmos.DrawWireCube(raycastedPOIcollider, Vector3.one * 2f);
                    }
                }
            }
#endif
        }
    }
}