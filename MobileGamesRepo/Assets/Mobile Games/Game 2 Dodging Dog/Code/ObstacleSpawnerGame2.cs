using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnerGame2 : MonoBehaviour
{
    public GameObject obstacle;
    public float spawnRate;
    public float maxXpos;
    // Start is called before the first frame update
    void Start()
    {
        StartSpawning();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    Spawn();
        //}
    }
    void Spawn()
    {
        float randomX = Random.Range(-maxXpos, maxXpos);
        Vector2 spawnPos = new Vector2(randomX, transform.position.y);
        Instantiate(obstacle, spawnPos, Quaternion.identity);
    }
    void StartSpawning()
    {
        InvokeRepeating("Spawn", 1f, spawnRate);
    }
    public void StopSpawning()
    {
        CancelInvoke("Spawn");
    }
}
