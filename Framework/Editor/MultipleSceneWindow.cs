using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class MultipleSceneWindow : EditorWindow {
    // Add menu named "My Window" to the Window menu
    [MenuItem("LiangMadFramework/Scenes/Multiple Scenes Window", priority = 1)]
    static void Init() {
        EnsureFolder();

        // Get existing open window or if none, make a new one:
        MultipleSceneWindow window = GetWindow<MultipleSceneWindow>();
        window.Show();
    }

    void OnGUI() {
        var datas = Resources.FindObjectsOfTypeAll<MultipleSceneLinkingData>();

        var setting = FindObjectOfType<MultipleSceneSetting>();

        foreach (var data in datas) {
            GUILayout.BeginHorizontal();
            var mark = (setting != null && setting.linkingData == data) ? "*" : "";
            EditorGUILayout.PrefixLabel(mark + data.name + mark);

            if (GUILayout.Button("Load Scenes")) {
                LoadScenes(data);
            }

            if (GUILayout.Button("Save Current Hierarchy")) {
                SaveCurrentHierarchy(data);
            }

            GUILayout.EndHorizontal();
        }
    }

    static void LoadScenes(MultipleSceneLinkingData _data) {
        var sceneSetups = EditorSceneManager.GetSceneManagerSetup();

        foreach (var setup in sceneSetups) {
            bool exist = false;
            foreach (var d in _data.sceneDatas) {
                var path = AssetDatabase.GetAssetPath(d.sceneObject);

                if (path == setup.path) {
                    exist = true;
                    break;
                }
            }

            if (!exist) {
                var scn = EditorSceneManager.GetSceneByPath(setup.path);
                EditorSceneManager.CloseScene(scn, true);
            }
        }


        foreach (var d in _data.sceneDatas) {
            var path = AssetDatabase.GetAssetPath(d.sceneObject);
            var scn = EditorSceneManager.GetSceneByPath(path);

            if (scn.isLoaded)
                continue;

            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
            Debug.Log("[MultipleSceneSetting] Scene Loaded Additively = " + d.sceneObject.name);
        }
    }

    static void SaveCurrentHierarchy(MultipleSceneLinkingData _data) {
        var setups = EditorSceneManager.GetSceneManagerSetup();

        var newList = new List<MultipleScenesData>();

        foreach (var s in setups) {
            SceneAsset scn = AssetDatabase.LoadAssetAtPath<SceneAsset>(s.path);
            MultipleScenesData dataToAdd = null;
            foreach (var d in _data.sceneDatas) {
                if (scn.name == d.SceneName) {
                    dataToAdd = d;
                    break;
                }
            }

            if (dataToAdd == null) {
                dataToAdd = new MultipleScenesData();
                dataToAdd.sceneObject = scn;
            }

            newList.Add(dataToAdd);
        }

        Undo.RecordObject(_data, "List Update");
        _data.sceneDatas = newList;
        EditorUtility.SetDirty(_data);
    }

    static void EnsureFolder() {
        if (!Directory.Exists(Config.SceneSaveDataPath)) {
            Directory.CreateDirectory(Config.SceneSaveDataPath);
        }
    }

}
