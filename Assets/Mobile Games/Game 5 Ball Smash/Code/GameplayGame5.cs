using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayGame5 : MonoBehaviour
{
    public GameObject winText;
    public GameObject restartButton;
    public Text timeText; // ✅ Legacy Text
    private float elapsedTime = 0f;
    private bool gameRunning = true;

    int score = 0;

    void Update()
    {
        if (gameRunning)
        {
            elapsedTime += Time.deltaTime;
            timeText.text = FormatTime(elapsedTime);
        }
    }

    public void ScoreUp()
    {
        score++;
        if (score >= 4)
        {
            Win();
        }
    }

    void Win()
    {
        winText.SetActive(true);
        restartButton.SetActive(true);
        gameRunning = false;

        // Save raw time as high-precision decimal string
        decimal timeRaw = (decimal)elapsedTime;
        PlayerPrefs.SetString("Game5_SubmitTimeRaw", timeRaw.ToString());

        // Save formatted time string for display
        PlayerPrefs.SetString("Game5_SubmitTime", FormatTime(elapsedTime));
        SceneManager.LoadScene("Submit Score and Name Game 5");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game 5 Ball Smash");
    }

    string FormatTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        int centiseconds = (int)((time * 100f) % 100f);
        return $"{minutes:00}:{seconds:00}.{centiseconds:00}";
    }
}
