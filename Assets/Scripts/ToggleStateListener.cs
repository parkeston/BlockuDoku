using System;
using UnityEngine;
using UnityEngine.UI;

//todo: make more reusable selectable state listeners components
public class ToggleStateListener : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private Graphic graphic;

    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    private void Reset()
    {
        toggle = GetComponent<Toggle>();
    }

    private void OnValidate()
    {
        if(graphic!=null && toggle!=null)
            graphic.color = toggle.isOn ? onColor : offColor;
    }

    private void Awake()
    {
        toggle.onValueChanged.AddListener((v)=> graphic.color = v?onColor:offColor);
    }
}
