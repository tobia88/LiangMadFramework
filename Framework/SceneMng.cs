using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMng : MonoBehaviour {
    private static SceneMng _Instance;

    public static SceneMng Instance {
        get { return _Instance; }
    }

    public static void Init() {
        var mng = FindObjectOfType<SceneMng>();

        if (mng != null)
            Destroy(mng.gameObject);

        var go = new GameObject("SceneMng");

        _Instance = go.AddComponent<SceneMng>();

        DontDestroyOnLoad(go);
    }

    public System.Action<float> loadProgressCallback;

    public System.Action FirePreUnloadEvent;
    public System.Action FireUnloadEvent;
    public System.Action<string> FireFinishEvent;

    private Scene m_currentScene;

    private string m_lastSceneName;
    private string m_sceneName;

    public void LoadScene(string _sceneName, bool _unload = true, params object[] _args) {
        // Load Scene
        StartCoroutine(LoadSceneAsync(_sceneName, LoadSceneMode.Additive, _unload, _args));
    }

    public void UnloadScene(string _sceneName) {
        SceneManager.UnloadSceneAsync(_sceneName);
    }

    IEnumerator LoadSceneAsync(string _sceneName, LoadSceneMode _sceneMode, bool _unload, params object[] _args) {
        m_lastSceneName = m_sceneName;
        m_sceneName = _sceneName;

        if (_unload) {
            yield return StartCoroutine(UnloadSceneProgress());
        }
        yield return StartCoroutine(LoadScene(_sceneMode, _args));

        if (FireFinishEvent != null)
            FireFinishEvent(_sceneName);

        Debug.Log("Scene Loaded: " + m_sceneName);
    }

    IEnumerator UnloadSceneProgress() {
        if (m_currentScene != null) {

            if (FirePreUnloadEvent != null)
                FirePreUnloadEvent();

            m_currentScene.PreUnload();

            if (FireUnloadEvent != null)
                FireUnloadEvent();

            yield return SceneManager.UnloadSceneAsync(m_lastSceneName);

            m_currentScene = null;

            Debug.Log("Scene Unload: " + m_lastSceneName);
        }
    }

    IEnumerator LoadScene(LoadSceneMode _sceneMode = LoadSceneMode.Single, params object[] _args) {
        AsyncOperation async = SceneManager.LoadSceneAsync(m_sceneName, _sceneMode);

        while (!async.isDone) {
            if (loadProgressCallback != null)
                loadProgressCallback(async.progress);

            yield return 1;
        }

        m_currentScene = FindObjectOfType<Scene>();
        m_currentScene.OnStart(_args);
    }
}
