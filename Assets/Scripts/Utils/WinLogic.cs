using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLogic : MonoBehaviour
{
    public GameObject winScreen;
    public BoxCollider winCollider;
    public AudioSource winSound;
    public ParticleSystem winParticles;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.currentPlayer.GetComponent<PlayerScript>().characterController.enabled = false;
            winScreen.SetActive(true);
            winSound.Play();
            winParticles.Play();
            winCollider.enabled = false; 
        }
    }
}

