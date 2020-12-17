using System;
using System.Collections;
using UnityEngine;

public class ChallengeScreen : MonoBehaviour
{
    [SerializeField] private Vector2Int maxMonthOffsetFromCurrent;

    [Space]
    [SerializeField] private SnapScrollRect snapScrollRect;
    [SerializeField] private GameObject monthCupPrefab;
    [SerializeField] private ChallengeCalendar challengeCalendar;

    private int totalMonthOffsetFromCurrent;
    private void Awake()
    {
        var cupsCount = maxMonthOffsetFromCurrent.y - maxMonthOffsetFromCurrent.x+1;
        for (int i = 0; i < cupsCount; i++)
            Instantiate(monthCupPrefab, snapScrollRect.content, false);
    }

    private void OnEnable()
    {
        snapScrollRect.OnSnapping += ChangeChallengeMonth;
    }
    
    private void OnDisable()
    {
        snapScrollRect.OnSnapping -= ChangeChallengeMonth;
    }

    private void Start()
    {
        totalMonthOffsetFromCurrent = maxMonthOffsetFromCurrent.x;
        snapScrollRect.InvokeSnap(-maxMonthOffsetFromCurrent.x);
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
}
