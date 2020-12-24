using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeAwardListItem : MonoBehaviour
{
    [SerializeField] private Image cupImage;
    [SerializeField] private TMP_Text awardMonth;

    [Space]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float lockedAlpha;
    [SerializeField] private float unlockedAlpha;

    public void SetAwardData(MonthChallengeSet monthChallengeSet)
    {
        cupImage.sprite = monthChallengeSet.CupImage;

        var awardDate = monthChallengeSet.Date;
        awardMonth.text = new DateTime(awardDate.year, awardDate.month, 1).ToString("MMMM");

        canvasGroup.alpha = monthChallengeSet.IsCompleted ? unlockedAlpha : lockedAlpha;
    }
}
