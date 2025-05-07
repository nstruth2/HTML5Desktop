using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerGame8 : MonoBehaviour
{
    public static GameManagerGame8 instance;

    public Text scoreText;
    public Text highScoreText;

    private int score = 0;
    private int localHighScore = 0;
    private int fetchedHighScore = 0;
    private string fetchedHighScorePlayer = "???";

    private bool isGameOver = false;
    private bool hasFetchedHighScore = false;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        score = 0;
        localHighScore = PlayerPrefs.GetInt("Game8_SubmitScore", 0);
        UpdateUI();
    }

    public void ScoreUp()
    {
        if (isGameOver) return;

        score++;

        if (score > localHighScore)
        {
            localHighScore = score;
            PlayerPrefs.SetInt("Game8_SubmitScore", localHighScore);
        }

        UpdateUI();
    }

    public void GameOver()
    {
        isGameOver = true;

        PlayerPrefs.SetInt("Game8_SubmitScore", score);
        PlayerPrefs.Save();

        Debug.Log($"Game Over. Final Score: {score}, Local High Score: {localHighScore}");

        SceneManager.LoadScene("Submit Score and Name Game 8");
    }

    public void SetFetchedHighScore(int fetchedScore, string fetchedPlayer = "???")
    {
        fetchedHighScore = fetchedScore;
        fetchedHighScorePlayer = fetchedPlayer;
        hasFetchedHighScore = true;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (highScoreText != null)
        {
            if (hasFetchedHighScore)
            {
                if (localHighScore > fetchedHighScore)
                {
                    highScoreText.text = $"Top Score: You: {localHighScore}";
                }
                else
                {
                    highScoreText.text = $"Top Score: {fetchedHighScorePlayer}: {fetchedHighScore}";
                }
            }
            else
            {
                highScoreText.text = $"High Score: {localHighScore}";
            }
        }
    }

    public int GetCurrentScore() => score;
    public int GetHighScore() => localHighScore;
}
