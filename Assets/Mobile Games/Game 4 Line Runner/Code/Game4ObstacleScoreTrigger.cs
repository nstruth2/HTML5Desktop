using UnityEngine;

public class ObstacleScoreTriggerGame4 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) // Use OnTriggerEnter2D if using 2D
    {
        if (other.CompareTag("Player")) // Ensure your player has the tag "Player"
        {
            GameManagerGame4.instance.UpdateScore(); // Increase score when the player passes
            Destroy(gameObject); // Destroy trigger to prevent multiple points for one obstacle
        }
    }
}
