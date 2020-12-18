using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : UIPanel
{
    [SerializeField] private TMP_Text scoreTitle;
    [SerializeField] private ScoreGroup scorePoints;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private CanvasGroup canvasGroup;

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
        
        canvasGroup.alpha = 0;
        StartCoroutine(ShowWithDelay(1,1));
    }

    private IEnumerator ShowWithDelay(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        float t = 0;
        while (t<=1)
        {
            t += Time.deltaTime * 1 / duration;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
    }
}
