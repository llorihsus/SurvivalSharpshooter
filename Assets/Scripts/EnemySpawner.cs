using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int spawnCount = 1;
    [SerializeField] float spawnInterval = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(spawnInterval);
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject enemy = ObjectPool.SharedInstance.GetPooledObject();
            if (enemy != null)
            {
                Health enemyHealth = enemy.GetComponent<Health>();
                enemyHealth.ResetHealthToMax();
                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                agent.enabled = false;
                enemy.transform.position = transform.position;
                agent.Warp(transform.position);

                enemy.SetActive(true);
                agent.enabled = true;
                agent.isStopped = false;
            }
        }
        StartCoroutine(SpawnEnemy());
    }
}
