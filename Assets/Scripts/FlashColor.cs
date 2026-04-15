using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FlashColor : MonoBehaviour
{
    public MeshRenderer meshRenderer;


    public Color color = Color.red;
    public float duration = 0.1f;

    private Color defaultColor;

    private Tween _currentTween;

        private void Start()
    {
        defaultColor = meshRenderer.material.GetColor("_EmissionColor");
    }


    public void Flash()
    {
        if(!_currentTween.IsActive())
        {
           _currentTween = meshRenderer.material.DOColor(color,"_EmissionColor", duration).SetLoops(2,LoopType.Yoyo);
        }
        

    }
}
