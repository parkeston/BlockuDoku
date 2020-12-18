using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<UIPanel.Type,UIPanel> panels;
    
    private void Awake()
    {
       RegisterChildPanels();
    }

    private void RegisterChildPanels()
    {
        panels = new Dictionary<UIPanel.Type, UIPanel>();
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out UIPanel uiPanel))
            {
                uiPanel.Init();
                uiPanel.Hide();
                panels.Add(uiPanel.PanelType,uiPanel);
            }
        }
    }

    public void Show(UIPanel.Type type)
    {
        if(panels.ContainsKey(type))
            panels[type].Show();
    }

    public void Hide(UIPanel.Type type)
    {
        if(panels.ContainsKey(type))
            panels[type].Hide();
    }

    public void HideAll()
    {
        foreach (var panel in panels.Values)
            panel.Hide();
    }
}
