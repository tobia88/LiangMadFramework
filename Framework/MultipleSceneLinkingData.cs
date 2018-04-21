using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSceneLinkingData : ScriptableObject {
    public List<MultipleScenesData> multipleScenesDatas = new List<MultipleScenesData>();
}

[System.Serializable]
public struct MultipleScenesData {
    public string sceneName;
    public string scenePath;
    public SceneTypes sceneType;

    public MultipleScenesData(string _sceneName, string _scenePath, SceneTypes _sceneType) {
        sceneName = _sceneName;
        scenePath = _scenePath;
        sceneType = _sceneType;
    }
}
