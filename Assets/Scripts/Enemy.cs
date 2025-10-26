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
    
    private float nextFireTime;
    private Vector3 startPosition;
    private float timeAlive;
    private GameManager gameManager;
    
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
        }
    }
    
    void HandleShooting()
    {
        if (enemyBulletPrefab != null && firePoint != null && Time.time >= nextFireTime)
        {
            if (Random.value < shootChance)
            {
                GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
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
        
        Destroy(gameObject);
    }
    
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         // Enemy hit player
    //         PlayerController player = other.GetComponent<PlayerController>();
    //         if (player != null)
    //         {
    //             GameManager gameManager = FindFirstObjectByType<GameManager>();
    //             if (gameManager != null)
    //             {
    //                 gameManager.PlayerHit();
    //             }
    //             Destroy(other.gameObject);
    //         }
    //         Die();
    //     }
    // }
}
