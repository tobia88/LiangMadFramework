using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCtrl : MonoBehaviour {
    public Sound playSound;
    public bool destroyIfPlayEnd;

    private ParticleSystem m_particleSystem;

    private void Start() {
        m_particleSystem = GetComponent<ParticleSystem>();

        if (playSound.IsValid) {
            AudioMng.Instance.PlayOneShot(playSound);
        }

        if (destroyIfPlayEnd) {
            StartCoroutine(WaitingToPlayEnd());
        }
    }

    private IEnumerator WaitingToPlayEnd() {
        yield return new WaitUntil(() => !m_particleSystem.isPlaying);
        Destroy(gameObject);
    }
}
