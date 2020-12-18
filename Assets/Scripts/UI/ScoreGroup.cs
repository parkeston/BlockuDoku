using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGroup : MonoBehaviour
{
    [SerializeField] private GameObject scoreIcon;
    [SerializeField] private TMP_Text scoreText;

    public GameObject ScoreIcon => scoreIcon;
    public TMP_Text ScoreText => scoreText;

    private void OnEnable()
    {
        StartCoroutine(OnEnableRoutine());
    }
    private IEnumerator OnEnableRoutine()
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
    }
}
