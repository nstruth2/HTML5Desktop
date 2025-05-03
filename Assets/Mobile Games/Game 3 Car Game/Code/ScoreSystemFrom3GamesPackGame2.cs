using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreSystemFrom3GamesPackGame2 : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text bestTimeText;
    [SerializeField] private TMP_Text messageText;

    public const string BestTimeKey = "BestTime3GamesPackGame2";
    public const string LastTimeKey = "LastTime3GamesPackGame2";
    public const string BeatBestTimeKey = "BeatBestTime3GamesPackGame2";

    private decimal timeElapsed;
    private decimal bestTime;
    private bool gameEnded;

    void Start()
    {
        string bestTimeStr = PlayerPrefs.GetString(BestTimeKey, "00:00.00");
        bestTime = ParseTime(bestTimeStr);

        timeText.text = "Time: 00:00.00";
        messageText.text = "";
        bestTimeText.text = "Best Time: " + (bestTime > 0 ? FormatTime(bestTime) : "--:--.--");
    }

    void Update()
    {
        if (gameEnded) return;

        timeElapsed += (decimal)Time.deltaTime;
        timeText.text = "Time: " + FormatTime(timeElapsed);

        // Update best time text live, checking for longer times
        if (bestTime == 0 || timeElapsed > bestTime)
        {
            bestTimeText.text = "Best Time: " + FormatTime(timeElapsed);
        }
    }

    public void OnGameEnd()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("Game Ended, starting scene load.");

        string lastTimeStr = FormatTime(timeElapsed);
        PlayerPrefs.SetString(LastTimeKey, lastTimeStr);
        PlayerPrefs.SetString("Game3_SubmitTime", lastTimeStr);              // 🔹 formatted
        PlayerPrefs.SetString("Game3_SubmitTimeRaw", timeElapsed.ToString()); // 🔹 raw decimal

        if (bestTime == 0 || timeElapsed > bestTime)
        {
            bestTime = timeElapsed;
            string bestTimeStr = FormatTime(bestTime);
            PlayerPrefs.SetString(BestTimeKey, bestTimeStr);
            PlayerPrefs.SetInt(BeatBestTimeKey, 1);
            messageText.text = "You beat the best time!";
        }
        else
        {
            PlayerPrefs.SetInt(BeatBestTimeKey, 0);
        }

        PlayerPrefs.Save();
        StartCoroutine(DelayedSceneLoad());
    }


    private System.Collections.IEnumerator DelayedSceneLoad()
    {
        Debug.Log("Coroutine started. Waiting for 1 second...");
        yield return new WaitForSeconds(1f); // delay ensures PlayerPrefs save completes
        SceneManager.LoadScene("Submit Time and Name Game 3");  // Load the specific scene directly
    }

    private string FormatTime(decimal time)
    {
        int minutes = (int)(time / 60m);
        int seconds = (int)(time % 60m);
        int centiseconds = (int)((time * 100m) % 100m);
        return $"{minutes:00}:{seconds:00}.{centiseconds:00}";
    }

    private decimal ParseTime(string timeStr)
    {
        string[] parts = timeStr.Split(':', '.');
        if (parts.Length == 3)
        {
            int minutes = int.Parse(parts[0]);
            int seconds = int.Parse(parts[1]);
            int centiseconds = int.Parse(parts[2]);

            decimal time = (decimal)minutes * 60 + (decimal)seconds + (decimal)centiseconds / 100;
            return time;
        }
        return 0m;
    }
}
