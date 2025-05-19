using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class HighScoreFetcherGame5 : MonoBehaviour
{
    public Text bestTimeText;
    public string currentBestTime = "";
    public string currentBestTimePlayer = "";
    public bool hasFetched = false;

    void Start()
    {
        hasFetched = false;
        StartCoroutine(FetchBestTimes());
    }

   IEnumerator FetchBestTimes()
    {
        string url = "https://ourgoodguide.com/HTML5DesktopProject/GetTopScoresandNames/get_best_time_game_5.php";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                Debug.Log("Received JSON: " + json);

                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        // Deserialize JSON into TimeScoreArray
                        TimeScoreArray topTimes = JsonUtility.FromJson<TimeScoreArray>("{\"scores\":" + json + "}");

                        if (topTimes != null && topTimes.scores.Length > 0)
                        {
                            decimal bestTime = 0m;

                            // Try to parse the best time safely
                            if (!decimal.TryParse(topTimes.scores[0].time_raw, out bestTime))
                            {
                                Debug.LogError("Failed to parse best time.");
                                bestTimeText.text = "Error parsing best times.";
                                hasFetched = true;
                                yield break;
                            }

                            List<string> topPlayers = new List<string>();

                            foreach (TimeScoreData scorer in topTimes.scores)
                            {
                                decimal rawTime = 0m;

                                // Try to parse each player's raw time safely
                                if (decimal.TryParse(scorer.time_raw, out rawTime) && rawTime == bestTime)
                                {
                                    topPlayers.Add(scorer.player_name);
                                }
                            }

                            currentBestTime = bestTime.ToString("0.###");  // Format to a decimal string (you can adjust precision)
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
                    catch (System.Exception ex)
                    {
                        Debug.LogError("Error parsing JSON: " + ex.Message);
                        bestTimeText.text = "Error parsing best times.";
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