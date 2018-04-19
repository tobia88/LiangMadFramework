using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class MultipleSceneEditor : UnityEditor.AssetModificationProcessor{
    public static string SceneSaveDataPath { get { return "Assets/_Projects/Resources/Datas/Scenes"; } }

    [MenuItem("LiangMadFramework/Save Scenes")]
    public static void GetScenesList() {
        SceneSetup[] setups = EditorSceneManager.GetSceneManagerSetup();
        foreach(SceneSetup setup in setups) {
            Debug.Log(setup.path);
        }
    }

    static string[] OnWillSaveAssets(string[] paths) {
        EnsureFolderExist();

        return paths;
    }

    static void EnsureFolderExist() {
        if (!Directory.Exists(SceneSaveDataPath)) {
            Directory.CreateDirectory(SceneSaveDataPath);
            Debug.Log("[Directory Created]: " + SceneSaveDataPath);
        }
    }

}
