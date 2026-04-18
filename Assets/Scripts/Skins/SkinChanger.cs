using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    public SkinnedMeshRenderer[] playerMeshRenderer;
    [Header("Skins")]
    public Texture2D defaultSkin;
    public Texture2D infiniteBulletsSkin;
    public Texture2D superJumpSkin;
    public Texture2D megaBulletsSkin;


    private Texture2D currentSkin;

    public void Awake()
    {
        if (playerMeshRenderer == null || playerMeshRenderer.Length == 0)
        {
            Debug.LogError("Nenhum SkinnedMeshRenderer atribuído!");
        }

        ApplySkin(defaultSkin);
    }



    public void ApplySkin(Texture2D newSkin)
    {
        if (newSkin == null) return;

        currentSkin = newSkin;

        foreach (var renderer in playerMeshRenderer)
        {
            if (renderer == null) continue;

            var mat = renderer.material;

            mat.SetTexture("_MainTex", currentSkin);

            mat.SetTexture("_EmissionMap", currentSkin);

        }
    }
}
