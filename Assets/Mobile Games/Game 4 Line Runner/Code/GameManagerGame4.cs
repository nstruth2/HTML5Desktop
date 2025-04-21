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
    private int currentHighScore = 0;

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

        currentHighScore = PlayerPrefs.GetInt("Game4_HighScore", 0);
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

        UpdateHighScoreText();
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        player.SetActive(false);

        PlayerPrefs.SetInt("Game4_SubmitScore", score);
        PlayerPrefs.Save();

        if (score >= currentHighScore)
        {
            PlayerPrefs.SetInt("Game4_HighScore", score);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Submit Score And Name Game 4");
            return;
        }
        else
        {
            SceneManager.LoadScene("Main Menu Game 4");
        }
    }

    private IEnumerator FreezeGameAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0;

        if (score < currentHighScore)
        {
            ReloadLevel();
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

        if (score > currentHighScore)
        {
            currentHighScore = score;
            PlayerPrefs.SetInt("Game4_HighScore", score);
            PlayerPrefs.SetInt("TopScoreValue", score);
            PlayerPrefs.SetString("TopScorePlayer", "YOU"); // You can customize this as needed
            PlayerPrefs.Save();
        }

        UpdateHighScoreText();
    }


    private void UpdateHighScoreText()
    {
        string topPlayer = PlayerPrefs.GetString("TopScorePlayer", "N/A");
        int topScore = PlayerPrefs.GetInt("TopScoreValue", 0);
        highScoreText.text = "Top Score: " + topPlayer + " - " + topScore;
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
        Debug.Log("SHAKE CALLED");
        StartCoroutine(CameraShake());
    }

    private IEnumerator CameraShake()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * 0.5f;
            Camera.main.transform.position = new Vector3(randomPos.x, randomPos.y, originalCamPos.z);
            yield return null;
        }
        Camera.main.transform.position = originalCamPos;
    }

    private void ReloadLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game 4 Line Runner");
    }
}
