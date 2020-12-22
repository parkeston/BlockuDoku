using UnityEngine;

[CreateAssetMenu]
public class MonthChallengeSet : ScriptableObject
{
    [SerializeField] private int month;
    [SerializeField] private int year;
    
    [SerializeField] private Challenge[] challenges;

    public (int month, int year) Date => (month,year);

    public Challenge GetChallenge(int day)
    {
        return day > challenges.Length ? null : challenges[day - 1];
    }
}
