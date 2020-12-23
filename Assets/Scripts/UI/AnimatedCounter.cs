using System.Collections;
using TMPro;
using UnityEngine;

public class AnimatedCounter : MonoBehaviour
{
    [SerializeField] private float timeToUpdate;
    [SerializeField] private TMP_Text valueText;

    private float value;
    private Coroutine valueUpdatingRoutine;

    public void ResetCounterTo(float value)
    {
        this.value = value;
        valueText.text = ((int)value).ToString();
    }

    public void SetValue(int targetValue)
    {
        if(valueUpdatingRoutine !=null)
            StopCoroutine(valueUpdatingRoutine);
        valueUpdatingRoutine = StartCoroutine(UpdateValue(targetValue));
    }

    private IEnumerator UpdateValue(int targetValue)
    {
        float t = 0;
        float startingValue = value;
        while (t<=1)
        {
            t += Time.deltaTime * 1/timeToUpdate;
            value = Mathf.Lerp(startingValue, targetValue, Mathf.SmoothStep(0, 1, t));
            valueText.text = ((int)value).ToString();
            yield return null;
        }
    }
}
