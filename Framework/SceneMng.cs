using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public enum SceneTypes {
    Baked,
    AdditiveAsync,
}

[System.Serializable]
public class SceneField {
    public Object sceneObject;
    public string SceneName {
        get { return (sceneObject != null) ? sceneObject.name : string.Empty; }
    }

    public bool IsLoaded {
        get { return SceneManager.GetSceneByName(SceneName).isLoaded; }
    }
}

public class SceneLoadProgress {
    public string SceneName { get; private set; }
    public float Progress { get; private set; }
    public System.Action onProgressFinish;

    public SceneLoadProgress(string _sceneName) {
        SceneName = _sceneName;
    }

    public SceneLoadProgress OnComplete(System.Action _callback) {
        onProgressFinish += _callback;
        return this;
    }

    public IEnumerator LoadSceneAsync() {
        if (!SceneManager.GetSceneByName(SceneName).isLoaded) {
            var async = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

            while (async.progress < 1f) {
                Progress = async.progress;
                yield return 1;
            }
        }

        yield return 1;

        Progress = 1;

        if (onProgressFinish != null)
            onProgressFinish();
    }

    public IEnumerator UnloadSceneAsync() {
        if (SceneManager.GetSceneByName(SceneName).isLoaded) {
            var async = SceneManager.UnloadSceneAsync(SceneName);

            while (async.progress < 1f) {
                yield return 1;
            }
        }

        yield return 1;

        if (onProgressFinish != null)
            onProgressFinish();
    }
}


// ============================================================
// SceneMng
// ============================================================
public class SceneMng : MonoBehaviour {
    public static SceneMng Instance { get; private set; }

    public static void Init() {
        Instance = FindObjectOfType<SceneMng>();
    }

    public bool loadSceneOnStart;
    public SceneField startScene;

    private void Start() {
        if (loadSceneOnStart && startScene != null) {
            SceneManager.LoadSceneAsync(startScene.SceneName, LoadSceneMode.Additive);
        }
    }

    public SceneLoadProgress LoadScene(string _sceneName) {
        var progress = new SceneLoadProgress(_sceneName);
        StartCoroutine(progress.LoadSceneAsync());
        return progress;
    }

    public SceneLoadProgress UnloadScene(string _sceneName) {
        var progress = new SceneLoadProgress(_sceneName);
        StartCoroutine(progress.UnloadSceneAsync());
        return progress;
    }
}
