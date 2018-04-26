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
    public SceneField initScene;
    public string CurrentScene { get; private set; }

    private void Awake() {
        if (string.IsNullOrEmpty(CurrentScene)) {
            BaseScene scn = FindObjectOfType<BaseScene>();
            if (scn != null)
                SetCurrentScene(scn.sceneName);
        }
    }

    private void Start() {
        if (loadSceneOnStart)
            LoadSingleScene(initScene.SceneName, SceneTypes.AdditiveAsync);
    }


    public void LoadSingleScene(string _sceneName, SceneTypes _sceneType, UnityEngine.Object _transitionScene = null) {
        string transitionSceneName = (_transitionScene == null) ? "" : _transitionScene.name;
        var process = new SingleSceneLoadProcess(this,
                                                 CurrentScene,
                                                 _sceneName,
                                                 transitionSceneName);

        StartCoroutine(process.ProcessRoutine());
    }

    public void SetCurrentScene(string _sceneName) {
        CurrentScene = _sceneName;
        Debug.Log("Current Scene: " + CurrentScene);
    }

    public void UnloadScene(string _sceneName) {
        StartCoroutine(UnloadAsync(_sceneName));
    }

    private IEnumerator UnloadAsync(string _unloadSceneName) {
        var scn = SceneManager.GetSceneByName(_unloadSceneName);

        var setting = scn.GetRootGameObjects().GetFirstScript<MultipleSceneSetting>();

        if (setting != null && setting.linkingData != null) {
            var datas = setting.linkingData.sceneDatas;

            foreach (var d in datas)
                yield return SceneManager.UnloadSceneAsync(d.SceneName);
        }

        yield return SceneManager.UnloadSceneAsync(_unloadSceneName);
    }


    // ============================================================
    // SingleSceneLoadProcess
    // ============================================================
    public class SingleSceneLoadProcess {
        public string FromScene { get; private set; }
        public string ToScene { get; private set; }
        public string TransitionScene { get; private set; }

        private BaseTransition m_transition;
        private List<AsyncOperation> m_operations = new List<AsyncOperation>();
        private SceneMng m_mng;

        public SingleSceneLoadProcess(SceneMng _mng, string _fromScene, string _toScene, string _transitionScene = "") {
            m_mng = _mng;

            FromScene = _fromScene;
            ToScene = _toScene;
            TransitionScene = _transitionScene;
        }

        public IEnumerator ProcessRoutine() {
            if (!string.IsNullOrEmpty(TransitionScene)) {
                yield return m_mng.StartCoroutine(SceneTransitionRoutine(TransitionScene));
            }

            if (!string.IsNullOrEmpty(FromScene)) {
                yield return m_mng.StartCoroutine(m_mng.UnloadAsync(FromScene));
            }

            yield return m_mng.StartCoroutine(LoadSceneAsync(ToScene));

            // Fade out transition scene if available
            if (m_transition != null) {
                m_transition.SetState(BaseTransition.TransitionStates.ToFinish);
            }

            // Awake every scenes
            //m_operations.ForEach(op => op.allowSceneActivation = true);
        }

        private IEnumerator SceneTransitionRoutine(string _transitionSceneName) {
            AsyncOperation load = SceneManager.LoadSceneAsync(_transitionSceneName, LoadSceneMode.Additive);

            yield return new WaitUntil(() => load.isDone);

            var scn = SceneManager.GetSceneByName(_transitionSceneName);

            m_transition = scn.GetRootGameObjects().GetFirstScript<BaseTransition>();
            m_transition.SetState(BaseTransition.TransitionStates.ToReady);

            yield return new WaitUntil(() => m_transition.State == BaseTransition.TransitionStates.Ready);
        }

        private IEnumerator LoadSceneAsync(string _loadSceneName) {
            yield return m_mng.StartCoroutine(QueueLoadAsync(_loadSceneName, m_transition));

            var scn = SceneManager.GetSceneByName(_loadSceneName);

            var objects = scn.GetRootGameObjects();

            // Check if any linked scenes
            var setting = scn.GetRootGameObjects().GetFirstScript<MultipleSceneSetting>();

            // Load every single linked scenes
            if (setting != null && setting.linkingData != null) {
                var datas = setting.linkingData.sceneDatas;

                foreach (var d in datas)
                    yield return m_mng.StartCoroutine(QueueLoadAsync(d.SceneName, m_transition));
            }
        }

        private IEnumerator QueueLoadAsync(string _sceneName, BaseTransition _transition = null) {
            var async = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
            async.allowSceneActivation = false;
            m_operations.Add(async);

            while (async.progress < 0.9f) {
                if (_transition != null)
                    _transition.OnProgressUpdate(_sceneName, async.progress);

                yield return null;
            }
        }
    }
}
