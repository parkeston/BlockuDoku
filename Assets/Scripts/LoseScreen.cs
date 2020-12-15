﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreTitle;
    [SerializeField] private ScoreGroup scorePoints;
    [SerializeField] private Button newGameButton;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        gameObject.SetActive(false);
        newGameButton.onClick.AddListener(()=>SceneManager.LoadScene(0));
    }

    public void Show(string points, bool newHighScore, float delay = 0.5f, float duration = 2)
    {
        scoreTitle.text = newHighScore ? "New record" : "Score";
        scorePoints.ScoreIcon.SetActive(newHighScore);
        scorePoints.ScoreText.text = points;
        
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        
        StartCoroutine(ShowWithDelay(delay,duration));
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
