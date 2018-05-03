using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReference : MonoBehaviour {
    public MultipleSceneSetting linkingData;

    public bool IsLoading { get; private set; }

    protected System.Action m_callback;

    public void LoadScene(System.Action _callback) {
        if (IsLoading)
            return;

        m_callback = _callback;

        StartCoroutine(LoadSceneRoutine(linkingData.sceneDatas));
    }

    private IEnumerator LoadSceneRoutine(List<MultipleScenesData> _datas) {
        IsLoading = true;

        foreach(var d in _datas) {
            var prog = SceneMng.Instance.LoadScene(d.SceneName);

            while (prog.Progress < 1)
                yield return 1;
        }

        IsLoading = false;

        if (m_callback != null)
            m_callback();
    }
}
