using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeAwardCalendarPreview : MonoBehaviour
{
    [SerializeField] private Image cupImage;
    
    public void SetImage(Sprite cupSprite) => cupImage.sprite = cupSprite;
}
