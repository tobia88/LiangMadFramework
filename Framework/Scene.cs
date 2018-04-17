using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Scene : BaseEntity {
    public string sceneName {
        get { return gameObject.scene.name; }
    }
}
