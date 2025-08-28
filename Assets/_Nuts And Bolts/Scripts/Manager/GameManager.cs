using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<eStateGame> StateChangedAction = delegate { };
    public event Action<eTypeGame> TypeChangedAction = delegate { };

    private UIMainManager m_uiMenu;
    public enum eLevelMode
    {
        EASY,
        MEDIUM,
        HARD
    }
    public enum eTypeGame
    {
        MENU,
        NUTS_AND_BOLTS,
    }
    public enum eStateGame
    {
        MAIN_MENU,
        SETUP,
        GAME_STARTED,
        PAUSE,
        GAME_OVER,
    }
    private eTypeGame m_type;
    private eStateGame m_state;
    public eTypeGame Type
    {
        get { return m_type; }
        private set
        {
            m_type = value;
            TypeChangedAction(m_type);
        }
    }
    public eStateGame State
    {
        get { return m_state; }
        private set
        {
            m_state = value;
            StateChangedAction(m_state);
        }
    }
    void Start()
    {
        m_uiMenu = FindObjectOfType<UIMainManager>();
        m_uiMenu.Setup(this);
        State = eStateGame.MAIN_MENU;
    }
    internal void SetType(eTypeGame type)
    {
        Type = type;
    }
    internal void SetState(eStateGame state)
    {
        State = state;

        if (State == eStateGame.PAUSE)
        {
            DOTween.PauseAll();
        }
        else
        {
            DOTween.PlayAll();
        }
    }
    public void LoadLevel(eLevelMode mode)
    {

    }

}
