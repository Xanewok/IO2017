using UnityEngine;
using System.Collections;
using UnityEditor;
using YAGTSS.Level;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    private bool displayWarning = true;

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.wideMode = true;

        MapGenerator script = (MapGenerator)target;
        var tileGenerator = script.GetComponent<ITileGenerator>();
        if (tileGenerator == null)
        {
            EditorGUILayout.HelpBox("This component needs a MonoBehaviour implementing ITileGenerator to work properly.",
                MessageType.Warning);
        }

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
