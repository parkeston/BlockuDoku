using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private AnimatedPopup pointsAnimatedPopup;

    private void Start()
    {
        GameManager.Instance.GameScore.OnScoreChanged += ShowScoreAdditionPopup;
    }

    private void ShowScoreAdditionPopup(int earnedPoints)
    {
        if(earnedPoints<18)
            return;
        
        if(earnedPoints>18) //multi-combo
            pointsAnimatedPopup.PlayComboPopup(earnedPoints.ToString());
        else
            pointsAnimatedPopup.PlayPopup(earnedPoints.ToString());
    }
}
