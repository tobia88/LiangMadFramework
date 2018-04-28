using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(MultipleSceneLinkingData))]
public class MultipleSceneLinkingDataEditor : Editor {

    [MenuItem("LiangMadFramework/Scenes/Save Current Scenes Hierarchy")]
    static void SaveCurrentHierarchy() {
        EnsureFolder();

        var setups = EditorSceneManager.GetSceneManagerSetup();

        var path = EditorUtility.SaveFilePanelInProject("Create Scene Linking Data", "Scn_Data_1", "asset", "");

        if (path.Length != 0) {
            var linkingData = ScriptableObject.CreateInstance<MultipleSceneLinkingData>();

            foreach (var s in setups) {
                MultipleScenesData data = new MultipleScenesData();
                data.sceneObject = AssetDatabase.LoadAssetAtPath<SceneAsset>(s.path);
                linkingData.sceneDatas.Add(data);
            }

            AssetDatabase.CreateAsset(linkingData, path);
            AssetDatabase.SaveAssets();
        }
    }

    static void EnsureFolder() {
        if (!Directory.Exists(Config.SceneSaveDataPath)) {
            Directory.CreateDirectory(Config.SceneSaveDataPath);
        }
    }

    public override void OnInspectorGUI() {
        var t = (MultipleSceneLinkingData) target;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save Current Hierarchy")) {
            var setups = EditorSceneManager.GetSceneManagerSetup();

            var newList = new List<MultipleScenesData>();

            foreach (var s in setups) {
                SceneAsset scn = AssetDatabase.LoadAssetAtPath<SceneAsset>(s.path);
                MultipleScenesData dataToAdd = null;
                foreach (var d in t.sceneDatas) {
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

            Undo.RecordObject(t, "List Update");
            t.sceneDatas = newList;
            EditorUtility.SetDirty(t);
        }

        if (GUILayout.Button("Load Scenes")) {
            LoadScenes(t);
        }

        GUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }

    void LoadScenes(MultipleSceneLinkingData _data) {
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
}
