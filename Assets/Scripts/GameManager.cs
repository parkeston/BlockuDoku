using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
   [SerializeField] private UIManager UIManager;
   
   public GameMode GameMode { get; private set; }
   public GameScore GameScore { get; private set; }
   public event Action<GameMode> OnGameStarted;

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
      UIManager.Show(UIPanel.Type.GameScore,true,panel => OnGameStarted?.Invoke(GameMode));
   }

   public void Win()
   {
      GameScore.TrySaveNewHighScore();
   }

   public void Lose(Rect gridRect)
   {
      if (!GameMode.IsChallengeMode)
      {
         GameScore.TrySaveNewHighScore();
         UIManager.InvokePanelAction(UIPanel.Type.LoseScreen,
            losePanel =>  (losePanel as LoseScreen)?.TakeScreenshot(gridRect));
         UIManager.Show(UIPanel.Type.LoseScreen);
      }
      else
         UIManager.Show(UIPanel.Type.LoseScreenChallenge);
   }

   public void ToMainMenu()
   {
      UIManager.Show(UIPanel.Type.MainMenu,true,
         mainMenu=>  (mainMenu as TabSystem)?.SelectTab(GameMode.IsChallengeMode?1:0));
   }
}
