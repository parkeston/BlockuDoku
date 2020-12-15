using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimatedPopup : MonoBehaviour
{
    [SerializeField] private Animation animation;
    [SerializeField] private AnimationClip usualPopup;
    [SerializeField] private AnimationClip comboPopup;

    [SerializeField] private TMP_Text scorePoints;
    
    public void PlayPopup(string points)
    {
        scorePoints.text = $"+{points}";
        animation.clip = usualPopup;
        animation.Play();
    }

    public void PlayComboPopup(string points)
    {
        scorePoints.text = $"Combo!\n+{points}";
        animation.clip = comboPopup;
        animation.Play();
    }
}
