using System;

[Serializable]
public class HighScoreData
{
    public string player_name;
    public int score;
}

[Serializable]
public class HighScoreArray
{
    public HighScoreData[] scores;
}