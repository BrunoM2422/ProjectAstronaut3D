using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUpdater : MonoBehaviour
{
    public Image uiImage;
    public float duration = .1f;
    public Ease ease = Ease.OutBack;
    private Tween _currentTween;

    private void OnValidate()
    {
        if (uiImage == null)
        {
            uiImage = GetComponent<Image>();
        }
    }

    public void UpdateValue(float f)
    {
        uiImage.fillAmount = f;
    }

    public void UpdateValue(float max, float current)
    {
        if (_currentTween != null)
        {
            _currentTween.Kill();
        }

        uiImage.DOFillAmount(current / max, duration).SetEase(ease);
    }
}
