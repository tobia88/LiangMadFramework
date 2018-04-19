using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadingTransition : BaseTransition {
    public float duration = 2f;
    protected Image m_image;

    private void Awake() {
        m_image = transform.Find("MainImg").GetComponent<Image>();
        m_image.SetAlpha(0f);
    }

    protected override void OnChangeToReady() {
        DOTween.To(() => m_image.color.a, (a) => m_image.SetAlpha(a), 1f, duration)
               .OnComplete(() => SetState(TransitionStates.Ready));

        Debug.Log("On Switch To Ready");
    }

    protected override void OnChangeToFinish() {
        Color targetColor = m_image.color;
        targetColor.a = 0f;

        DOTween.To(() => m_image.color, (c) => m_image.color = c, targetColor, duration)
               .OnComplete(() => SetState(TransitionStates.Finish));
    }

}
