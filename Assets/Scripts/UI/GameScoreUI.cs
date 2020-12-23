using UnityEngine;
using UnityEngine.UI;

public class GameScoreUI : UIPanel
{
    [SerializeField] private AnimatedCounter pointsAnimatedCounter;

    [Space] 
    [SerializeField] private ScoreGroup highScoreGroup;
    [SerializeField] private ScoreGroup currentPointsGroup;

    [Space]
    [SerializeField] private Button backButton;

    public override void Init()
    {
        backButton.onClick.AddListener(GameManager.Instance.ToMainMenu);
    }

    protected override void OnShown()
    {
        highScoreGroup.ScoreText.text = GameManager.Instance.GameScore.CurrentHighScore.ToString();
        highScoreGroup.gameObject.SetActive(GameManager.Instance.GameScore.CurrentHighScore>0);
        
        currentPointsGroup.ScoreText.text =  GameManager.Instance.GameScore.CurrentScore.ToString();
        currentPointsGroup.ScoreIcon.SetActive(false);
        
        pointsAnimatedCounter.ResetCounterTo(GameManager.Instance.GameScore.CurrentScore);
        
        GameManager.Instance.GameScore.OnScoreChanged += UpdateScore;
    }

    protected override void OnHide()
    {
        GameManager.Instance.GameScore.OnScoreChanged -= UpdateScore;
        Close();
    }

    private void UpdateScore(int earnedPoints)
    {
        pointsAnimatedCounter.SetValue(GameManager.Instance.GameScore.CurrentScore);
        CheckForNewHighScore();
    }

    private void CheckForNewHighScore()
    {
        if (highScoreGroup.gameObject.activeSelf && GameManager.Instance.GameScore.IsNewHighScore)
        {
            //todo: play new high score animation
            currentPointsGroup.ScoreIcon.SetActive(true);
            highScoreGroup.gameObject.SetActive(false);
        }
    }
}
