using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMode
{
    private Dictionary<(int month,int year),MonthChallengeSet> monthChallengeSets;
    
    public bool IsChallengeMode => CurrentChallenge!=null;
    public DateTime ChallengeDate { get; private set; }
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
        ChallengeDate = dateTime;
        CurrentChallenge = monthChallengeSets.ContainsKey((month, year)) ? monthChallengeSets[(month, year)].GetChallenge(day) : null;
    }

    public void SetChallengeCompleted()
    { 
        if(monthChallengeSets.ContainsKey((ChallengeDate.Month,ChallengeDate.Year)))
            monthChallengeSets[(ChallengeDate.Month,ChallengeDate.Year)].SetChallengeCompleted(ChallengeDate.Day);
    }

    public bool IsChallengeCompleted()
    {
        return monthChallengeSets.ContainsKey((ChallengeDate.Month,ChallengeDate.Year)) 
               && monthChallengeSets[(ChallengeDate.Month,ChallengeDate.Year)].IsChallengeCompleted(ChallengeDate.Day);
    }

    public MonthChallengeSet[] GetAvailableChallenges()
    {
        return monthChallengeSets.Values.ToArray();
    }
}
