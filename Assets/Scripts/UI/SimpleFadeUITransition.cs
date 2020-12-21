using System;
using System.Collections;
using UnityEngine;

public class SimpleFadeUITransition : UITransition
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float waitingTime;

    protected override IEnumerator OnPlayed(Action onTransitionCompleted)
    {
        canvasGroup.alpha = 0;
        
        float t = 0;
        while (t<=1)
        {
            t += Time.deltaTime / fadeInTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        
        onTransitionCompleted?.Invoke();
        yield return new WaitForSeconds(waitingTime);

        t = 0;
        while (t<=1)
        {
            t += Time.deltaTime / fadeOutTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }
    }
}
