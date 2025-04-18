using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Game6Enemy : MonoBehaviour
{
    public float speed;
    void Start()
    {
    }
    void Update()
    {
        transform.Translate(0,0,speed*Time.deltaTime);
        if(transform.position.z<-10f)
        {
            Game6GameManager.instance.ScoreUp();
            Destroy(gameObject);
        }
    }
}
