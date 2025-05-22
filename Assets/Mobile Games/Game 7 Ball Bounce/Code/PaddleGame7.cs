using UnityEngine;

public class PaddleGame7 : MonoBehaviour
{
    public Collider2D ballCollider;
    public float screenEdgeBuffer = 0.5f; // Prevents paddle from going offscreen
    
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private float minXBound;
    private float maxXBound;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    void Start()
    {
        if (ballCollider != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ballCollider, false);
        }

        startPosition = transform.position;
        CalculateScreenBounds();
    }

    void Update()
    {
        FollowMouse();
    }

    void CalculateScreenBounds()
    {
        Camera mainCamera = Camera.main;
        Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        float paddleHalfWidth = GetComponent<Collider2D>().bounds.extents.x;
        
        minXBound = -screenBounds.x + paddleHalfWidth + screenEdgeBuffer;
        maxXBound = screenBounds.x - paddleHalfWidth - screenEdgeBuffer;
    }

    void FollowMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float targetX = Mathf.Clamp(mousePosition.x, minXBound, maxXBound);
        // Instantly set position, only X changes
        rb.position = new Vector2(targetX, startPosition.y);
    }
}
