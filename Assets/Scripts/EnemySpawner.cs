using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 2f;
    public int maxEnemies = 20;
    
    [Header("Spawn Area")]
    public float spawnWidth = 8f;
    public float spawnHeight = 1f;
    
    private float nextSpawnTime;
    private int currentEnemyCount;
    
    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }
    
    void Update()
    {
        if (Time.time >= nextSpawnTime && currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }
    
    void SpawnEnemy()
    {
        if (enemyPrefabs.Length > 0)
        {
            // Spawn at top of screen
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;
                Vector3 spawnPosition = new Vector3(
                    Random.Range(-screenWidth + 1f, screenWidth - 1f),
                    mainCamera.orthographicSize + 1f,
                    0
                );
                
                // Random enemy type
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                
                // Track enemy count
                currentEnemyCount++;
                
                // Subscribe to enemy death event
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.OnEnemyDestroyed += OnEnemyDestroyed;
                }
            }
        }
    }
    
    void OnEnemyDestroyed()
    {
        currentEnemyCount--;
    }
}
