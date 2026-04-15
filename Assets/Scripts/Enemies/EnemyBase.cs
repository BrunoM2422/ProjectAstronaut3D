using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Animation;

public class EnemyBase : MonoBehaviour, IDamageable
{
   public AnimationBase animationBase;
    public float health = 10f;

    public FlashColor flashColor;

    [SerializeField]private float _currentHealth;

    public float startAnimationDuration = 0.5f;
    public Ease ease = Ease.OutBack;
    public bool startWithSpawnAnimation = true;

    public ParticleSystem deathParticle;

    private void Awake()
    {
        Init();
    }
    protected virtual void Init()
    {
        ResetLife();
    }

    protected void ResetLife()
    {
        _currentHealth = health;
        Spawn();
    }
    protected virtual void Kill()
    {
        
        OnKill();
    }

    protected virtual void OnKill()
    {
        GetComponent<BoxCollider>().enabled = false;
        if(deathParticle != null)
        {
            deathParticle.Play();
        }
        Destroy(gameObject, 0.7f);
        PlayAnimation(AnimationType.Death);


    }   

    public void OnDamage(float f)
    {
        if(flashColor != null)
        {
            flashColor.Flash();
        }
        _currentHealth -= f;

        if(_currentHealth <= 0)
        {
            Kill();
        }
    }

    private void Spawn()
    {
        if(startWithSpawnAnimation)
            SpawnAnimation();
    }
    private void SpawnAnimation()
    {
        transform.DOScale(0, startAnimationDuration).SetEase(ease).From();
    }

    public void PlayAnimation(AnimationType type, bool value = true)
    {
        animationBase.PlayAnimation(type, value);
    }


    public void Damage(float damage)
    {
        OnDamage(damage);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayAnimation(AnimationType.Attack);
        }
    }


}
