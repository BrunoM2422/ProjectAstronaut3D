using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public int coinsAmount = 10;
    public ParticleSystem coinParticles;
    public BoxCollider boxCollider;


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            coinParticles.Play();
            OpenChest();
            ItemManager.Instance.AddCoins(coinsAmount);
            boxCollider.enabled = false;

            Destroy(gameObject, 1f);
            
        }
    }

    public void OpenChest()
    {
               animator.SetTrigger("Open");

    }
}
