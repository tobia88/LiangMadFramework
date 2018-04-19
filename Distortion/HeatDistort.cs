using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeatDistort : MonoBehaviour {
    public Vector3 startSize;
    public Vector3 endSize;
    public float duration = 2f;
    public bool destroyOnFinished = true;
    public Renderer render;

    private void Awake() {
        render = GetComponent<Renderer>();
    }

    private void Start() {
        if (destroyOnFinished) {
            Destroy(gameObject, duration);
        }

        transform.localScale = startSize;
        transform.DOScale(endSize, duration);

        DOTween.To(() => render.material.GetFloat("_BumpAmt"), (v) => render.material.SetFloat("_BumpAmt", v), 0f, duration);
    }
}
