using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGame4 : MonoBehaviour
{
    public static float globalSpeed = 5f; // Shared speed for all obstacles

    void Update()
    {
        // Move the obstacle using the current globalSpeed
        transform.position += Vector3.left * globalSpeed * Time.deltaTime;

        // Destroy obstacle if it moves too far left
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    public static void ResetSpeed()
    {
        globalSpeed = 5f; // Reset to initial value when scene reloads
        Debug.Log("Speed Reset to: " + globalSpeed);
    }

    public static void IncreaseGlobalSpeed(float amount)
    {
        globalSpeed += amount;
        Debug.Log("Global Speed Increased to: " + globalSpeed);
    }
}
