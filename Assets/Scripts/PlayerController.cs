using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float boundaryPadding = 0.5f;
    
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    
    [Header("Audio")]
    public AudioClip shootSound;
    
    private float nextFireTime;
    private Camera mainCamera;
    private AudioSource audioSource;
    private Vector2 screenBounds;
    
    void Start()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        
        // Calculate screen bounds
        float screenHeight = mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;
        screenBounds = new Vector2(screenWidth - boundaryPadding, screenHeight - boundaryPadding);
    }
    
    void Update()
    {
        HandleMovement();
        HandleShooting();
    }
    
    void HandleMovement()
    {
        Vector2 inputVector = Vector2.zero;
        
        // Check for keyboard input
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                inputVector.x -= 1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                inputVector.x += 1;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                inputVector.y += 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                inputVector.y -= 1;
        }
        
        Vector3 movement = new Vector3(inputVector.x, inputVector.y, 0) * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;
        
        // Clamp position to screen bounds
        newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds.x, screenBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, -screenBounds.y, screenBounds.y);
        
        transform.position = newPosition;
    }
    
    void HandleShooting()
    {
        bool fireInput = false;
        
        // Check for keyboard input (Spacebar)
        if (Keyboard.current != null)
        {
            fireInput = Keyboard.current.spaceKey.isPressed;
        }
        
        // Check for mouse input (Left mouse button)
        if (Mouse.current != null)
        {
            fireInput = fireInput || Mouse.current.leftButton.isPressed;
        }
        
        if (fireInput && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Player hit - notify game manager
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.PlayerHit();
            }
            
            // Destroy player
            Destroy(gameObject);
        }
    }
}
