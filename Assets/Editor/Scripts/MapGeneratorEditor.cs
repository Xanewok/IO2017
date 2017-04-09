using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    private bool displayWarning = true;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DrawExtendedInspector();
    }

    void DrawExtendedInspector()
    {
        MapGenerator myScript = (MapGenerator)target;

        displayWarning = EditorGUILayout.Toggle("Display warning on Clear", displayWarning);
        if (GUILayout.Button("Generate"))
        {
            myScript.Generate();
        }

        if (GUILayout.Button("Clear") &&
            (!displayWarning || EditorUtility.DisplayDialog(
                "Clear generated tiles",
                "Are you sure you want to delete generated tiles? This will remove every generated object tagged MapTile!",
                "Confirm",
                "Cancel")))
        {
            myScript.Clear();
        }
    }
}
