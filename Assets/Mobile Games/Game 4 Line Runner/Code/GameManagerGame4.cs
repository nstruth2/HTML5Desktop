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
    private int savedHighScore = 0;
    private bool isShaking = false;

    public Text scoreText;
    public Text livesText;
    public Text highScoreText;

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

        savedHighScore = PlayerPrefs.GetInt("Game4_HighScore", 0);
        UpdateHighScoreText();

        StartGame();
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

        if (score >= savedHighScore)
        {
            PlayerPrefs.SetInt("Game4_HighScore", score);
            PlayerPrefs.SetInt("TopScoreValue", score);
            PlayerPrefs.SetString("TopScorePlayer", "YOU");
            PlayerPrefs.Save();

            Debug.Log("New high score! Loading submission scene.");
            SceneManager.LoadScene("Submit Score And Name Game 4");
            return;
        }
        else
        {
            PlayerPrefs.Save();
            SceneManager.LoadScene("Main Menu Game 4");
        }
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

        // Check if the score has surpassed the saved high score
        if (score > savedHighScore)
        {
            savedHighScore = score; // Update session variable
            PlayerPrefs.SetInt("Game4_HighScore", savedHighScore); // Save the new high score
            PlayerPrefs.Save();

            // Update high score text to show "You: score"
            highScoreText.text = "Top Score: You: " + score;

            // Optionally, save this new "You" score into PlayerPrefs for later
            PlayerPrefs.SetInt("TopScoreValue", score);
            PlayerPrefs.SetString("TopScorePlayer", "YOU");
            PlayerPrefs.Save();
        }
        else
        {
            // If the current score is not a new high score, show the previous top score
            UpdateHighScoreText();
        }
    }

    private void UpdateHighScoreText()
    {
        // Retrieve the top player and their score from PlayerPrefs
        string topPlayer = PlayerPrefs.GetString("TopScorePlayer", "N/A");
        int topScore = PlayerPrefs.GetInt("TopScoreValue", 0);

        // Display the top player and score
        highScoreText.text = "Top Score: " + topPlayer + ": " + topScore;
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
