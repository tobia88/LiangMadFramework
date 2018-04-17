using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

[System.Serializable]
public class SceneData {
    public UnityEngine.Object scene;
    public UnityEngine.Object transitionScene;
    public SceneTypes sceneType;

    public string sceneName {
        get { return (scene != null) ? scene.name : string.Empty; }
    }

    public string transitionSceneName {
        get { return (scene != null) ? transitionScene.name : string.Empty; }
    }
}

public class SceneLoadProgress {
    public SceneTypes sceneType { get; private set; }
    public LoadSceneMode sceneMode { get; private set; }
    public string sceneName { get; private set; }
    public float progress { get; private set; }
    public bool isLoaded {
        get { return SceneManager.GetSceneByName(sceneName).isLoaded; }
    }

    private Action<SceneLoadProgress> m_loadProgressCallback;
    private Action<SceneLoadProgress> m_onFinishedCallback;

    public SceneLoadProgress(SceneData _data, LoadSceneMode _sceneMode) {
        sceneName = _data.sceneName;
        sceneType = _data.sceneType;
        sceneMode = _sceneMode;
    }

    public SceneLoadProgress OnFinished(Action<SceneLoadProgress> _func) {
        m_onFinishedCallback += _func;
        return this;
    }

    public SceneLoadProgress OnLoading(Action<SceneLoadProgress> _func) {
        m_loadProgressCallback += m_onFinishedCallback;
        return this;
    }

    public IEnumerator LoadingRoutine() {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, sceneMode);

        while (!async.isDone) {
            progress = async.progress;

            if (m_loadProgressCallback != null)
                m_loadProgressCallback(this);

            yield return 1;
        }
    }

    public void LoadingFinished() {
        if (m_onFinishedCallback != null)
            m_onFinishedCallback(this);
    }
}

public enum SceneTypes {
    Main,
    Sub
}

public class SceneMng : MonoBehaviour {
    public static SceneMng Instance { get; private set; }

    public static void Init() {
        Instance = FindObjectOfType<SceneMng>();
    }

    private void Awake() {
        if (string.IsNullOrEmpty(CurrentScene)) {
            Scene scn = FindObjectOfType<Scene>();
            if (scn != null)
                SetCurrentScene(scn.sceneName);
        }
    }

    public string CurrentScene { get; private set; }

    public SceneLoadProgress LoadScene(SceneData _sceneData) {
        SceneLoadProgress retval = new SceneLoadProgress(_sceneData, LoadSceneMode.Additive);
        StartCoroutine(LoadSceneAsync(retval));
        return retval;
    }

    public void UnloadScene(string _sceneName) {
        StartCoroutine(UnloadSceneProgress(_sceneName));
    }

    public void SetCurrentScene(string _sceneName) {
        CurrentScene = _sceneName;
        Debug.Log("Current Scene: " + CurrentScene);
    }

    IEnumerator LoadSceneAsync(SceneLoadProgress _sceneProgress) {
        yield return null;

        if (!_sceneProgress.isLoaded) {
            if (_sceneProgress.sceneType == SceneTypes.Main) {
                if (!string.IsNullOrEmpty(CurrentScene)) {
                    yield return StartCoroutine(UnloadSceneProgress(CurrentScene));
                }

                CurrentScene = _sceneProgress.sceneName;
            }

            yield return StartCoroutine(_sceneProgress.LoadingRoutine());
        }

        _sceneProgress.LoadingFinished();

        Debug.Log("[SceneMng]:Scene Loaded: " + _sceneProgress.sceneName);
    }

    IEnumerator UnloadSceneProgress(string _sceneName) {
        if (SceneManager.GetSceneByName(_sceneName).isLoaded) {
            yield return SceneManager.UnloadSceneAsync(_sceneName);
        }

        Debug.Log("[SceneMng]:Scene Unload: " + _sceneName);
    }
}
