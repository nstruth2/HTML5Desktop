using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreSystemFrom3GamesPackGame2 : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text bestTimeText;

    public const string BestTimeKey = "BestTime3GamesPackGame2";
    public const string LastTimeKey = "LastTime3GamesPackGame2";
    public const string BeatBestTimeKey = "BeatBestTime3GamesPackGame2";
    public const string Game3_SubmitTimeRaw = "Game3_SubmitTimeRaw";

    private decimal timeElapsed;
    private decimal bestTime;
    private bool gameEnded;

    void Start()
    {
        // Load best time from PlayerPrefs (use decimal here)
        string bestTimeStr = PlayerPrefs.GetString(BestTimeKey, "00:00.00");
        bestTime = ParseTime(bestTimeStr);

        timeText.text = "Time: 00:00.00";
        bestTimeText.text = "Best Time: " + (bestTime > 0 ? FormatTime(bestTime) : "--:--.--");
    }

    void Update()
    {
        if (gameEnded) return;

        // Increment timeElapsed by the time spent per frame
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

        // Store raw decimal time and formatted time
        string lastTimeStr = FormatTime(timeElapsed);
        PlayerPrefs.SetString(LastTimeKey, lastTimeStr);
        PlayerPrefs.SetString(Game3_SubmitTimeRaw, timeElapsed.ToString()); // Save the raw decimal time
        
        // Check if the player beat the best time
        if (bestTime == 0 || timeElapsed > bestTime)
        {
            bestTime = timeElapsed;
            string bestTimeStr = FormatTime(bestTime);
            PlayerPrefs.SetString(BestTimeKey, bestTimeStr); // Save the best time as formatted string
            PlayerPrefs.SetInt(BeatBestTimeKey, 1);
        }
        else
        {
            PlayerPrefs.SetInt(BeatBestTimeKey, 0);
        }

        PlayerPrefs.Save();

        // Load the next scene after saving the data
        StartCoroutine(DelayedSceneLoad());
    }

    private System.Collections.IEnumerator DelayedSceneLoad()
    {
        Debug.Log("Coroutine started. Waiting for 1 second...");
        yield return new WaitForSeconds(.1f); // Delay ensures PlayerPrefs save completes
        Debug.Log("Loading scene: Game Over Game 3");
        SceneManager.LoadScene("Game Over Game 3");
    }

    private string FormatTime(decimal time)
    {
        int minutes = (int)(time / 60m);               // Extract minutes
        int seconds = (int)(time % 60m);               // Extract seconds
        int centiseconds = (int)((time * 100m) % 100m); // Extract centiseconds
        return $"{minutes:00}:{seconds:00}.{centiseconds:00}"; // Format as MM:SS.CC
    }

    private decimal ParseTime(string timeStr)
    {
        string[] parts = timeStr.Split(':', '.'); // Split the formatted string into parts
        if (parts.Length == 3)
        {
            int minutes = int.Parse(parts[0]);
            int seconds = int.Parse(parts[1]);
            int centiseconds = int.Parse(parts[2]);

            decimal time = (decimal)minutes * 60 + (decimal)seconds + (decimal)centiseconds / 100;
            return time;
        }
        return 0m; // Return 0 if the format is invalid
    }
}
