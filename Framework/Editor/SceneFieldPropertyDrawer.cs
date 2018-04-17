using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SceneData))]
public class SceneFieldPropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, position.width * 0.5f, 16);
        var sceneTypeRect = new Rect(position.x + position.width * 0.5f + 5, position.y, position.width * 0.2f, 16);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.ObjectField(amountRect, property.FindPropertyRelative("scene"), typeof(SceneAsset), GUIContent.none);
        EditorGUI.PropertyField(sceneTypeRect, property.FindPropertyRelative("sceneType"), GUIContent.none);
        
        if ((SceneTypes)property.FindPropertyRelative("sceneType").enumValueIndex == SceneTypes.Main) {
            var tSceneRect = new Rect(position.x, position.y + 18, position.width * 0.5f, 15);
            EditorGUI.ObjectField(tSceneRect, property.FindPropertyRelative("transitionScene"), typeof(SceneAsset), GUIContent.none);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if ((SceneTypes) property.FindPropertyRelative("sceneType").enumValueIndex == SceneTypes.Main) {
            return base.GetPropertyHeight(property, label) * 2f + 2;
        }
        return base.GetPropertyHeight(property, label);
    }
}
