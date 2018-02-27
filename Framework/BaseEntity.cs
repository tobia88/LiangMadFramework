using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour {
    public void Call(System.Action callback, float duration, bool ignoreTimescale = false) {
        StartCoroutine(CallDelay(callback, duration, ignoreTimescale));
    }

    IEnumerator CallDelay(System.Action callback, float duration, bool ignoreTimescale) {
        if (ignoreTimescale) {
            yield return new WaitForSecondsRealtime(duration);
        }
        else {
            yield return new WaitForSeconds(duration);
        }

        callback();
    }
}
