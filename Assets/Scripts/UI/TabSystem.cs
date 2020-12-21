using System;
using UnityEngine;
using UnityEngine.UI;

public class TabSystem : UIPanel
{
    [Serializable]
    private struct TabScreenPair
    {
        [SerializeField] private TabToggle tab;
        [SerializeField] private UIPanel uiPanel;

        public void Init(Action<Toggle,bool> tabClicked)
        {
            tab.OnToggleClicked += tabClicked;
            tab.onValueChanged.AddListener(TabScreenResponse);
            uiPanel.Init();
        }

        public void SetToggleGroup(ToggleGroup toggleGroup) => tab.group = toggleGroup;
        
        public bool IsOn
        {
            get => tab.isOn;
            set
            {
                //setting toggle value manually
                tab.SetIsOnWithoutNotify(value);
                tab.onValueChanged.Invoke(value);
            }
        }

        private void TabScreenResponse(bool isOn)
        {
            if(isOn)
                uiPanel.Show();
            else
                uiPanel.Hide();
        }
    }

    [SerializeField] private bool useToggleGroup;
    [SerializeField] private bool allowSwitchOff;
    [SerializeField] private int defaultPair;
    
    [Space]
    [SerializeField] private TabScreenPair[] tabScreenPairs;
    [SerializeField] private UITransition tabTransition;
    
    [HideInInspector][SerializeField] private ToggleGroup toggleGroup;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (useToggleGroup)
        {
            if(toggleGroup==null)
                toggleGroup = gameObject.AddComponent<ToggleGroup>();
            toggleGroup.allowSwitchOff = allowSwitchOff;
        }
        else if (!useToggleGroup && toggleGroup!=null)
            UnityEditor.EditorApplication.delayCall+=()=>DestroyImmediate(toggleGroup);
    }
    #endif
    
    public override void Init()
    {
        foreach (var tabScreenPair in tabScreenPairs)
        {
            tabScreenPair.Init(TryInvokeTabTransition);
            if(useToggleGroup)
                tabScreenPair.SetToggleGroup(toggleGroup);
        }
    }
    
    protected override void OnShown()
    {
        DisableAllTabs();
        tabScreenPairs[defaultPair].IsOn = true;
    }
    
    private void TryInvokeTabTransition(Toggle toggle, bool value)
    {
        if(!value && (useToggleGroup && !allowSwitchOff))
            return;
        if(tabTransition!=null)
            tabTransition.Play(()=>toggle.isOn=value);
        else
            toggle.isOn = value;
    }

    public void SelectTab(int index) => tabScreenPairs[index].IsOn = true;

    private void DisableAllTabs()
    {
        bool oldAllowSwitchOff=false;
        if (useToggleGroup)
        {
            oldAllowSwitchOff = toggleGroup.allowSwitchOff;
            toggleGroup.allowSwitchOff = false;
        }
        
        for (int i = 0; i < tabScreenPairs.Length; i++)
            tabScreenPairs[i].IsOn = false;

        if (useToggleGroup)
            toggleGroup.allowSwitchOff = oldAllowSwitchOff;
    }
}
