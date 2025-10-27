using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health = 1;
    public int scoreValue = 10;
    public float moveSpeed = 2f;
    
    [Header("Shooting")]
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float shootChance = 0.3f;
    
    [Header("Movement Pattern")]
    public MovementPattern movementPattern = MovementPattern.StraightDown;
    public float zigzagAmplitude = 2f;
    public float zigzagFrequency = 2f;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip enemyDeathSound;

    private float nextFireTime;
    private Vector3 startPosition;
    private float timeAlive;
    private GameManager gameManager;
    private AudioSource audioSource;
    
    // Event for when enemy is destroyed
    public System.Action OnEnemyDestroyed;
    
    public enum MovementPattern
    {
        StraightDown,
        Zigzag,
        Circle,
        SideToSide
    }
    
    void Start()
    {
        startPosition = transform.position;
        nextFireTime = Time.time + Random.Range(0f, fireRate);
        gameManager = FindFirstObjectByType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        HandleMovement();
        HandleShooting();
    }
    
    void HandleMovement()
    {
        timeAlive += Time.deltaTime;
        Vector3 movement = Vector3.zero;
        
        switch (movementPattern)
        {
            case MovementPattern.StraightDown:
                movement = Vector3.down * moveSpeed * Time.deltaTime;
                break;
                
            case MovementPattern.Zigzag:
                float zigzagOffset = Mathf.Sin(timeAlive * zigzagFrequency) * zigzagAmplitude;
                movement = new Vector3(zigzagOffset * Time.deltaTime, -moveSpeed * Time.deltaTime, 0);
                break;
                
            case MovementPattern.Circle:
                float circleRadius = 2f;
                float angle = timeAlive * moveSpeed;
                Vector3 circlePosition = new Vector3(
                    Mathf.Cos(angle) * circleRadius,
                    startPosition.y - timeAlive * moveSpeed,
                    0
                );
                transform.position = startPosition + circlePosition;
                if (transform.position.y < -6f)
                {
                    break;
                }
                return;
                
            case MovementPattern.SideToSide:
                float sideOffset = Mathf.Sin(timeAlive * zigzagFrequency) * zigzagAmplitude;
                movement = new Vector3(sideOffset * Time.deltaTime, -moveSpeed * Time.deltaTime, 0);
                break;
        }
        
        transform.position += movement;
        
        // Destroy if off screen
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
            if (gameManager != null)
            {
                gameManager.EnemyDestroyed();
            }
        }
    }
    
    void HandleShooting()
    {
        if (enemyBulletPrefab != null && firePoint != null && Time.time >= nextFireTime)
        {
            if (Random.value < shootChance)
            {
                GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
                if (audioSource != null && shootSound != null)
                {
                    audioSource.PlayOneShot(shootSound);
                }
                
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.isPlayerBullet = false;
                }
            }
            nextFireTime = Time.time + fireRate;
        }
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        // Add score
        if (gameManager != null)
        {
            gameManager.AddScore(scoreValue);
        }

        // Notify spawner
        OnEnemyDestroyed?.Invoke();

        // Create explosion effect (optional)
        // Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // Play sound
        if (enemyDeathSound != null)
        {
            AudioSource.PlayClipAtPoint(enemyDeathSound, Camera.main.transform.position, 1f);
        }
        
        Destroy(gameObject);
    }
}
