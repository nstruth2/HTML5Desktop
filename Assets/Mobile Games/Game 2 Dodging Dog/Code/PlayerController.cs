using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerDodgingDog : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed;
    SpriteRenderer sp;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        float horizontal = 0f;

        // Check left/right movement input
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            horizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            horizontal = 1f;
        }

        // Apply movement
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        // Flip sprite based on direction
        if (horizontal != 0)
        {
            sp.flipX = horizontal < 0;
        }
    }
}
