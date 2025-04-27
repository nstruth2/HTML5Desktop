using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScoreFetcherGame4 : MonoBehaviour
{
    public Text highScoreText; // Assign this in the Inspector
    public int currentHighScore = 0;
    public string currentHighScorePlayer = "";

    void Start()
    {
        StartCoroutine(FetchTopScorers());
    }

    IEnumerator FetchTopScorers()
    {
        string url = "https://ourgoodguide.com/MobileProject/GetTopScoresandNames/get_top_score_game_4.php";
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

                    if (topScorers != null && topScorers.scores.Length > 0)
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

                        currentHighScore = highestScore;
                        currentHighScorePlayer = topPlayers[0];

                        // If there's one player with the highest score, show "Top Score: PlayerName: Score"
                        if (topPlayers.Count == 1)
                        {
                            highScoreText.text = $"Top Score: {currentHighScorePlayer}: {currentHighScore}";
                        }
                        else
                        {
                            // Sort players alphabetically if there are multiple top scorers
                            topPlayers.Sort();
                            highScoreText.text = $"Top Score: {string.Join(", ", topPlayers)}: {currentHighScore}";
                        }
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
