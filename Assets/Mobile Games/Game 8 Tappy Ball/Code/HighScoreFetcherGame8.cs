using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScoreFetcherGame8 : MonoBehaviour
{
    public int currentHighScore = 0;
    public string currentHighScorePlayer = "";
    public bool hasFetched = false;

    // Optional UI Text to display high score in Main Menu Game 8
    public Text highScoreText;

    void Start()
    {
        hasFetched = false;
        StartCoroutine(FetchTopScorers());
    }

    IEnumerator FetchTopScorers()
    {
        string url = "https://ourgoodguide.com/HTML5DesktopProject/GetTopScoresandNames/get_top_score_game_8.php";
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

                        // ✅ Update UI text only if available
                        if (highScoreText != null)
                        {
                            highScoreText.text = $"Top Score: {currentHighScorePlayer}: {currentHighScore}";
                        }

                        // ✅ Notify GameManager (in-game only)
                        GameManagerGame8.instance?.SetFetchedHighScore(currentHighScore, currentHighScorePlayer);

                        hasFetched = true;
                    }
                    else
                    {
                        Debug.Log("No high scores yet.");
                        hasFetched = true;
                    }
                }
                else
                {
                    Debug.Log("Empty response for high scores.");
                    hasFetched = true;
                }
            }
            else
            {
                Debug.LogError("Error fetching top scorers: " + www.error);
                hasFetched = true;
            }
        }
    }
}
