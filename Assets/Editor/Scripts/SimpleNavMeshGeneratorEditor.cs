using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using UnityEditor.AI;
using YAGTSS.Level;

[CustomEditor(typeof(SimpleNavMeshGenerator))]
public class SimpleNavMeshGeneratorEditor : Editor
{
    SerializedProperty agentTypeID;
    SerializedProperty defaultArea;
    SerializedProperty layerMask;
    SerializedProperty useGeometry;

    void OnEnable()
    {
        agentTypeID = serializedObject.FindProperty("m_AgentTypeID");
        defaultArea = serializedObject.FindProperty("m_DefaultArea");
        layerMask = serializedObject.FindProperty("m_LayerMask");
        useGeometry = serializedObject.FindProperty("m_UseGeometry");

        NavMeshVisualizationSettings.showNavigation++;
    }

    void OnDisable()
    {
        NavMeshVisualizationSettings.showNavigation--;
    }

    public override void OnInspectorGUI()
    {
        DrawProperties();
        DrawActionButtons();
    }

    void DrawProperties()
    {
        serializedObject.Update();

        var bs = NavMesh.GetSettingsByID(agentTypeID.intValue);
        if (bs.agentTypeID != -1)
        {
            // Draw image
            const float diagramHeight = 80.0f;
            Rect agentDiagramRect = EditorGUILayout.GetControlRect(false, diagramHeight);
            NavMeshEditorHelpers.DrawAgentDiagram(agentDiagramRect, bs.agentRadius, bs.agentHeight, bs.agentClimb, bs.agentSlope);
        }
        NavMeshComponentsGUIUtility.AgentTypePopup("Agent Type", agentTypeID);

        EditorGUILayout.Space();
        NavMeshComponentsGUIUtility.AreaPopup("Default Area", defaultArea);
        EditorGUILayout.PropertyField(layerMask);
        EditorGUILayout.PropertyField(useGeometry);

        serializedObject.ApplyModifiedProperties();
    }

    void DrawActionButtons()
    {
        SimpleNavMeshGenerator myScript = target as SimpleNavMeshGenerator;
        if (GUILayout.Button("Generate"))
        {
            myScript.BuildNavMesh();
        }

        if (GUILayout.Button("Clear"))
        {
            myScript.RemoveData();
        }
    }
}
