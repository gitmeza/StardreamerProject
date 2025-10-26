using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int playerLives = 3;
    public int score = 0;
    public int currentWave = 1;

    [Header("Player Settings")]
    public GameObject playerPrefab;
    public Vector3 respawnPosition = Vector3.zero;
    
    [Header("Enemy Spawning")]
    public GameObject[] enemyPrefabs;
    public float spawnRate = 2f;
    public int enemiesPerWave = 10;
    public float waveDelay = 3f;
    
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI waveText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;
    
    [Header("Audio")]
    public AudioClip gameOverSound;
    
    private int enemiesSpawned = 0;
    private int enemiesDestroyed = 0;
    private bool gameActive = true;
    private float nextSpawnTime;
    private AudioSource audioSource;
    private Camera mainCamera;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;
        
        UpdateUI();
        StartWave();
        
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
    }
    
    void Update()
    {
        if (gameActive)
        {
            SpawnEnemies();
        }
    }
    
    void StartWave()
    {
        enemiesSpawned = 0;
        enemiesDestroyed = 0;
        nextSpawnTime = Time.time + waveDelay;
    }
    
    void SpawnEnemies()
    {
        if (enemiesSpawned < enemiesPerWave && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            enemiesSpawned++;
            nextSpawnTime = Time.time + spawnRate;
        }
        else if (enemiesSpawned >= enemiesPerWave && enemiesDestroyed >= enemiesPerWave)
        {
            // Wave completed
            currentWave++;
            UpdateUI();
            StartWave();
        }
    }
    
    void SpawnEnemy()
    {
        if (enemyPrefabs.Length > 0)
        {
            // Random spawn position at top of screen
            float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;
            Vector3 spawnPosition = new Vector3(
                Random.Range(-screenWidth + 1f, screenWidth - 1f),
                mainCamera.orthographicSize + 1f,
                0
            );
            
            // Random enemy type
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            
            // Set movement pattern based on wave
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                if (currentWave <= 2)
                {
                    enemyScript.movementPattern = Enemy.MovementPattern.StraightDown;
                }
                else if (currentWave <= 4)
                {
                    enemyScript.movementPattern = Enemy.MovementPattern.Zigzag;
                }
                else
                {
                    enemyScript.movementPattern = (Enemy.MovementPattern)Random.Range(0, 4);
                }
            }
        }
    }
    
    public void AddScore(int points)
    {
        score += points;
        enemiesDestroyed++;
        UpdateUI();
    }
    
    public void PlayerHit()
    {
        playerLives--;
        UpdateUI();
        
        if (playerLives <= 0)
        {
            GameOver();
        }
        else
        {
            // Respawn player after a delay
            Invoke("RespawnPlayer", 2f);
        }
    }
    
    void RespawnPlayer()
    {
        // Find and respawn player
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, respawnPosition , Quaternion.identity);
        }
    }
    
    void GameOver()
    {
        gameActive = false;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + score;
        }
        
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
        
        // Stop all enemies
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }
    
    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        
        if (livesText != null)
        {
            livesText.text = "Lives: " + playerLives;
        }
        
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
