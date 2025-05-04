// EnemyGame6Redo.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGame6 : MonoBehaviour
{
    // We'll now use the speed from the GameManager directly
    // No need for a local speed variable that gets set only at spawn time
    
    void Update()
    {
        // Get the current speed from the GameManager
        float currentSpeed = GameManagerGame6.instance.currentEnemySpeed;
        
        // Move using the current global speed (negative to move in negative Z direction)
        transform.Translate(0, 0, -currentSpeed * Time.deltaTime);
        
        if(transform.position.z < -10f)
        {
            GameManagerGame6.instance.ScoreUp();
            Destroy(gameObject);
        }
    }
}