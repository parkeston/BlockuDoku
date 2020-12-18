using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeScreen : UIPanel
{
    [SerializeField] private Vector2Int maxMonthOffsetFromCurrent;

    [Space]
    [SerializeField] private SnapScrollRect snapScrollRect;
    [SerializeField] private GameObject monthCupPrefab;
    [SerializeField] private ChallengeCalendar challengeCalendar;
    [SerializeField] private Button playButton;

    private int totalMonthOffsetFromCurrent;

    public override void Init()
    {
        var cupsCount = maxMonthOffsetFromCurrent.y - maxMonthOffsetFromCurrent.x+1;
        for (int i = 0; i < cupsCount; i++)
            Instantiate(monthCupPrefab, snapScrollRect.content, false);
        
        snapScrollRect.OnSnapping += ChangeChallengeMonth;
        playButton.onClick.AddListener(PlayChallenge);
    }

    protected override void OnShown()
    {
        totalMonthOffsetFromCurrent = maxMonthOffsetFromCurrent.x;
        snapScrollRect.horizontalNormalizedPosition = 0;
        snapScrollRect.InvokeSnap(-maxMonthOffsetFromCurrent.x,true);
    }

    private void ChangeChallengeMonth(int monthDelta)
    {
        totalMonthOffsetFromCurrent += monthDelta;
        if (totalMonthOffsetFromCurrent > maxMonthOffsetFromCurrent.y ||
            totalMonthOffsetFromCurrent < maxMonthOffsetFromCurrent.x)
        {
            totalMonthOffsetFromCurrent = Mathf.Clamp(
                totalMonthOffsetFromCurrent, maxMonthOffsetFromCurrent.x, maxMonthOffsetFromCurrent.y);
            return;
        }
        
        challengeCalendar.ChangeMonth(totalMonthOffsetFromCurrent);
    }

    private void PlayChallenge() => GameManager.Instance.Play(true);
}
