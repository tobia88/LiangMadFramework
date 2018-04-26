using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;
using System.IO;

[CustomEditor(typeof(MultipleSceneSetting))]
public class MultipleSceneSettingEditor : Editor {

    public override void OnInspectorGUI() {
        var t = target as MultipleSceneSetting;

        var data = EditorGUILayout.ObjectField("Data", t.linkingData, typeof(MultipleSceneLinkingData), false);

        if (data != t.linkingData) {
            Undo.RecordObject(t, "Linking Data Update");
            t.linkingData = (MultipleSceneLinkingData) data;
            EditorUtility.SetDirty(t);
        }

        if (GUILayout.Button("Create New")) {
            var path = EditorUtility.SaveFilePanelInProject("Save Data", "SceneData", "asset", "Message");

            if (path.Length != 0) {
                var newData = ScriptableObject.CreateInstance<MultipleSceneLinkingData>();
                AssetDatabase.CreateAsset(newData, path);
                AssetDatabase.SaveAssets();

                t.linkingData = newData;
            }
        }

        var so = new SerializedObject(t.linkingData);

        so.Update();

        var list = so.FindProperty("sceneDatas");

        EditorGUILayout.PropertyField(list, true);

        so.ApplyModifiedProperties();
    }
}

[InitializeOnLoad]
static class MultipleSceneListener {
    static MultipleSceneListener() {
        EditorSceneManager.sceneOpened += SceneOpenedCallback;
    }

    private static void SceneOpenedCallback(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode) {
        if (mode == OpenSceneMode.Single) {
            var settings = GameObject.FindObjectsOfType<MultipleSceneSetting>();

            if (settings.Length > 1) {
                Debug.LogWarning("[MultipleSceneEditor] Make sure there's only a scene component in active Scene!");
                return;
            }
            else if (settings.Length == 1) {
                SetupScenes(settings[0]);
            }
        }
    }

    static void SetupScenes(MultipleSceneSetting _setting) {
        var datas = _setting.linkingData.sceneDatas;

        foreach (var d in datas) {
            var path = AssetDatabase.GetAssetPath(d.sceneObject);
            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
            Debug.Log("[MultipleSceneSetting] Scene Loaded Additively = " + d.sceneObject.name);
        }
    }
}


//public class MultipleSceneEditor : UnityEditor.AssetModificationProcessor {
//    static string[] OnWillSaveAssets(string[] paths) {
//        EnsureFolderExist();
//        SetupScenesData();

//        return paths;
//    }

//    static void EnsureFolderExist() {
//        if (!Directory.Exists(Config.SceneSaveDataPath)) {
//            Directory.CreateDirectory(Config.SceneSaveDataPath);
//            Debug.Log("[Directory Created] " + Config.SceneSaveDataPath);
//        }
//    }

//    static void SetupScenesData() {
//        if (EditorSceneManager.sceneCount > 1) {
//            MultipleSceneSetting setting = EnsureSettingObject();
//            CreateSceneDatas(setting);
//        }
//    }

//    static MultipleSceneSetting EnsureSettingObject() {
//        var retval = Object.FindObjectOfType<MultipleSceneSetting>();
//        if (retval == null) {
//            var go = new GameObject("MultpleSceneSetting");
//            go.transform.SetAsFirstSibling();
//            retval = go.AddComponent<MultipleSceneSetting>();
//        }

//        return retval;
//    }

//    static void CreateSceneDatas(MultipleSceneSetting _setting) {
//        var setups = EditorSceneManager.GetSceneManagerSetup();
//        var datas = new List<MultipleScenesData>();

//        var linkData = _setting.linkingData;

//        if (linkData == null) {
//            linkData = InitSceneLinkData(_setting.gameObject.scene.name);
//        }

//        for (int i = 0; i < setups.Length; i++) {
//            if (setups[i].path == EditorSceneManager.GetActiveScene().path) {
//                continue; // Skip if Active Scene
//            }

//            MultipleScenesData dataToAdd = null;

//            if (linkData.multipleScenesDatas != null && linkData.multipleScenesDatas.Count > 0) {

//                foreach (var d in linkData.multipleScenesDatas) {
//                    if (d.scenePath == setups[i].path) {
//                        dataToAdd = d;
//                        break;
//                    }
//                }
//            }

//            if (dataToAdd == null) {
//                var actualScene = EditorSceneManager.GetSceneByPath(setups[i].path);
//                dataToAdd = new MultipleScenesData(actualScene.name, setups[i].path, SceneTypes.AdditiveAsync);
//            }

//            datas.Add(dataToAdd);
//        }

//        linkData.multipleScenesDatas = datas;

//        _setting.linkingData = linkData;
//    }

//    static MultipleSceneLinkingData InitSceneLinkData(string _sceneName) {
//        var retval = ScriptableObject.CreateInstance<MultipleSceneLinkingData>();

//        AssetDatabase.CreateAsset(retval, Config.SceneSaveDataPath + "/" + _sceneName + ".asset");

//        return retval;
//    }
//}
