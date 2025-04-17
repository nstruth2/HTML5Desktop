using UnityEngine;
using UnityEngine.UI;

public class PlayerGame8 : MonoBehaviour
{
    private Rigidbody rb;
    public float jumpForce; // Force applied when the player "flaps"
    public GameObject gameOverPanel; // Reference to the Game Over UI
    public Text scoreText; // Reference to the UI text for the score
    public Text highScoreText; // Reference to the UI text for the high score
    public Button clearPlayerPrefsButton; // Reference to the "Clear High Score" button

    private int score = 0; // Current score
    private int highScore = 0; // High score

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

        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();

        if (clearPlayerPrefsButton != null)
        {
            clearPlayerPrefsButton.onClick.AddListener(ClearPlayerPrefs);
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
            AddScore();
        }
        else if (other.gameObject.CompareTag("KillZone"))
        {
            Debug.Log("KillZone triggered!");
            KillPlayer();
        }
    }

    private void AddScore()
    {
        score++;
        UpdateScoreUI();

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            UpdateScoreUI();
        }
    }

    private void KillPlayer()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true; // Disable physics on the player

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Show the Game Over UI
        }

        gameObject.SetActive(false); // Disable the player object
        Time.timeScale = 0f; // Pause the game
    }

    private void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("HighScore"); // Clear the high score PlayerPrefs
        highScore = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }
}
