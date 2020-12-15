using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScore : MonoBehaviour
{
    [SerializeField] private AnimatedCounter pointsAnimatedCounter;
    [SerializeField] private AnimatedPopup pointsAnimatedPopup;

    [Space] 
    [SerializeField] private ScoreGroup highScoreGroup;
    [SerializeField] private ScoreGroup currentPointsGroup;
    
    private int scoreForComboCell = 2;
    
    public int CurrentScore { get; private set; }
    private int currentHighScore;

    private void OnEnable()
    {
        currentHighScore = PlayerPrefs.GetInt("HighScore",0);
        highScoreGroup.ScoreText.text = currentHighScore.ToString();
        highScoreGroup.gameObject.SetActive(currentHighScore>0);
        
        currentPointsGroup.ScoreText.text = 0.ToString();
        currentPointsGroup.ScoreIcon.SetActive(false);
    }

    //todo: merge in one method
    public void AddScore(int cells,Vector3 comboPopupPosition)
    {
        int earnedPoints = cells * (cells<9?1:scoreForComboCell);
        CurrentScore += earnedPoints;
        
        pointsAnimatedCounter.SetValue(CurrentScore);
        CheckForNewHighScore();

        if(cells<9) //not a combo
            return;
       
        pointsAnimatedPopup.transform.position = comboPopupPosition;
        if(cells>9) //multi-combo
            pointsAnimatedPopup.PlayComboPopup(earnedPoints.ToString());
        else
            pointsAnimatedPopup.PlayPopup(earnedPoints.ToString());
    }

    private void CheckForNewHighScore()
    {
        if (highScoreGroup.gameObject.activeSelf && CurrentScore > currentHighScore)
        {
            //todo: play new high score animation
            currentPointsGroup.ScoreIcon.SetActive(true);
            highScoreGroup.gameObject.SetActive(false);
        }
    }

    public bool TrySaveNewHighScore()
    {
        bool newHighScore = CurrentScore > currentHighScore;
        if(newHighScore)
            PlayerPrefs.SetInt("HighScore",CurrentScore);
        return newHighScore;
    }
}
