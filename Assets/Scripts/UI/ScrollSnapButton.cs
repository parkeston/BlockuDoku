using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSnapButton : MonoBehaviour
{
    [SerializeField] private int direction;
    [SerializeField] private Button button;
    [SerializeField] private SnapScrollRect snapScrollRect;

    private void Awake()
    {
        button.onClick.AddListener(Snap);
    }

    private void OnEnable()
    {
        snapScrollRect.OnSnapping += UpdateSnapButtonAvailability;
    }

    private void OnDisable()
    {
        snapScrollRect.OnSnapping -= UpdateSnapButtonAvailability;
    }

    private void UpdateSnapButtonAvailability(int direction)
    {
        button.interactable = snapScrollRect.CanSnap(this.direction);
    }

    private void Snap()
    {
        snapScrollRect.InvokeSnap(direction);
    }
}
