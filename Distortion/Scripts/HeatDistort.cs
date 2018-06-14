using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeatDistort : MonoBehaviour {
    public Vector3 startSize;
    public Vector3 endSize;
    public float duration = 2f;
    public bool loop = false;
    public bool destroyOnFinished = true;

    public Renderer Renderer { get; private set; }

    private void Awake() {
        Renderer = GetComponent<Renderer>();
    }

    private void Start() {
        if (destroyOnFinished) {
            Destroy(gameObject, duration);
        }

        transform.localScale = startSize;
        transform.DOScale(endSize, duration);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(0f, 0f));
        seq.Append(transform.DOScale(endSize, duration).SetEase(Ease.OutQuint));

        seq.Join(DOTween.To(() => Renderer.material.GetFloat("_BumpAmt"), (v) => Renderer.material.SetFloat("_BumpAmt", v), 0f, duration).SetEase(Ease.OutQuint));

        if (loop)
            seq.SetLoops(-1, LoopType.Restart);
    }
}
