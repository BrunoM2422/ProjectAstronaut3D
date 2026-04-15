using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOrbsBase : MonoBehaviour
{
    public int damage = 5;

    public Transform boss;
    public float rotationSpeed = 50f;

    void Update()
    {
        transform.RotateAround(boss.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null) damageable.Damage(damage);
    }
}
