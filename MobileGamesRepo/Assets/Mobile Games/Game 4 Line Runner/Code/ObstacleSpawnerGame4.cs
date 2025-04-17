using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnerGame4 : MonoBehaviour
{
    public GameObject[] obstacles;
    private Vector3 spawnPos;
    
    public static float globalSpawnRate = 4f; // Shared spawn rate for all spawners
    public float minSpawnRate = 0.6f; // Prevents unfair difficulty
    public float spawnRateDecrease = 0.4f; // How much to decrease per second
    public float speedIncreaseRate = 0.5f; // Speed increase per second

    void Awake()
    {
        // Reset global values when the scene reloads
        globalSpawnRate = 4f;
        ObstacleGame4.ResetSpeed();
    }

    void Start()
    {
        spawnPos = transform.position;
        StartCoroutine(SpawnObstacles());
        Debug.Log("Min Spawn Rate at Start: " + minSpawnRate);
    }

    IEnumerator SpawnObstacles()
    {
        float lastUpdateTime = Time.timeSinceLevelLoad;

        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(globalSpawnRate);

            // Decrease the spawn rate over time
            float elapsedTime = Time.timeSinceLevelLoad - lastUpdateTime;
            globalSpawnRate = Mathf.Max(minSpawnRate, globalSpawnRate - spawnRateDecrease * elapsedTime);
            lastUpdateTime = Time.timeSinceLevelLoad;

            // Increase obstacle speed over time
            ObstacleGame4.IncreaseGlobalSpeed(speedIncreaseRate);
        }
    }

    void Spawn()
    {
        int randObstacle = Random.Range(0, obstacles.Length);
        int randomSpot = Random.Range(0, 2); // 0 = top, 1 = bottom
        spawnPos = transform.position;

        // Spawn the obstacle
        if (randomSpot < 1)
        {
            Instantiate(obstacles[randObstacle], spawnPos, Quaternion.identity);
        }
        else
        {
            spawnPos.y = -transform.position.y;
            Instantiate(obstacles[randObstacle], spawnPos, Quaternion.Euler(0, 0, 180));
        }
    }
}
