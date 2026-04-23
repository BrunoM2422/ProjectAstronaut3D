using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float destroyTime = 2f;

    public int damage = 5;

    public float speed = 20f;

    public MeshRenderer meshRenderer;

    private void Awake()
    {
        Destroy(gameObject, destroyTime);
    }

    public void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damageable = collision.gameObject.GetComponent<IDamageable>();

        if(damageable != null) damageable.Damage(damage);


        Destroy(gameObject);

    }

    public void Setup(float newDamage, float scaleMultiplier)
    {
        float finalDamage = newDamage;

        if (GameManager.Instance.currentPlayer.GetComponent<PlayerScript>() != null && GameManager.Instance.currentPlayer.GetComponent<PlayerScript>().IsMegaBulletsActive())
        {
            finalDamage = damage * GameManager.Instance.currentPlayer.GetComponent<PlayerScript>().megaBulletDamageMultiplier;
        }

        damage = (int)finalDamage;

        transform.localScale *= scaleMultiplier;

        if (GameManager.Instance.currentPlayer.GetComponent<PlayerScript>() != null && GameManager.Instance.currentPlayer.GetComponent<PlayerScript>().IsMegaBulletsActive())
        {
            if (meshRenderer != null)
            {
                meshRenderer.material.color = Color.black;
            }
        }
    }
}