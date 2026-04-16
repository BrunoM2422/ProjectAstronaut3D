using Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyBase, IDamageable
{
    
    public float range;
    public ProjectileBase projectilePrefab;
    public Transform shootingPosition;
    public float speed = 150;

    public float cooldownTime = 1.5f;

    private float nextShootTime = 0f;

    private void Start()
    {
        if (player == null)
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

        PlayAnimation(AnimationType.Run, false);
        if (distance <= range)
        {

            Vector3 direction = (player.position - transform.position).normalized;

            Vector3 flatDirection = direction;
            flatDirection.y = 0f;

            if (flatDirection != Vector3.zero)
            {
                transform.forward = flatDirection;
            }


            if (distance <= range && Time.time >= nextShootTime)
            {
                Shoot();
                nextShootTime = Time.time + cooldownTime;
            }
        }
    
           
    }

    public void Shoot()
    {
        var projectile = Instantiate(projectilePrefab);
        projectile.transform.position = shootingPosition.position;
        projectile.transform.rotation = shootingPosition.rotation;
        projectile.speed = speed;
        PlayAnimation(AnimationType.Attack);

    }

    override protected void OnKill()
    {
        isDead = true;
        GetComponent<BoxCollider>().enabled = false;
        if (deathParticle != null)
        {
            deathParticle.Play();
        }
        Destroy(gameObject, 0.7f);
        PlayAnimation(AnimationType.Death);


    }
}



