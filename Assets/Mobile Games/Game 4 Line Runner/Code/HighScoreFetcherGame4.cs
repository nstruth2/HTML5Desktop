using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScoreFetcherGame4 : MonoBehaviour
{
    public Text highScoreText; // Assign this in the Inspector (or use TextMeshProUGUI if needed)
    private int currentHighScore = 0;
    private string currentHighScorePlayer = "";
    
    // Optional: set this if you're comparing against the player's current score
    public int score = 0;

    void Start()
    {
        StartCoroutine(FetchTopScorers());
    }

    IEnumerator FetchTopScorers()
    {
        string url = "https://ourgoodguide.com/MobileProject/get_top_scorers.php";
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

[System.Serializable]
public class HighScoreData
{
    public string player_name;
    public int score;
}

[System.Serializable]
public class HighScoreArray
{
    public HighScoreData[] scores;
}
