using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerGame7 : MonoBehaviour
{
    public static GameManagerGame7 instance;
    int score;
    int highScore;
    string highScorePlayer;

    public Text scoreText;
    public Text highScoreText;
    public GameObject mainMenuUI;
    public BallGame7 ballScript;

    public InputField nameInputField;
    public Button submitButton;
    public GameObject nameInputPanel;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {    
        highScore = PlayerPrefs.GetInt("PlayerPrefsGame7_HighScore", 0);
        highScorePlayer = PlayerPrefs.GetString("PlayerPrefsGame7_HighScorePlayer", "No One");

        highScoreText.text = "High Score: " + highScorePlayer + " - " + highScore;

        mainMenuUI.SetActive(true);
        scoreText.gameObject.SetActive(false);
        nameInputPanel.SetActive(false);
        highScoreText.gameObject.SetActive(true); // Ensure high score is always visible

        Time.timeScale = 0;

        // Ensure Submit Button is hooked up
        submitButton.onClick.RemoveAllListeners(); // Remove previous listeners to avoid duplicates
        submitButton.onClick.AddListener(SubmitHighScore);
    }

    void Update()
    {    
        if (Input.GetMouseButtonDown(0) && Time.timeScale == 0 && !nameInputPanel.activeSelf)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        scoreText.gameObject.SetActive(true);
        Time.timeScale = 1;
        ballScript.BeginGame();
    }
    public void ClearHighScore()
    {
        // Reset the high score and player name in PlayerPrefs
        PlayerPrefs.DeleteKey("PlayerPrefsGame7_HighScore");
        PlayerPrefs.DeleteKey("PlayerPrefsGame7_HighScorePlayer");

        // Reset the high score variables in the script
        highScore = 0;
        highScorePlayer = "No One";

        // Update the UI to reflect the reset high score
        highScoreText.text = "High Score: " + highScorePlayer + " - " + highScore;

        // Optionally, save the changes to PlayerPrefs (although PlayerPrefs.DeleteKey already does this)
        PlayerPrefs.Save();
    }
    public void Restart()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("PlayerPrefsGame7_HighScore", highScore);
            PlayerPrefs.Save();

            highScoreText.text = "High Score: No One - " + highScore; // Show placeholder name
            nameInputPanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            SceneManager.LoadScene("Ball Bounce");
        }
    }

    public void SubmitHighScore()
    {
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Whoever";
        }

        highScorePlayer = playerName;
        PlayerPrefs.SetString("PlayerPrefsGame7_HighScorePlayer", highScorePlayer);
        PlayerPrefs.Save();

        highScoreText.text = "High Score: " + highScorePlayer + " - " + highScore;

        nameInputPanel.SetActive(false);
        SceneManager.LoadScene("Ball Bounce");
    }

    public void ScoreUp()
    {
        score++;
        scoreText.text = "Score: " + score;

        if (score > highScore)
        {
            highScoreText.text = "High Score: No One - " + score;
        }
    }
}
