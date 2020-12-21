using UnityEngine;

public class GameScoreUI : UIPanel
{
    [SerializeField] private AnimatedCounter pointsAnimatedCounter;
    [SerializeField] private AnimatedPopup pointsAnimatedPopup;

    [Space] 
    [SerializeField] private ScoreGroup highScoreGroup;
    [SerializeField] private ScoreGroup currentPointsGroup;

    public override void Init()
    {
        GameManager.Instance.GameScore.OnScoreAdded += ShowScoreAdditionPopup;
    }

    protected override void OnShown()
    {
        highScoreGroup.ScoreText.text = GameManager.Instance.GameScore.CurrentHighScore.ToString();
        highScoreGroup.gameObject.SetActive(GameManager.Instance.GameScore.CurrentHighScore>0);
        
        currentPointsGroup.ScoreText.text =  GameManager.Instance.GameScore.CurrentScore.ToString();
        currentPointsGroup.ScoreIcon.SetActive(false);
        
        pointsAnimatedCounter.ResetCounterTo(GameManager.Instance.GameScore.CurrentScore);
    }

    private void ShowScoreAdditionPopup(int earnedPoints,Vector3 comboPopupPosition)
    {
        pointsAnimatedCounter.SetValue(GameManager.Instance.GameScore.CurrentScore);
        CheckForNewHighScore();

        if(earnedPoints<18) //not a combo
            return;
       
        pointsAnimatedPopup.transform.position = comboPopupPosition;
        if(earnedPoints>18) //multi-combo
            pointsAnimatedPopup.PlayComboPopup(earnedPoints.ToString());
        else
            pointsAnimatedPopup.PlayPopup(earnedPoints.ToString());
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
