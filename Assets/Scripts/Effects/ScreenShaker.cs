using UnityEngine;
using Cinemachine;
using System.Collections;


public class ScreenShaker : Singleton<ScreenShaker>
{
    public CinemachineFreeLook freeLook;

    private CinemachineBasicMultiChannelPerlin noise;

    protected override void Awake()
    {
        base.Awake(); 

        if (freeLook == null)
        {
            Debug.LogError("FreeLook não atribuído!");
            return;
        }

        var rig = freeLook.GetRig(1);

        if (rig == null)
        {
            Debug.LogError("Rig não encontrado!");
            return;
        }

        noise = rig.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise == null)
        {
            Debug.LogError("Noise não encontrado!");
        }
    }

    private Coroutine currentShake;

    public void Shake(float amplitude, float frequency, float duration)
    {
        if (currentShake != null)
            StopCoroutine(currentShake);

        currentShake = StartCoroutine(ShakeCoroutine(amplitude, frequency, duration));
    }

    IEnumerator ShakeCoroutine(float amplitude, float frequency, float duration)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;

        currentShake = null;
    }
}