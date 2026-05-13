using UnityEngine;
using TMPro;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public BoxCollider checkpointCollider;
    public ParticleSystem checkpointEffect;

    public float textDisplayDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetCheckpoint(transform);
            GameManager.Instance.ShowCheckpointText();

            SaveManager.Instance.SavePlayerState(transform.position);

            checkpointEffect.Play();
            checkpointCollider.enabled = false;
        }
    }
}