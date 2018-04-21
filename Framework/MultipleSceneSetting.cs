using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSceneSetting : MonoBehaviour {
    public MultipleSceneLinkingData LinkingData {
        get { return GameData.GetSceneLinkingData(gameObject.scene.name); }
    }

    public string SceneName {
        get { return gameObject.scene.name; }
    }
}
