using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScoreFetcherGame1 : MonoBehaviour
{
    public Text highScoreText; // Assign this in the Inspector
    public int currentHighScore = 0;
    public string currentHighScorePlayer = "";
    public bool hasFetched = false; // <-- Added to indicate fetch completion

    void Start()
    {
        hasFetched = false;
        StartCoroutine(FetchTopScorers());
    }

    IEnumerator FetchTopScorers()
    {
        string url = "https://ourgoodguide.com/HTML5DesktopProject/GetTopScoresandNames/get_top_score_game_1.php";
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

                        if (topPlayers.Count == 1)
                        {
                            highScoreText.text = $"Top Score: {currentHighScorePlayer}: {currentHighScore}";
                        }
                        else
                        {
                            topPlayers.Sort();
                            highScoreText.text = $"Top Score: {string.Join(", ", topPlayers)}: {currentHighScore}";
                        }

                        hasFetched = true;
                    }
                    else
                    {
                        highScoreText.text = "No high scores yet.";
                        hasFetched = true;
                    }
                }
                else
                {
                    highScoreText.text = "No high scores yet.";
                    hasFetched = true;
                }
            }
            else
            {
                Debug.LogError("Error fetching top scorers: " + www.error);
                highScoreText.text = "Failed to load scores.";
                hasFetched = true;
            }
        }
    }
}
