using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseScene : BaseEntity {
    public string SceneName {
        get { return gameObject.scene.name; }
    }

    public IEnumerator UnloadSceneRoutine() {
        yield return 1;
    }
}
