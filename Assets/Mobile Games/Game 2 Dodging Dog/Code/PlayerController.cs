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
        if(Input.GetMouseButton(0))
        {
            if(Input.mousePosition.x < Screen.width/2)
            {
                //move left
                rb.velocity = Vector2.left * moveSpeed;
                sp.flipX = true;
            }
            else
            {
                //move right
                rb.velocity = Vector2.right * moveSpeed;
                sp.flipX = false;
            }
            //6:18
        }
    }
}
