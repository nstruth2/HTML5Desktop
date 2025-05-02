using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class HighScoreFetcherGame3 : MonoBehaviour
{
    public TMP_Text bestTimeText;
    public string currentBestTime = "";
    public string currentBestTimePlayer = "";
    public bool hasFetched = false;

    void Start()
    {
        hasFetched = false;
        StartCoroutine(FetchTopTimes());
    }

    IEnumerator FetchTopTimes()
    {
        string url = "https://ourgoodguide.com/MobileProject/GetTopScoresandNames/get_top_time_game_3.php";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                Debug.Log("Received JSON: " + json);

                if (!string.IsNullOrEmpty(json))
                {
                    TimeScoreArray topTimes = JsonUtility.FromJson<TimeScoreArray>("{\"scores\":" + json + "}");

                    if (topTimes != null && topTimes.scores.Length > 0)
                    {
                        string bestTime = topTimes.scores[0].time;
                        List<string> topPlayers = new List<string>();

                        foreach (TimeScoreData scorer in topTimes.scores)
                        {
                            if (scorer.time == bestTime)
                            {
                                topPlayers.Add(scorer.player_name);
                            }
                        }

                        currentBestTime = bestTime;
                        currentBestTimePlayer = topPlayers[0];

                        if (topPlayers.Count == 1)
                        {
                            bestTimeText.text = $"Best Time: {currentBestTimePlayer}: {currentBestTime}";
                        }
                        else
                        {
                            topPlayers.Sort();
                            bestTimeText.text = $"Best Time: {string.Join(", ", topPlayers)}: {currentBestTime}";
                        }

                        hasFetched = true;
                    }
                    else
                    {
                        bestTimeText.text = "No best times yet.";
                        hasFetched = true;
                    }
                }
                else
                {
                    bestTimeText.text = "No best times yet.";
                    hasFetched = true;
                }
            }
            else
            {
                Debug.LogError("Error fetching best times: " + www.error);
                bestTimeText.text = "Failed to load times.";
                hasFetched = true;
            }
        }
    }
}
