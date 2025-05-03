[System.Serializable]
public class TimeScoreData
{
    public string player_name;
    public string time_raw;  // Time as string, we'll parse this to decimal in Unity
    public string formatted_time;  // Formatted time string like "00:02.19"
}

[System.Serializable]
public class TimeScoreArray
{
    public TimeScoreData[] scores;  // Array of score data
}