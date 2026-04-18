using UnityEngine;
using DG.Tweening;

public class PlayerFlashColor : MonoBehaviour
{
    private Renderer[] renderers;

    public Color color = Color.red;
    public float duration = 0.1f;

    private Color[] defaultColors;
    private Tween _currentTween;

    private void Start()
    {
        // pega TODOS os renderers (Mesh + SkinnedMesh)
        renderers = GetComponentsInChildren<Renderer>();

        defaultColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            defaultColors[i] = renderers[i].material.GetColor("_EmissionColor");
        }
    }

    public void Flash()
    {
        if (_currentTween != null && _currentTween.IsActive()) return;

        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < renderers.Length; i++)
        {
            int index = i;

            seq.Join(
                renderers[index].material
                    .DOColor(color, "_EmissionColor", duration)
                    .SetLoops(2, LoopType.Yoyo)
            );
        }

        _currentTween = seq;
    }
}