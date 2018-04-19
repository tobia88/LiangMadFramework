using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTransition : Scene {
    public TransitionStates State { get; private set; }

    public enum TransitionStates {
        Null,
        ToReady,
        Ready,
        ToFinish,
        Finish,
    }

    public virtual void SetState(TransitionStates _state) {
        State = _state;

        switch(_state) {
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
    }

    protected virtual void OnChangeToReady() { }
    protected virtual void OnChangeReady() {}
    protected virtual void OnChangeToFinish() { }
    protected virtual void OnChangeFinish() {
        SceneMng.Instance.UnloadScene(sceneName);
    }
}
