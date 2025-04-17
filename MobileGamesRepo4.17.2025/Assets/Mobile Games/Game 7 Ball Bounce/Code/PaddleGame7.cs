using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleGame7 : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Collider2D ballCollider; // Reference to the ball's collider
    private Rigidbody2D rb;
    private Vector2 startPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Constrain movement to the X-axis (Freeze Y movement & rotation)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    void Start()
    {
        // Ensure no collision between the paddle and ball
        if (ballCollider != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ballCollider, false); // Allow collision
        }

        // Store the starting position (Y should remain constant)
        startPosition = transform.position;
    }

    void Update()
    {
        TouchMove();
    }

    void TouchMove()
    {
        if (Input.GetMouseButton(0)) // Detect mouse click or touch
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Move the paddle only on the X-axis while keeping Y constant
            transform.position = new Vector2(touchPos.x, startPosition.y);
        }
    }
}
