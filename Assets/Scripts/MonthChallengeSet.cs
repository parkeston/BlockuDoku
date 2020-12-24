using System;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu]
public class MonthChallengeSet : ScriptableObject
{
    [SerializeField] private int month;
    [SerializeField] private int year;

    [Space] 
    [SerializeField] private Sprite cupImage;
    [SerializeField] private bool isCompleted;
    
    [Space]
    [SerializeField] private Challenge[] challenges;
    
    public (int month, int year) Date => (month,year);
    public Sprite CupImage => cupImage;
    public bool IsCompleted => isCompleted;

    private void OnValidate()
    {
        if (challenges.Length != DateTime.DaysInMonth(year, month))
            Array.Resize(ref challenges,DateTime.DaysInMonth(year, month));
    }


    public Challenge GetChallenge(int day)
    {
        return day > challenges.Length ? null : challenges[day - 1];
    }

    public void SetChallengeCompleted(int day)
    {
        if (day <= challenges.Length)
            challenges[day - 1].IsCompleted = true;

        isCompleted = challenges.Count(challenge => challenge.IsCompleted) == challenges.Length;
    }

    public bool IsChallengeCompleted(int day)
    {
        if (day <= challenges.Length)
            return challenges[day - 1].IsCompleted;
        return false;
    }

    public bool[] GetCompletionState()
    {
        bool[] completionState = new bool[challenges.Length];
        for (int i = 0; i < challenges.Length; i++)
            completionState[i] = challenges[i].IsCompleted;
        return completionState;
    }

    public string GetProgressString()
    {
        int completed = challenges.Count(challenge => challenge.IsCompleted);
        return $"{completed}/{challenges.Length}";
    }
}
