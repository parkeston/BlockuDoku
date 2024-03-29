﻿
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : UIPanel
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueChallengeButton;
    [SerializeField] private ScoreGroup highScore;
    [SerializeField] private TMP_Text todayChallengeDate;

    public override void Init()
    {
        newGameButton.onClick.AddListener(PlayDefaultMode);
        continueChallengeButton.onClick.AddListener(PlayChallengeMode);
    }

    protected override void OnShown()
    {
        highScore.ScoreText.text = GameManager.Instance.GameScore.CurrentHighScore.ToString();
        todayChallengeDate.text = DateTime.Today.ToString("M");
    }

    private void PlayDefaultMode() => GameManager.Instance.Play();
    private void PlayChallengeMode() => GameManager.Instance.Play(DateTime.Today);
}
