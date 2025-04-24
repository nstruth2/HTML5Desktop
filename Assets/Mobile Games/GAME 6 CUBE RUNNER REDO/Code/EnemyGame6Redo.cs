using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGame6Redo : MonoBehaviour
{

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);

        if(transform.position.z < -10f)
        {
            GameManagerGame6Redo.instance.ScoreUp();
            Destroy(gameObject);
        }

    }
}
