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
        EditorGUILayout.ObjectField(new GUIContent("Data"), t.LinkingData, typeof(MultipleSceneLinkingData), false);

        var sceneDatas = t.LinkingData.multipleScenesDatas;

        for(int i = 0; i < sceneDatas.Count; i++) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PrefixLabel(new GUIContent(sceneDatas[i].sceneName));
            var newSceneType = (SceneTypes) EditorGUILayout.EnumPopup(sceneDatas[i].sceneType);

            if (sceneDatas[i].sceneType != newSceneType) {
                
                //sceneDatas[i].sceneType = newSceneType;
            }
            EditorGUI.indentLevel--;
        }
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
                Debug.LogWarning("[MultipleSceneEditor]: Make sure there's only a scene component in active Scene!");
                return;
            }
            else if (settings.Length == 1) {
                SetupScenes(settings[0]);
            }
        }
    }

    static void SetupScenes(MultipleSceneSetting _setting) {
        var datas = _setting.LinkingData.multipleScenesDatas;

        foreach (var d in datas) {
            EditorSceneManager.OpenScene(d.scenePath, OpenSceneMode.Additive);
            Debug.Log("[MultipleSceneSetting]: Scene Loaded Additively = " + d.sceneName);
        }
    }
}


public class MultipleSceneEditor : UnityEditor.AssetModificationProcessor {
    static bool isSaving = false;

    static string[] OnWillSaveAssets(string[] paths) {
        if (isSaving)
            return paths;

        isSaving = true;
        EnsureFolderExist();
        SetupScenesData();
        isSaving = false;

        return paths;
    }

    static void EnsureFolderExist() {
        if (!Directory.Exists(GameData.SceneSaveDataPath)) {
            Directory.CreateDirectory(GameData.SceneSaveDataPath);
            Debug.Log("[Directory Created]: " + GameData.SceneSaveDataPath);
        }
    }

    static void SetupScenesData() {
        if (EditorSceneManager.sceneCount > 1) {
            MultipleSceneSetting setting = EnsureSettingObject();
            CreateSceneDatas(setting);
        }
    }

    static MultipleSceneSetting EnsureSettingObject() {
        var retval = Object.FindObjectOfType<MultipleSceneSetting>();
        if (retval == null) {
            var go = new GameObject("MultpleSceneSetting");
            go.transform.SetAsFirstSibling();
            retval = go.AddComponent<MultipleSceneSetting>();
        }

        return retval;
    }

    static void CreateSceneDatas(MultipleSceneSetting _setting) {
        var setups = EditorSceneManager.GetSceneManagerSetup();
        var datas = new List<MultipleScenesData>();

        var linkData = _setting.LinkingData;

        if (linkData == null) {
            linkData = InitSceneLinkData(_setting.SceneName);
        }

        for (int i = 0; i < setups.Length; i++) {
            if (setups[i].path == EditorSceneManager.GetActiveScene().path) {
                continue; // Skip if Active Scene
            }

            if (linkData.multipleScenesDatas != null && linkData.multipleScenesDatas.Count > 0) {
                foreach (var d in linkData.multipleScenesDatas) {
                    if (d.scenePath == setups[i].path) {
                        datas.Add(d);
                        continue;
                    }
                }
            }

            var actualScene = EditorSceneManager.GetSceneByPath(setups[i].path);
            datas.Add(new MultipleScenesData(actualScene.name, setups[i].path, SceneTypes.AdditiveAsync));
        }

        linkData.multipleScenesDatas = datas;

        Debug.Log("Data Saved");
    }

    static MultipleSceneLinkingData InitSceneLinkData(string _sceneName) {
        var retval = ScriptableObject.CreateInstance<MultipleSceneLinkingData>();

        AssetDatabase.CreateAsset(retval, GameData.SceneDataPath + "/" + _sceneName + ".asset");
        AssetDatabase.SaveAssets();

        return retval;
    }
}
