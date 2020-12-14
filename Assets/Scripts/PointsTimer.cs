﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsTimer : MonoBehaviour
{
    [SerializeField] private float timeToUpdate;
    [SerializeField] private TMP_Text valueText;

    private float value;
    private Coroutine valueUpdatingRoutine;
    
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
