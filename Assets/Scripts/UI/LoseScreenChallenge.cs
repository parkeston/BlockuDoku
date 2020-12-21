using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreenChallenge : UIPanel
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
}
