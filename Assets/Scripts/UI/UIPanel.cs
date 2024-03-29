﻿using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    //todo:prefer switching by panel class type, not enum, but has unified tab system
    public enum Type
    {
        Undefined,
        MatchResultScreen,
        MainMenu,
        GameScore,
        ChallengeResultScreen,
        GameScoreChallenge
    }

    [SerializeField] private Type type;
    public Type PanelType => type;

    public void Show()
    {
        gameObject.SetActive(true);
        OnShown();
    }
    
    public void Hide() => OnHide();
    protected void Close() => gameObject.SetActive(false);
    public virtual void Init(){}
    protected virtual void OnShown(){}
    protected virtual void OnHide() => Close();
}
