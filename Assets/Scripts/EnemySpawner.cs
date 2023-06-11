using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int enemyCount = 7;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnDelay;

    private float nextSpawnTime;
    private int enemiesSpawned;

    private void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime += spawnDelay;

            if (enemiesSpawned < enemyCount)
            {
                SpawnEnemy();
                enemiesSpawned++;
            }
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
