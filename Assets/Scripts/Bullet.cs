using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 3f;
    public int damage = 1;
    
    [Header("Bullet Type")]
    public bool isPlayerBullet = true;
    
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Set velocity based on bullet direction
        if (isPlayerBullet)
        {
            rb.linearVelocity = transform.up * speed;
        }
        else
        {
            rb.linearVelocity = -transform.up * speed;
        }
        
        // Destroy bullet after lifetime
        Destroy(gameObject, lifetime);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerBullet)
        {
            // Player bullet hits enemy
            if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
        }
        else
        {
            // Enemy bullet hits player
            if (other.CompareTag("Player"))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    // Player hit - notify game manager
                    GameManager gameManager = FindFirstObjectByType<GameManager>();
                    if (gameManager != null)
                    {
                        gameManager.PlayerHit();
                    }
                    Destroy(other.gameObject);
                }
                Destroy(gameObject);
            }
        }
        
        // Destroy bullet if it hits boundaries
        // if (other.CompareTag("Boundary"))
        // {
        //     Destroy(gameObject);
        // }
    }
}
