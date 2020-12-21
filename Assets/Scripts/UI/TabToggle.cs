using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabToggle : Toggle
{
    public event Action<Toggle,bool> OnToggleClicked;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!IsActive() || !IsInteractable())
            return;
        
        //do not set isOn value immediately on click, but invokes event (isOn can be set manually)
        //useful for external conditions check, like delay on tuning on/off due to tab system transition fx
        OnToggleClicked?.Invoke(this,!isOn); 
    }
}
