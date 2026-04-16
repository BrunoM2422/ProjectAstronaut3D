using Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyBase, IDamageable 
{
    
    public float speed = 5f;
    public float range;
    public ProjectileBase projectilePrefab;
    public Transform shootingPosition;
    public float bulletSpeed = 80;

    public float cooldownTime = 2f;

    private float nextShootTime = 0f;

    public int amountPerShot = 12;
    public float angle = 8f;

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

            if (distance <= range && Time.time >= nextShootTime)
            {
                Shoot();
                nextShootTime = Time.time + cooldownTime;
            }
        }
        else
        {
            PlayAnimation(AnimationType.Run, false);
        }

    }

    public void Shoot()
    {
        int mult = 0;

        for (int i = 0; i < amountPerShot; i++)
        {
            if (i % 2 == 0)
            {
                mult++;
            }

            float currentAngle = (i % 2 == 0 ? angle : -angle) * mult;

            Quaternion rotation = shootingPosition.rotation * Quaternion.Euler(0, currentAngle, 0);

            var projectile = Instantiate(projectilePrefab, shootingPosition.position, rotation);

            projectile.speed = bulletSpeed;
        }

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
