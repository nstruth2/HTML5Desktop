using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGame7 : MonoBehaviour
{
    Rigidbody2D rb;
    public float bounceForce;
    bool gameStarted;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Ensure the ball doesn't start moving until the game starts
        gameStarted = false;

        // Make sure the ball's velocity is zero when the game starts
        rb.velocity = Vector2.zero;

        // Pause the physics by setting the Rigidbody2D's body type to Kinematic
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    // Start the ball's movement when the paddle hits it
    public void StartBounce()
    {
        if (!gameStarted) // Only apply force if the game hasn't started yet
        {
            // Generate a random direction for the ball to start bouncing
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), 1).normalized;

            // Switch to Dynamic body type to allow physics interactions
            rb.bodyType = RigidbodyType2D.Dynamic;

            // Apply force to the ball
            rb.AddForce(randomDirection * bounceForce, ForceMode2D.Impulse);

            // Set gameStarted to true so we don't keep applying the force
            gameStarted = true;
        }
    }

    // Called when the game starts, set this up via the GameManager
    public void BeginGame()
    {
        // Start the ball's movement
        StartBounce();
    }

    // Handle ball collisions with other objects
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FallCheck"))
        {
            // Restart the game if the ball falls
            GameManagerGame7.instance.Restart();
        }
        else if (collision.gameObject.CompareTag("Paddle"))
        {
            // Increase the score when the ball hits the paddle
            GameManagerGame7.instance.ScoreUp();

            // Start bouncing the ball once it hits the paddle (if game hasn't started yet)
            if (!gameStarted)
            {
                StartBounce();
            }
        }
    }
}
