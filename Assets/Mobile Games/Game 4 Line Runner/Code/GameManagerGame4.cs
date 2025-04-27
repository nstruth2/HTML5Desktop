using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerGame4 : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject gamePlayUI;
    public GameObject spawner;
    public GameObject backgroundParticle;
    public static GameManagerGame4 instance;
    public bool gameStarted = false;
    Vector3 originalCamPos;
    public GameObject player;

    private int lives = 3;
    private int score = 0;
    private bool isShaking = false;

    public Text scoreText;
    public Text livesText;
    public Text highScoreText;

    private int currentHighScore = 0; // Store the fetched high score

    private HighScoreFetcherGame4 highScoreFetcher;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        originalCamPos = Camera.main.transform.position;

        menuUI.SetActive(true);
        gamePlayUI.SetActive(true);
        spawner.SetActive(false);
        backgroundParticle.SetActive(false);

        // Get the HighScoreFetcherGame4 script to fetch high score
        highScoreFetcher = FindObjectOfType<HighScoreFetcherGame4>();

        // Wait for high score to be fetched
        StartCoroutine(WaitForHighScore());

        StartGame();
    }

    private IEnumerator WaitForHighScore()
    {
        // Wait for the high score to be fetched
        while (highScoreFetcher == null || highScoreFetcher.currentHighScore == 0)
        {
            yield return null;
        }

        // Set the fetched high score
        currentHighScore = highScoreFetcher.currentHighScore;

        // Update the UI with the fetched high score
        highScoreText.text = $"Top Score: {highScoreFetcher.currentHighScorePlayer}: {currentHighScore}";
    }

    public void StartGame()
    {
        gameStarted = true;
        lives = 3;
        UpdateLivesUI();

        menuUI.SetActive(false);
        gamePlayUI.SetActive(true);
        spawner.SetActive(true);
        backgroundParticle.SetActive(true);
        player.SetActive(true);
        score = 0;
        scoreText.text = "Score: " + score;

        Time.timeScale = 1;
    }

    public void GameOver()
    {
        player.SetActive(false);
        PlayerPrefs.SetInt("Game4_SubmitScore", score);

        // Store player's score before transitioning to the submit scene
        PlayerPrefs.SetInt("CurrentScore", score);  // Save score
        PlayerPrefs.SetInt("CurrentHighScore", currentHighScore);  // Save current high score

        // Load the Submit Score And Name Game 4 scene where users can enter their name and score
        SceneManager.LoadScene("Submit Score And Name Game 4");  // Transition to score submission scene
    }

    public void UpdateLives()
    {
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateLivesUI()
    {
        livesText.text = "Lives: " + lives;
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = "Score: " + score;

        // Check if the current score surpasses the stored high score
        if (score > currentHighScore)
        {
            // Update the fetched high score immediately
            currentHighScore = score;
            highScoreText.text = $"Top Score: You: {score}";
        }
    }

    public void ExitGame()
    {
#if UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("finishAndRemoveTask");
#else
        Application.Quit();
#endif
    }

    public void Shake()
    {
        if (!isShaking)
        {
            StartCoroutine(CameraShake());
        }
    }

    private IEnumerator CameraShake()
    {
        isShaking = true;

        Vector3 startPos = Camera.main.transform.position;

        for (int i = 0; i < 5; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * 0.5f;
            Camera.main.transform.position = new Vector3(randomPos.x, randomPos.y, startPos.z);
            yield return new WaitForSeconds(0.05f);
        }

        Camera.main.transform.position = startPos;
        isShaking = false;
    }

    private void ReloadLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game 4 Line Runner");
    }
}
