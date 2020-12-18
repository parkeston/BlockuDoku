
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : UIPanel
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueChallengeButton;

    public override void Init()
    {
        newGameButton.onClick.AddListener(PlayDefaultMode);
        continueChallengeButton.onClick.AddListener(PlayChallengeMode);
    }

    private void PlayDefaultMode() => GameManager.Instance.Play(false);
    private void PlayChallengeMode() => GameManager.Instance.Play(true);
}
