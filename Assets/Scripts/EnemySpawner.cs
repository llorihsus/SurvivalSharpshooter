using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private int spawnCount = 1;
    [SerializeField] private float spawnInterval = 5f;

    private void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = objectPool.GetPooledObject();

        if (enemy == null)
        {
            Debug.LogWarning("No enemy available in object pool.");
            return;
        }

        enemy.SetActive(true);

        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.ResetHealthToMax();
        }

        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = true;
            agent.Warp(transform.position);
            agent.isStopped = false;
        }
        else
        {
            enemy.transform.position = transform.position;
        }
    }
}