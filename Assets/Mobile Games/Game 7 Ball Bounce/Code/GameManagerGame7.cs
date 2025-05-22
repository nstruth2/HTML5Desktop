using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerGame7 : MonoBehaviour
{
    public static GameManagerGame7 instance;
    int score;
    int localHighScore = 0;
    int fetchedHighScore = 0;
    string fetchedHighScorePlayer = "???";
    bool hasFetchedHighScore = false;
    bool playerSurpassedFetchedHighScore = false;

    public Text scoreText;
    public Text highScoreText;
    public BallGame7 ballScript;

    private bool isGameOver = true; // Game starts in a stopped state

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scoreText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(true);

        Time.timeScale = 0;
        isGameOver = true;

        // Load local high score
        localHighScore = PlayerPrefs.GetInt("Game7_SubmitScore", 0);

        // At startup, show fetched high score if available, else local
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isGameOver)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        score = 0;
        // Only reset if no fetched high score exists
        if (!hasFetchedHighScore)
        {
            playerSurpassedFetchedHighScore = false;
        }
        scoreText.text = "Score: " + score;
        scoreText.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);

        Time.timeScale = 1;
        isGameOver = false;
        ballScript.BeginGame();
        UpdateUI();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        isGameOver = true;

        // Save local high score if needed
        if (score > localHighScore)
        {
            localHighScore = score;
            PlayerPrefs.SetInt("Game7_SubmitScore", localHighScore);
            PlayerPrefs.Save();
        }

        // Always load the Game Over scene
        SceneManager.LoadScene("Game Over Game 7");
    }

    public void ScoreUp()
    {
        score++;

        // Update local high score if surpassed
        if (score > localHighScore)
        {
            localHighScore = score;
            PlayerPrefs.SetInt("Game7_SubmitScore", localHighScore);
            PlayerPrefs.Save();
        }

        // Only switch to "You" if the player beats the fetched high score in this session
        if (hasFetchedHighScore && !playerSurpassedFetchedHighScore && score > fetchedHighScore)
        {
            playerSurpassedFetchedHighScore = true;
        }

        UpdateUI();
    }

    // Call this from your fetch code when you get the high score from the server
    public void SetFetchedHighScore(int fetchedScore, string fetchedPlayer = "???")
    {
        fetchedHighScore = fetchedScore;
        fetchedHighScorePlayer = fetchedPlayer;
        hasFetchedHighScore = true;

        // Don't set playerSurpassedFetchedHighScore yet unless game is ongoing and score > fetched
        if (!isGameOver && score > fetchedHighScore)
        {
            playerSurpassedFetchedHighScore = true;
        }

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
                if (playerSurpassedFetchedHighScore)
                {
                    highScoreText.text = $"Top Score: You: {score}";
                }
                else
                {
                    highScoreText.text = $"Top Score: {fetchedHighScorePlayer}: {fetchedHighScore}";
                }
            }
            else
            {
                highScoreText.text = $"Top Score: You: {localHighScore}";
            }
        }
    }



    public int GetCurrentScore() => score;
    public int GetHighScore() => localHighScore;
}
