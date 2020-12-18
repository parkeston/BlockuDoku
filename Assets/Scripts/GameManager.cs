
using System;

public class GameManager : Singleton<GameManager>
{
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
      UIManager.Instance.Show(UIPanel.Type.MainMenu);
   }

   public void Play(bool challengeMode)
   {
      if (challengeMode)
         gameMode.SetChallengeMode();
      else
         gameMode.SetDefaultMode();
      
      GameScore.ResetScore();

      UIManager.Instance.HideAll();
      UIManager.Instance.Show(UIPanel.Type.GameScore);
      OnGameStarted?.Invoke(gameMode);
   }

   public void Retry()
   {
      GameScore.ResetScore();
      
      //do not change game mode, start the same as last
      UIManager.Instance.HideAll();
      UIManager.Instance.Show(UIPanel.Type.GameScore);
      OnGameStarted?.Invoke(gameMode);
   }

   public void Win()
   {
      GameScore.TrySaveNewHighScore();
   }

   public void Lose()
   {
      GameScore.TrySaveNewHighScore();
      
      UIManager.Instance.HideAll();
      UIManager.Instance.Show(UIPanel.Type.LoseScreen);
   }

   public void ToMainMenu()
   {
      UIManager.Instance.HideAll();
      UIManager.Instance.Show(UIPanel.Type.MainMenu);
   }
}
