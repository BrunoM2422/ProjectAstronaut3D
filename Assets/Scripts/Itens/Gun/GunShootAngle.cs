using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootAngle : GunShootLimit
{

    public int amountPerShot = 4;
    public float angle = 15f;

    public override void Shoot()
    {
        int mult = 0;


        for (int i = 0; i < amountPerShot; i++)
        {

            if (i % 2 == 0)
            {
                mult++;
            }
            
            shootSound.Play();
            var projectile = Instantiate(projectilePrefab, shootingPosition);

            projectile.transform.localPosition = Vector3.zero;
            projectile.transform.localEulerAngles = Vector3.zero + Vector3.up * (i % 2 == 0 ? angle : -angle) * mult;

            projectile.speed = speed;
            projectile.transform.parent = null;

            if (GameManager.Instance.currentPlayer.GetComponent<PlayerScript>() != null && GameManager.Instance.currentPlayer.GetComponent<PlayerScript>().IsMegaBulletsActive())
            {
                projectile.Setup(
                    GameManager.Instance.currentPlayer.GetComponent<PlayerScript>().megaBulletDamageMultiplier,
                    GameManager.Instance.currentPlayer.GetComponent<PlayerScript>().megaBulletSizeMultiplier
                );
            }

        }


    }
}
