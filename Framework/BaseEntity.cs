using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour {
    protected GameCtrl gameCtrl;

    protected virtual void Awake() {
        gameCtrl = GameCtrl.Instance;
    }

    protected virtual void Start() { }
    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }
    protected virtual void OnDestroy() { }
    protected virtual void OnCollisionEnter2D(Collision2D collision) { }
    protected virtual void OnCollisionExit2D(Collision2D collision) { }
    protected virtual void OnTriggerEnter2D(Collider2D collider) { }
    protected virtual void OnTriggerExit2D(Collider2D collider) { }

    protected virtual void Call(System.Action p_callback, float p_duration) {
        StartCoroutine(CallDelay(p_callback, p_duration));
    }

    IEnumerator CallDelay(System.Action p_callback, float p_duration) {
        yield return new WaitForSeconds(p_duration);
        p_callback();
    }
}
