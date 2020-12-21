using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : UIPanel
{
    [SerializeField] private TMP_Text scoreTitle;
    [SerializeField] private ScoreGroup scorePoints;
    
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button homeButton;

    [SerializeField] private Image gridPreview;
    
    public override void Init()
    {
        newGameButton.onClick.AddListener( GameManager.Instance.Retry);
        homeButton.onClick.AddListener(GameManager.Instance.ToMainMenu); //todo: return to specific tab of main menu!
    }

    protected override void OnShown()
    {
        scoreTitle.text = GameManager.Instance.GameScore.IsNewHighScore ? "New record" : "Score";
        scorePoints.ScoreIcon.SetActive(GameManager.Instance.GameScore.IsNewHighScore);
        scorePoints.ScoreText.text = GameManager.Instance.GameScore.CurrentScore.ToString();
    }
    
    public void TakeScreenshot(Rect rect)
    { 
        CoroutineSheduler.Instance.InvokeOnEndOfFrame(() =>
        {
            var sourceTexture = ScreenCapture.CaptureScreenshotAsTexture();
            gridPreview.sprite = Sprite.Create(sourceTexture,rect,Vector2.one*0.5f,100);
        });
    }
}
