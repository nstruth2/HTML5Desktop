using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MainMenuFrom3GamePackGame2 : MonoBehaviour
{
    [SerializeField] private TMP_Text bestTimeText;
    [SerializeField] private TMP_Text lastTimeText;
    [SerializeField] private TMP_Text messageText;

    void Start()
    {
        string bestTimeStr = PlayerPrefs.GetString(ScoreSystemFrom3GamesPackGame2.BestTimeKey, "00:00.00");
        decimal bestTime = ParseTime(bestTimeStr);
        Debug.Log("Loaded Best Time: " + bestTimeStr); // Debugging loaded best time

        string lastTimeStr = PlayerPrefs.GetString(ScoreSystemFrom3GamesPackGame2.LastTimeKey, "00:00.00");
        decimal lastTime = ParseTime(lastTimeStr);
        Debug.Log("Loaded Last Time: " + lastTimeStr); // Debugging loaded last time

        int beatBest = PlayerPrefs.GetInt(ScoreSystemFrom3GamesPackGame2.BeatBestTimeKey, 0);

        // Update UI with the retrieved best time and last time
        bestTimeText.text = "Best Time: " + (bestTime > 0 ? FormatTime(bestTime) : "--:--.--");
        lastTimeText.text = "Last Time: " + (lastTime > 0 ? FormatTime(lastTime) : "--:--.--");
        messageText.text = beatBest == 1 ? "You beat the best time!" : "";

        // Debugging to ensure values are loaded correctly
        Debug.Log("Loaded Best Time: " + bestTime);
        Debug.Log("Loaded Last Time: " + lastTime);
    }

    public void Play()
    {
        SceneManager.LoadScene("Gameplay Game 3");
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    string FormatTime(decimal time)
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
            Debug.Log($"Parsed Time: {timeStr} -> {time}"); // Debugging parsed time
            return time;
        }
        return 0m;
    }
}