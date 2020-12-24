using System;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeScreen : UIPanel
{
    [Space]
    [SerializeField] private SnapScrollRect snapScrollRect;
    [SerializeField] private ChallengeAwardCalendarPreview awardCalendarPreview;
    [SerializeField] private ChallengeCalendar challengeCalendar;
    [SerializeField] private Button playButton;

    private int currentChallengeIndex;
    private MonthChallengeSet[] availableChallenges;
    
    public override void Init()
    {
        availableChallenges = GameManager.Instance.GameMode.GetAvailableChallenges();
        foreach (var challenge in availableChallenges)
            Instantiate(awardCalendarPreview, snapScrollRect.content, false)
                .SetImage(challenge.CupImage);

        snapScrollRect.OnSnapping += ChangeChallengeMonth;
        playButton.onClick.AddListener(PlayChallenge);
    }

    protected override void OnShown()
    {
        //set challenge scroll at the beginning & then snap to the last available challenge
        currentChallengeIndex = 0;
        snapScrollRect.horizontalNormalizedPosition = 0;
        snapScrollRect.InvokeSnap(availableChallenges.Length-1,true);
    }

    private void ChangeChallengeMonth(int challengeIndexDelta)
    {
        if (currentChallengeIndex+challengeIndexDelta<0 || currentChallengeIndex+challengeIndexDelta>availableChallenges.Length-1)
            return;

        currentChallengeIndex += challengeIndexDelta;
        challengeCalendar.ChangeMonth(availableChallenges[currentChallengeIndex]);
    }

    private void PlayChallenge() => GameManager.Instance.Play(challengeCalendar.GetSelectedDate());
}
