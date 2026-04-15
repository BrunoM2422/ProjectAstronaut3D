using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnBoss();
            Destroy(gameObject);
        }
    }

    public void SpawnBoss()
    {
        bossPrefab.SetActive(true);
    }
}

