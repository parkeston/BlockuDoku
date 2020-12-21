using System;
using System.Collections;
using UnityEngine;

public class CoroutineSheduler : Singleton<CoroutineSheduler>
{
    public void InvokeOnEndOfFrame(Action action) => StartCoroutine(EndOfFrameRoutine(action));
    private IEnumerator EndOfFrameRoutine(Action action)
    {
        yield return new WaitForEndOfFrame();
        action?.Invoke();
    }

    public void InvokeWithDelay(Action action, float delay) => StartCoroutine(DelayRoutine(action, delay));
    private IEnumerator DelayRoutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
