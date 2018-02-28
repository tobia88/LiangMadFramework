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

    private Scene m_currentScene;

    private string m_lastSceneName;
    private string m_sceneName;

    public void LoadScene(string sceneName, params object[] args) {
        // Load Scene
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Additive, args));
    }

    public void LoadSceneImmediate(string sceneName, params object[] args) {
        m_sceneName = sceneName;
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single, args));
    }

    IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode sceneMode, params object[] args) {
        m_lastSceneName = m_sceneName;
        m_sceneName = sceneName;

        yield return StartCoroutine(UnloadSceneProgress());
        yield return StartCoroutine(LoadScene(sceneMode, args));

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
        }
    }

    IEnumerator LoadScene(LoadSceneMode sceneMode = LoadSceneMode.Single, params object[] args) {
        AsyncOperation async = SceneManager.LoadSceneAsync(m_sceneName, sceneMode);

        while (!async.isDone) {
            if (loadProgressCallback != null)
                loadProgressCallback(async.progress);

            yield return 1;
        }

        m_currentScene = FindObjectOfType<Scene>();
        m_currentScene.OnStart(args);
    }
}
