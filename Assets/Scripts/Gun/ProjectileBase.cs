using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float destroyTime = 2f;

    public int damage = 5;

    public float speed = 20f;

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
}