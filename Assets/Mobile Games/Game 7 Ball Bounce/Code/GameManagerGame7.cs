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

    public Text scoreText;
    public Text highScoreText;
    public GameObject mainMenuUI;
    public BallGame7 ballScript;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainMenuUI.SetActive(true);
        scoreText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(true);

        Time.timeScale = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale == 0)
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

    public void GameOver()
    {
        Time.timeScale = 0;

        if (score > PlayerPrefs.GetInt("Game7_SubmitScore", 0))
        {
            PlayerPrefs.SetInt("Game7_SubmitScore", score);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Submit Score and Name Game 7");//comment
        }
    }

    public void ScoreUp()
    {
        score++;
        scoreText.text = "Score: " + score;

        // Update top score live and change label if new top score is reached
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "Top Score: You - " + highScore;
        }
    }
}
