using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSceneLinkingData : ScriptableObject {
    public MultipleScenesData[] multipleScenesDatas;
}

[System.Serializable]
public class MultipleScenesData {
    public Object sceneObject;
    public SceneTypes sceneType;
}
