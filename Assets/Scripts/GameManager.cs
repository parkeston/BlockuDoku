using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
   [SerializeField] private UIManager UIManager;
   
   public GameMode GameMode { get; private set; }
   public GameScore GameScore { get; private set; }
   public event Action<GameMode> OnGameStarted;

   public float ChallengeProgress =>
      GameMode.IsChallengeMode ? Mathf.Clamp01((float)GameScore.CurrentScore / GameMode.CurrentChallenge.PointsToPass) : 0;

   private void Awake()
   {
      GameMode = new GameMode();
      GameScore = new GameScore();
   }

   private void Start()
   {
      UIManager.Show(UIPanel.Type.MainMenu);
   }

   public void Play()
   {
      GameMode.SetDefaultMode();
      Retry();
   }

   public void Play(DateTime dateTime)
   {
      GameMode.SetChallengeMode(dateTime);
      Retry();
   }

   public void Retry()
   {
      GameScore.ResetScore();
      UIManager.Show(GameMode.IsChallengeMode?UIPanel.Type.GameScoreChallenge:UIPanel.Type.GameScore,true,
         panel => OnGameStarted?.Invoke(GameMode));
   }

   public void Win()
   {
      GameMode.SetChallengeCompleted();
      UIManager.Show(UIPanel.Type.ChallengeResultScreen);
   }

   public void Lose(Rect gridRect)
   {
      if (!GameMode.IsChallengeMode)
      {
         GameScore.TrySaveNewHighScore();
         UIManager.InvokePanelAction(UIPanel.Type.MatchResultScreen,
            losePanel =>  (losePanel as MatchResultScreen)?.TakeScreenshot(gridRect));
         UIManager.Show(UIPanel.Type.MatchResultScreen);
      }
      else
         UIManager.Show(UIPanel.Type.ChallengeResultScreen);
   }

   public void ToMainMenu()
   {
      UIManager.Show(UIPanel.Type.MainMenu,true,
         mainMenu=>  (mainMenu as TabSystem)?.SelectTab(GameMode.IsChallengeMode?1:0));
   }
}
