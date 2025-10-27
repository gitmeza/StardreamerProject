using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 2f;    // Speed at which background moves down
    private Vector3 startPosition;
    private float spriteHeight;

    void Start()
    {
        startPosition = transform.position;

        // Get height of the sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            spriteHeight = sr.bounds.size.y;
    }

    void Update()
    {
        // Move background down
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Reset position when fully scrolled
        if (transform.position.y <= startPosition.y - spriteHeight)
        {
            transform.position = startPosition;
        }
    }
}
