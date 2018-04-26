using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultipleSceneLinkingData : ScriptableObject {
    public List<MultipleScenesData> sceneDatas = new List<MultipleScenesData>();
}

[System.Serializable]
public class MultipleScenesData {
    public Object sceneObject;
    public SceneTypes sceneType = SceneTypes.AdditiveAsync;
    public bool unloadTogether = true;

    public string SceneName {
        get { return (sceneObject == null) ? string.Empty : sceneObject.name; }
    }

    public bool IsLoaded {
        get { return SceneManager.GetSceneByName(SceneName).isLoaded; }
    }
}
