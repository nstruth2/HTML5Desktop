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
        gameStarted = false;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void StartBounce()
    {
        if (!gameStarted)
        {
            // Ensure a random direction that's not too vertical
            float xDirection = 0f;

            // Keep generating until the x is not too close to 0 (e.g., [-0.5, 0.5] is too straight down)
            while (Mathf.Abs(xDirection) < 0.4f)
            {
                xDirection = Random.Range(-1f, 1f);
            }

            Vector2 randomDirection = new Vector2(xDirection, 1f).normalized;

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(randomDirection * bounceForce, ForceMode2D.Impulse);
            gameStarted = true;
        }
    }

    public void BeginGame()
    {
        StartBounce();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FallCheck"))
        {
            GameManagerGame7.instance.Restart();
        }
        else if (collision.gameObject.CompareTag("Paddle"))
        {
            GameManagerGame7.instance.ScoreUp();

            if (!gameStarted)
            {
                StartBounce();
            }
        }
    }
}
