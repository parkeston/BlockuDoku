using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMode
{
    private Dictionary<(int month,int year),MonthChallengeSet> monthChallengeSets;
    
    public bool IsChallengeMode => CurrentChallenge!=null;
    public Challenge CurrentChallenge { get; private set; }

    public GameMode()
    {
        monthChallengeSets = Resources.LoadAll<MonthChallengeSet>("Challenges/")
            .ToDictionary(challengeSet => challengeSet.Date,challengeSet => challengeSet);
        SetDefaultMode();
    }

    public void SetDefaultMode()
    {
        CurrentChallenge = null;
    }

    public void SetChallengeMode(DateTime dateTime)
    {
        int month = dateTime.Month;
        int year = dateTime.Year;
        int day = dateTime.Day;
        CurrentChallenge = monthChallengeSets.ContainsKey((month, year)) ? monthChallengeSets[(month, year)].GetChallenge(day) : null;
    }
}
