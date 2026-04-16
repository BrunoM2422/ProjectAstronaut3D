using Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyBase, IDamageable
{
    
    public float speed = 5f;
    public float range;

    private void Start()
    {
        if(player == null)
        {
            player = GameManager.Instance.currentPlayer;
        }
    }

     public void Update()
    {
        if (isDead) return;

        CheckPlayerNullity();

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= range)
        {
            PlayAnimation(AnimationType.Run, true);

            Vector3 direction = player.position - transform.position;
            direction.y = 0f;
            direction = direction.normalized;

            transform.position += direction * speed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                transform.forward = direction;
            }
        }
        else {
            PlayAnimation(AnimationType.Run, false);
             }

    }

    override protected void OnKill()
    {
        GetComponent<BoxCollider>().enabled = false;
        if (deathParticle != null)
        {
            deathParticle.Play();
        }
        isDead = true;
        Destroy(gameObject, 0.7f);
        PlayAnimation(AnimationType.Death);


    }
}

