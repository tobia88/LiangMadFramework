using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class LMEditorUtility {
    public static void ShowList(SerializedProperty _list) {
        EditorGUILayout.PropertyField(_list);
        EditorGUI.indentLevel++;
        for (int i = 0; i < _list.arraySize; i++) {
            EditorGUILayout.PropertyField(_list.GetArrayElementAtIndex(i));
        }
        EditorGUI.indentLevel--;
    }
}
