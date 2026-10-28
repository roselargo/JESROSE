```csharp
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Settings")]
    [SerializeField, Min(0.1f)] private float spawnInterval = 5f;
    [SerializeField, Min(1)] private int maxEnemies = 10;
    [SerializeField] private bool spawnOnStart = true;

    [Header("Difficulty Settings")]
    [SerializeField] private bool increaseDifficulty = true;
    [SerializeField, Min(0.1f)] private float minimumSpawnInterval = 1.5f;
    [SerializeField, Min(0f)] private float difficultyStepTime = 30f;

    private readonly List<GameObject> activeEnemies =
        new List<GameObject>();

    private float currentSpawnInterval;
    private float elapsedTime;
    private Coroutine spawnRoutine;

    private void Start()
    {
        currentSpawnInterval = spawnInterval;

        if (spawnOnStart)
        {
            spawnRoutine = StartCoroutine(SpawnLoop());
        }
    }

    private void Update()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);

        if (increaseDifficulty && difficultyStepTime > 0f)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= difficultyStepTime)
            {
                elapsedTime = 0f;

                currentSpawnInterval = Mathf.Max(
                    minimumSpawnInterval,
                    currentSpawnInterval - 0.25f
                );
            }
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);

            activeEnemies.RemoveAll(enemy => enemy == null);

            if (activeEnemies.Count >= maxEnemies)
                continue;

            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning(
                "EnemySpawner: Enemy prefab is not assigned."
            );
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning(
                "EnemySpawner: Add at least one spawn point."
            );
            return;
        }

        Transform spawnPoint = spawnPoints[
            Random.Range(0, spawnPoints.Length)
        ];

        if (spawnPoint == null)
            return;

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        activeEnemies.Add(enemy);
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    public void StartSpawning()
    {
        if (spawnRoutine == null)
        {
            spawnRoutine = StartCoroutine(SpawnLoop());
        }
    }
}