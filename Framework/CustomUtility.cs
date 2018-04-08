using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class CustomUtility {
    public static void GizmosLabel(string label, Vector3 position, Color color) {
#if UNITY_EDITOR
        Handles.color = color;
        Handles.Label(position, label);
#endif
    }

}
