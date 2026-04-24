using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public ProjectileBase projectilePrefab;
    public Transform shootingPosition;
    public float timeBetweenShots = 0.3f;
    public float speed = 150;
    public AudioSource shootSound;

    private Coroutine _currentCoroutine;

    protected virtual IEnumerator StartShoot()
    {
        while(true)
        {
            Shoot();
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    public virtual void Shoot()
    {
        shootSound.Play();
        var projectile = Instantiate(projectilePrefab);
        projectile.transform.position = shootingPosition.position;
        projectile.transform.rotation = shootingPosition.rotation;
        projectile.speed = speed;

        if (GameManager.Instance.currentPlayer.GetComponent<PlayerScript>() != null && GameManager.Instance.currentPlayer.GetComponent<PlayerScript>().IsMegaBulletsActive())
        {
            projectile.Setup(
                GameManager.Instance.currentPlayer.GetComponent<PlayerScript>().megaBulletDamageMultiplier,
                GameManager.Instance.currentPlayer.GetComponent<PlayerScript>().megaBulletSizeMultiplier
            );
        }

    }

    public void StartShooting()
    {
        StopShooting();
        _currentCoroutine = StartCoroutine(StartShoot());
    }

    public void StopShooting()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
    }



}
