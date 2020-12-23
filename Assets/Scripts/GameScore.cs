using System;
using UnityEngine;

public class GameScore
{
    private int scoreForComboCell = 2;
    public int CurrentHighScore { get; private set; }
    public int CurrentScore { get; private set; }
    public bool IsNewHighScore { get; private set; }

    public event Action<int> OnScoreChanged;

    public GameScore() => ResetScore();

    public void ResetScore()
    {
        CurrentHighScore = PlayerPrefs.GetInt("HighScore",0);
        CurrentScore = 0;
        IsNewHighScore = false;
    }

    public void AddScore(int cells)
    {
        int earnedPoints = cells * (cells<9?1:scoreForComboCell);
        CurrentScore += earnedPoints;

        if (CurrentScore > CurrentHighScore && !IsNewHighScore)
            IsNewHighScore = true;
        
        OnScoreChanged?.Invoke(earnedPoints);
    }
    
    public void TrySaveNewHighScore()
    {
        if (IsNewHighScore)
        {
            PlayerPrefs.SetInt("HighScore", CurrentScore);
            CurrentHighScore = CurrentScore;
        }
    }
}
