using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerGame4PreStripSomePlayerPrefs : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject gamePlayUI;
    public GameObject spawner;
    public GameObject backgroundParticle;
    public static GameManagerGame4PreStripSomePlayerPrefs instance;
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

        savedHighScore = PlayerPrefs.GetInt("Game4_TopScoreValue", 0); // Updated key
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
            PlayerPrefs.SetInt("Game4_TopScoreValue", score); // Updated key
            PlayerPrefs.SetString("Game4_TopScorePlayer", "YOU"); // Updated key
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

        if (score > savedHighScore)
        {
            savedHighScore = score;
            PlayerPrefs.SetInt("Game4_HighScore", savedHighScore);
            PlayerPrefs.SetInt("Game4_TopScoreValue", score); // Updated key
            PlayerPrefs.SetString("Game4_TopScorePlayer", "YOU"); // Updated key
            PlayerPrefs.Save();

            highScoreText.text = "Top Score: You: " + score;
        }
        else
        {
            UpdateHighScoreText();
        }
    }

    private void UpdateHighScoreText()
    {
        string topPlayer = PlayerPrefs.GetString("Game4_TopScorePlayer", "N/A"); // Updated key
        int topScore = PlayerPrefs.GetInt("Game4_TopScoreValue", 0); // Updated key

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