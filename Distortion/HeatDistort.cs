using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeatDistort : MonoBehaviour {
    private Renderer m_renderer;

    public Vector3 startSize;
    public Vector3 endSize;
    public float duration = 2f;
    public bool loop = false;
    public LoopType loopType;
    public bool destroyOnFinished = true;

    private void Awake() {
        m_renderer = GetComponent<Renderer>();
    }

    private void Start() {
        if (destroyOnFinished) {
            Destroy(gameObject, duration);
        }

        var s = DOTween.Sequence();
        s.Append(transform.DOScale(startSize, 0f));
        s.Append(transform.DOScale(endSize, duration).SetEase(Ease.OutQuad));
        s.Join(m_renderer.material.DOFloat(0f, "_BumpAmt", duration).SetEase(Ease.OutQuad));

        if (loop)
            s.SetLoops(-1, loopType);
        else
            s.OnComplete(() => Destroy(gameObject));
        
    }
}
