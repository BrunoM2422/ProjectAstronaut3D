using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public BoxCollider checkpointCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetCheckpoint(transform);
            checkpointCollider.enabled = false;
        }
    }
}