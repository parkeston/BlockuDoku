using System;
using System.Collections;
using UnityEngine;

public abstract class UITransition : MonoBehaviour
{
    public void Play(Action onTransitionCompleted)
    {
        gameObject.SetActive(true);
        StartCoroutine(PlayRoutine((onTransitionCompleted)));
    }
    public void Hide() => gameObject.SetActive(false);

    private IEnumerator PlayRoutine(Action onTransitionCompleted)
    {        
        yield return OnPlayed(onTransitionCompleted);
        Hide();
    }

    protected virtual IEnumerator OnPlayed(Action onTransitionCompleted)
    {
        onTransitionCompleted?.Invoke();
        yield return 0;
    }
}
