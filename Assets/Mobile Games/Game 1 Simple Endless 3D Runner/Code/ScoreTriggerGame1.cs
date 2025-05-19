using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Call your ScoreUp method
            Debug.Log("Trigger done");
            FindObjectOfType<GameManagerGame1>().ScoreUp();

            // Optionally, destroy or disable this trigger so it doesn't fire again
            Destroy(gameObject);
        }
    }
}
