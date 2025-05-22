using System.Collections;
using UnityEngine;

public class ObstacleSpawnerGame2 : MonoBehaviour
{
    public GameObject obstacle;
    public float startSpawnRate = 2f;     // Initial time between spawns (seconds)
    public float minSpawnRate = 0.5f;     // Minimum time between spawns
    public float spawnRateDecrease = 0.05f; // How much to decrease the spawn rate after each spawn
    public float maxXpos;

    private float currentSpawnRate;
    private bool spawning = true;

    void Start()
    {
        currentSpawnRate = startSpawnRate;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(1f); // Initial delay

        while (spawning)
        {
            Spawn();

            // Decrease the spawn rate, but don't go below minSpawnRate
            currentSpawnRate = Mathf.Max(minSpawnRate, currentSpawnRate - spawnRateDecrease);

            yield return new WaitForSeconds(currentSpawnRate);
        }
    }

    void Spawn()
    {
        float randomX = Random.Range(-maxXpos, maxXpos);
        Vector2 spawnPos = new Vector2(randomX, transform.position.y);
        Instantiate(obstacle, spawnPos, Quaternion.identity);
    }

    public void StopSpawning()
    {
        spawning = false;
    }
}
