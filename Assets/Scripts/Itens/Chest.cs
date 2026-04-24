using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Itens;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public int coinsAmount = 10;
    public ParticleSystem coinParticles;
    public BoxCollider boxCollider;
    public AudioSource audioSource;


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        { 
            audioSource.Play();
            coinParticles.Play();
            OpenChest();
            ItemManager.Instance.AddByType(ItemType.Coin, coinsAmount);
            boxCollider.enabled = false;

            Destroy(gameObject, 1f);
            
        }
    }

    public void OpenChest()
    {
               animator.SetTrigger("Open");

    }
}
