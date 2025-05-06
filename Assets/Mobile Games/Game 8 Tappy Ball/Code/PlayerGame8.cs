using UnityEngine;
using UnityEngine.UI;

public class PlayerGame8 : MonoBehaviour
{
    private Rigidbody rb;
    public float jumpForce; // Force applied when the player "flaps"
    public GameObject gameOverPanel; // Reference to the Game Over UI
    public Button clearPlayerPrefsButton; // Optional: Reference to clear score button

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Ensure the Game Over panel is initially hidden
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector3.up * jumpForce; // Apply upward force
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pipe"))
        {
            KillPlayer();
        }
        else if (other.gameObject.CompareTag("ScoreZone"))
        {
            Debug.Log("ScoreZone triggered!");

            if (GameManagerGame8.instance != null)
            {
                GameManagerGame8.instance.ScoreUp();
            }
        }
        else if (other.gameObject.CompareTag("KillZone"))
        {
            Debug.Log("KillZone triggered!");
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true; // Disable physics

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Show Game Over UI
        }

        gameObject.SetActive(false); // Disable player

        if (GameManagerGame8.instance != null)
        {
            GameManagerGame8.instance.GameOver();
        }
    }
}
