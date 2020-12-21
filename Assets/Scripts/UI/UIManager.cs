using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UITransition uiTransition;
    
    private Dictionary<UIPanel.Type,UIPanel> panels;

    private void Start()
    {
       RegisterChildPanels();
       uiTransition.Hide();
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
    
    public void Show(UIPanel.Type type, bool hidePreviousPanels = false) => Show(type, out _,hidePreviousPanels);

    public void Show(UIPanel.Type type, out UIPanel outPanel, bool hidePreviousPanels=false)
    {
        var panel = outPanel = null;
        if (panels.ContainsKey(type))
            panel = outPanel = panels[type];
        else
            return;

        if (uiTransition == null || !panels.Any(uiPanel => uiPanel.Value.isActiveAndEnabled))
            ShowPanel();
        else
            uiTransition.Play(ShowPanel);

        void ShowPanel()
        {
            if(hidePreviousPanels) HideAll();
            panel.Show();
        }
    }

    private void Hide(UIPanel.Type type)
    {
        if(panels.ContainsKey(type))
            panels[type].Hide();
    }

    private void HideAll()
    {
        foreach (var panel in panels.Values)
            panel.Hide();
    }
}
