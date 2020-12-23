using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScoreChallengeUI : UIPanel
{
    [SerializeField] private AnimatedCounter pointsAnimatedCounter;
    
    [Space]
    [SerializeField] private TMP_Text challengePointsToPass;
    [SerializeField] private Image challengePointsProgress;
    [SerializeField] private TMP_Text challengeDate;
    
    [Space]
    [SerializeField] private Button backButton;

    public override void Init()
    {
        backButton.onClick.AddListener(GameManager.Instance.ToMainMenu);
    }

    protected override void OnShown()
    {
        challengePointsProgress.fillAmount = GameManager.Instance.ChallengeProgress;
        pointsAnimatedCounter.ResetCounterTo(GameManager.Instance.GameScore.CurrentScore);
        
        challengePointsToPass.text = GameManager.Instance.GameMode.CurrentChallenge.PointsToPass.ToString();
        challengeDate.text = GameManager.Instance.GameMode.ChallengeDate.ToString("M");
        
        GameManager.Instance.GameScore.OnScoreChanged += UpdateScore;
    }

    protected override void OnHide()
    {
        GameManager.Instance.GameScore.OnScoreChanged -= UpdateScore;
        Close();
    }

    private void UpdateScore(int earnedPoints)
    {
        challengePointsProgress.fillAmount = GameManager.Instance.ChallengeProgress;
        pointsAnimatedCounter.SetValue(GameManager.Instance.GameScore.CurrentScore);
    }
}
