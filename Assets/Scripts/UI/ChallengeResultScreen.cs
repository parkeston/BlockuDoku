﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeResultScreen : UIPanel
{
    [SerializeField] private TMP_Text challengeProgressTitle;
    [SerializeField] private Image challengeProgressBar;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button challengesButton;

    public override void Init()
    {
        retryButton.onClick.AddListener( GameManager.Instance.Retry);
        challengesButton.onClick.AddListener(GameManager.Instance.ToMainMenu); //todo: return to specific tab of main menu!
    }

    protected override void OnShown()
    {
        //todo: animate
        float progress = GameManager.Instance.ChallengeProgress;
        challengeProgressBar.fillAmount = progress;
        challengeProgressTitle.text = $"{(int)(progress*100)}% Done";
        
        retryButton.gameObject.SetActive(progress<1);
    }
}
