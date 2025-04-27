using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScoreFetcherGame4PreStripPlayerPrefs : MonoBehaviour
{
    public Text highScoreText; // Assign this in the Inspector (or use TextMeshProUGUI if needed)
    private int currentHighScore = 0;
    private string currentHighScorePlayer = "";
    
    // Optional: set this if you're comparing against the player's current score
    public int score = 0;

    void Start()
    {
        LoadTopScoreFromPlayerPrefs(); // Show saved score immediately
        StartCoroutine(FetchTopScorers());
    }

    void SaveTopScoreToPlayerPrefs(string playerName, int score)
    {
        PlayerPrefs.SetString("Game4_TopScorePlayer", playerName);
        PlayerPrefs.SetInt("Game4_TopScoreValue", score);
        PlayerPrefs.Save();
    }

    void LoadTopScoreFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Game4_TopScorePlayer") && PlayerPrefs.HasKey("Game4_TopScoreValue"))
        {
            string savedPlayer = PlayerPrefs.GetString("Game4_TopScorePlayer");
            int savedScore = PlayerPrefs.GetInt("Game4_TopScoreValue");
            highScoreText.text = $"Saved Top Score: {savedPlayer} {savedScore}";
        }
    }

    IEnumerator FetchTopScorers()
    {
        string url = "https://ourgoodguide.com/MobileProject/get_top_score_game_4.php";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                Debug.Log("Received JSON: " + json);

                if (!string.IsNullOrEmpty(json))
                {
                    HighScoreArray topScorers = JsonUtility.FromJson<HighScoreArray>("{\"scores\":" + json + "}");

                    if (topScorers.scores.Length > 0)
                    {
                        int highestScore = topScorers.scores[0].score;
                        List<string> topPlayers = new List<string>();

                        foreach (HighScoreData scorer in topScorers.scores)
                        {
                            if (scorer.score == highestScore)
                            {
                                topPlayers.Add(scorer.player_name);
                            }
                        }

                        if (topPlayers.Count == 1)
                        {
                            if (score <= currentHighScore)
                            {
                                highScoreText.text = $"Top Score: {topPlayers[0]}: {highestScore}";
                            }
                            else
                            {
                                highScoreText.text = $"Top Score: {topPlayers[0]} {highestScore}";
                            }
                            currentHighScorePlayer = topPlayers[0];
                        }
                        else
                        {
                            highScoreText.text = $"Top Scorers: {string.Join(", ", topPlayers)} {highestScore}";
                            currentHighScorePlayer = topPlayers[0];
                        }

                        currentHighScore = highestScore;
                        SaveTopScoreToPlayerPrefs(currentHighScorePlayer, currentHighScore);
                    }
                    else
                    {
                        highScoreText.text = "No high scores yet.";
                    }
                }
                else
                {
                    highScoreText.text = "No high scores yet.";
                }
            }
            else
            {
                Debug.LogError("Error fetching top scorers: " + www.error);
                highScoreText.text = "Failed to load scores.";
            }
        }
    }
}