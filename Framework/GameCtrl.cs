using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCtrl : MonoBehaviour {
    public static GameCtrl _Instance;

    public static GameCtrl Instance {
        get {
            if (_Instance == null) {
                Init();
            }

            return _Instance;
        }
    }

    public static void Init() {
        var ctrl = FindObjectOfType<GameCtrl>();

        if (ctrl != null)
            Destroy(ctrl.gameObject);

        _Instance = new GameObject("GameCtrl").AddComponent<GameCtrl>();

        DontDestroyOnLoad(_Instance.gameObject);

        AudioMng.Init();
        DataMng.Init();
        InputMng.Init();
    }

    private string _lastScene;
    private string _currentScene;

    public void LoadScene(string p_sceneName, LoadSceneMode p_mode = LoadSceneMode.Single) {
        // Load Scene
        StartCoroutine(LoadSceneAsync(p_sceneName, p_mode));
    }

    IEnumerator LoadSceneAsync(string p_sceneName, LoadSceneMode p_mode) {
        _lastScene = _currentScene;
        _currentScene = p_sceneName;

        yield return SceneManager.LoadSceneAsync(_currentScene, p_mode);
        Debug.Log("Scene Loaded: " + _currentScene);
    }
}
