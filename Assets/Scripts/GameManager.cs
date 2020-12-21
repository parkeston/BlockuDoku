using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
   [SerializeField] private UIManager UIManager;
   
   private GameMode gameMode;
   public GameScore GameScore { get; private set; }
   public event Action<GameMode> OnGameStarted;

   private void Awake()
   {
      gameMode = new GameMode();
      GameScore = new GameScore();
   }

   private void Start()
   {
      UIManager.Show(UIPanel.Type.MainMenu);
   }

   public void Play(bool challengeMode)
   {
      if (challengeMode)
         gameMode.SetChallengeMode();
      else
         gameMode.SetDefaultMode();
      
      GameScore.ResetScore();

      UIManager.Show(UIPanel.Type.GameScore,true);
      OnGameStarted?.Invoke(gameMode);
   }

   public void Retry() => Play(gameMode.IsChallengeMode);

   public void Win()
   {
      GameScore.TrySaveNewHighScore();
   }

   public void Lose(Rect gridRect)
   {
      if (!gameMode.IsChallengeMode)
      {
         GameScore.TrySaveNewHighScore();
         UIManager.Show(UIPanel.Type.LoseScreen, out UIPanel losePanel);
         (losePanel as LoseScreen)?.TakeScreenshot(gridRect);
      }
      else
         UIManager.Show(UIPanel.Type.LoseScreenChallenge);
   }

   public void ToMainMenu()
   {
      UIManager.Show(UIPanel.Type.MainMenu,out UIPanel mainMenu,true);
      (mainMenu as TabSystem)?.SelectTab(gameMode.IsChallengeMode?1:0);
   }
}
