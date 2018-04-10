using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public class SceneData {
    public Object sceneAsset;
    public SceneTypes sceneType;

    public string sceneName {
        get { return sceneAsset.name; }
    }

    public bool IsValid {
        get { return sceneAsset != null; }
    }

    public bool IsLoaded {
        get { return SceneManager.GetSceneByName(sceneName).isLoaded; }
    }
}

public enum SceneTypes {
    Main,
    Sub
}

public class SceneMng : MonoBehaviour {
    private static SceneMng _Instance;

    public static SceneMng Instance {
        get { return _Instance; }
    }

    public static void Init() {
        _Instance = FindObjectOfType<SceneMng>();
    }

    public SceneData firstLoadScene;

    public System.Action<float> loadProgressCallback;

    public System.Action FirePreUnloadEvent;
    public System.Action FireUnloadEvent;
    public System.Action<string> FireFinishEvent;

    private string m_currentScene;

    private void Start() {
        if (firstLoadScene != null) {
            if (!firstLoadScene.IsLoaded) {
                LoadScene(firstLoadScene);
            }

            SetCurrentScene(firstLoadScene.sceneName);
        }
    }

    public string CurrentScene {
        get { return m_currentScene; }
    }

    public void LoadScene(SceneData _sceneData) {
        if (_sceneData.IsLoaded) {
            if (FireFinishEvent != null)
                FireFinishEvent(_sceneData.sceneName);
            return;
        }

        StartCoroutine(LoadSceneAsync(_sceneData));
    }

    public void UnloadScene(string _sceneName) {
        StartCoroutine(UnloadSceneProgress(_sceneName));
    }

    public void UnloadScene(Scene _scene) {
        StartCoroutine(UnloadSceneProgress(_scene.sceneName));
    }

    public void SetCurrentScene(string _sceneName) {
        m_currentScene = _sceneName;
        Debug.Log("Current Scene: " + m_currentScene);
    }

    IEnumerator LoadSceneAsync(SceneData _sceneData) {
        string sceneName = _sceneData.sceneAsset.name;

        if (_sceneData.sceneType == SceneTypes.Main) {
            if (!string.IsNullOrEmpty(m_currentScene)) {
                yield return StartCoroutine(UnloadSceneProgress(m_currentScene));
            }

            m_currentScene = _sceneData.sceneName;
        }


        yield return StartCoroutine(LoadScene(sceneName, LoadSceneMode.Additive));

        if (FireFinishEvent != null)
            FireFinishEvent(sceneName);

        Debug.Log("Scene Loaded: " + sceneName);
    }

    IEnumerator UnloadSceneProgress(string _sceneName) {
        Scene unloadScene = FindObjectsOfType<Scene>().FirstOrDefault(s => s.sceneName == _sceneName);

        if (unloadScene != null) {
            if (FirePreUnloadEvent != null)
                FirePreUnloadEvent();

            unloadScene.PreUnload();

            if (FireUnloadEvent != null)
                FireUnloadEvent();
        }

        if (SceneManager.GetSceneByName(_sceneName).isLoaded) {
            yield return SceneManager.UnloadSceneAsync(_sceneName);
        }

        Debug.Log("Scene Unload: " + _sceneName);
    }

    IEnumerator LoadScene(string _sceneName, LoadSceneMode _sceneMode = LoadSceneMode.Single) {
        AsyncOperation async = SceneManager.LoadSceneAsync(_sceneName, _sceneMode);

        while (!async.isDone) {
            if (loadProgressCallback != null)
                loadProgressCallback(async.progress);

            yield return 1;
        }
    }
}
