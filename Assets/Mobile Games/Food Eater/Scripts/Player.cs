using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed;
    public float rotateAmount;
    float rot;
    int score;
    public GameObject winText;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; // Ensure game is running
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0) // Check if the game is not paused
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (mousePos.x < 0)
                {
                    rot = rotateAmount;
                }
                else
                {
                    rot = -rotateAmount;
                }
                transform.Rotate(0, 0, rot);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale != 0) // Check if the game is not paused
        {
            rb.velocity = transform.up * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop any movement if the game is paused
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            Destroy(collision.gameObject);
            score++;
            if (score >= 5)
            {
                print("Level Complete");
                winText.SetActive(true);
                FreezePlayer();
                Time.timeScale = 0; // Pause the game
            }
        }
        else if (collision.gameObject.tag == "Danger")
        {
            SceneManager.LoadScene("Game");
        }
    }

    void FreezePlayer()
    {
        rb.velocity = Vector2.zero; // Stop any movement
        rb.angularVelocity = 0f; // Stop any rotation
        rb.isKinematic = true; // Prevent further physics interactions
    }
}
