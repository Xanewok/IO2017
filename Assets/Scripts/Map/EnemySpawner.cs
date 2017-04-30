using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float initialDelay = 1.0f;
    public float spawnDelay = 5.0f;

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(initialDelay);
        while (true)
        {
            Instantiate(enemyPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void OnEnable()
    {
        StartCoroutine(SpawnCoroutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
