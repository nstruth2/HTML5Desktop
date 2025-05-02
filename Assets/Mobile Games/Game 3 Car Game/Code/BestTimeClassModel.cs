[System.Serializable]
public class TimeScoreData
{
    public string player_name;
    public string time;
}

[System.Serializable]
public class TimeScoreArray
{
    public TimeScoreData[] scores;
}