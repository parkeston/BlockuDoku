using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SnapScrollRect : ScrollRect
{
    public event Action<int> OnSnapping;
    
    private float startPosition;
    private float targetPosition;
    private Coroutine snappingRoutine;
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if(snappingRoutine!=null)
            return;
        base.OnBeginDrag(eventData);
        startPosition = horizontalNormalizedPosition;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if(snappingRoutine!=null)
            return;
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if(snappingRoutine!=null)
            return;
        base.OnEndDrag(eventData);

        var scrollElements = content.childCount;
        var endPosition = horizontalNormalizedPosition;

        var direction = (int)Mathf.Sign(endPosition - startPosition);
        var offset = direction * 1f / (scrollElements-1f);
        targetPosition = Mathf.Clamp01(startPosition + offset);
        
        snappingRoutine = StartCoroutine(SnapToScrollPosition(endPosition, targetPosition));
        OnSnapping?.Invoke(direction);
    }

    public void InvokeSnap(int direction)
    {
        if(snappingRoutine!=null)
            return;
        
        var scrollElements = content.childCount;
        targetPosition = horizontalNormalizedPosition + direction * 1f / (scrollElements - 1f);
        targetPosition = Mathf.Clamp01(targetPosition);
        
        snappingRoutine = StartCoroutine(SnapToScrollPosition(horizontalNormalizedPosition, targetPosition));
        OnSnapping?.Invoke(direction);
    }

    public bool CanSnap(int direction)
    {
        var scrollElements = content.childCount;
        float futurePosition = (snappingRoutine!=null?targetPosition:horizontalNormalizedPosition)+ direction * 1f / (scrollElements - 1f);
        return futurePosition >= 0 && futurePosition <= 1;
    }

    private IEnumerator SnapToScrollPosition(float from, float to)
    {
        float t = 0;
        while (t<=1)
        {
            t += Time.deltaTime * 1 / 0.3f;
            horizontalNormalizedPosition = Mathf.Lerp(from, to, Mathf.SmoothStep(0f,1f,t));
            yield return null;
        }

        snappingRoutine = null;
    }
}
