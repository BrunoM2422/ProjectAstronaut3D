using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;   


public class EffectsManagert : Singleton<EffectsManagert>
{
    public PostProcessVolume postProcessVolume;
    public Vignette vignette;
    public float duration = 0.5f;

    [NaughtyAttributes.Button]
    public void ChangeVignette()
    {
        
        StartCoroutine(FlashColorVignette());
    }

    IEnumerator FlashColorVignette()
    {
        if (postProcessVolume.profile.TryGetSettings<Vignette>(out vignette))
        {
           
            Color originalColor = vignette.color.value;
            float originalIntensity = vignette.intensity.value;

            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;

                vignette.color.Override(Color.Lerp(originalColor, Color.red, t));

                float intensity = Mathf.Sin(t * Mathf.PI);
                vignette.intensity.Override(Mathf.Lerp(originalIntensity, 0.7f, intensity));

                yield return null;
            }

           
            vignette.color.Override(originalColor);
            vignette.intensity.Override(originalIntensity);
        }
    }

}
