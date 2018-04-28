using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionStates {
    Null,
    ToReady,
    Ready,
    ToFinish,
    Finish,
}

public class BaseTransition : BaseScene {
    public TransitionStates State { get; private set; }

    public System.Action<TransitionStates> onStateChanged;

    private string m_fromSceneName;
    private string m_targetSceneName;


    public virtual void SetState(TransitionStates _state) {
        State = _state;

        switch (_state) {
            case TransitionStates.ToReady:
                OnChangeToReady();
                break;

            case TransitionStates.Ready:
                OnChangeReady();
                break;

            case TransitionStates.ToFinish:
                OnChangeToFinish();
                break;

            case TransitionStates.Finish:
                OnChangeFinish();
                break;
        }

        if (onStateChanged != null)
            onStateChanged(State);
    }

    protected virtual void OnChangeToReady() { }

    protected virtual void OnChangeReady() {
        SceneMng.Instance.UnloadScene(m_fromSceneName);
        SceneMng.Instance.LoadScene(m_targetSceneName)
                         .OnComplete(() => SetState(TransitionStates.ToFinish));
    }

    protected virtual void OnChangeToFinish() { }

    protected virtual void OnChangeFinish() {
        SceneMng.Instance.UnloadScene(SceneName);
    }

    public void BeginTransit(string _fromSceneName, string _targetSceneName) {
        m_fromSceneName = _fromSceneName;
        m_targetSceneName = _targetSceneName;

        SetState(TransitionStates.ToReady);
    }
}
