using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeatDistort))]
public class HeatDistortEditor : Editor
{
    private HeatDistort m_distort;

    private void OnEnable()
    {
        m_distort = (HeatDistort) target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var startSize = serializedObject.FindProperty("startSize");
        var endSize = serializedObject.FindProperty("endSize");
        var duration = serializedObject.FindProperty("duration");
        var destroy = serializedObject.FindProperty("destroyOnFinished");
        var loop = serializedObject.FindProperty("loop");
        var loopType = serializedObject.FindProperty("loopType");

        SerializeSize(startSize);
        SerializeSize(endSize);

        EditorGUILayout.PropertyField(duration);
        EditorGUILayout.PropertyField(loop, new GUIContent("Loop?"));

        if (loop.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(loopType);
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUILayout.PropertyField(destroy);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void SerializeSize(SerializedProperty _prop)
    {
        EditorGUILayout.PropertyField(_prop);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Set"))
        {
            _prop.vector3Value = m_distort.transform.localScale;
        }

        if (GUILayout.Button("Preview"))
        {
            m_distort.transform.localScale = _prop.vector3Value;
        }

        EditorGUILayout.EndHorizontal();

    }
}
