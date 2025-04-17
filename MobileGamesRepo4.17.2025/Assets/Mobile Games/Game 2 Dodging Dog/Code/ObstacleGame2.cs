using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGame2 : MonoBehaviour
{
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
            GameManagerGame2.instance.GameOver();
        }
        if(collision.gameObject.tag == "Ground")
        {
            GameManagerGame2.instance.IncrementScore();
            Destroy(gameObject);
        }
    }
}
