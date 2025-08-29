using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action<eStateGame> StateChangedAction = delegate { };
    public event Action<eTypeGame> TypeChangedAction = delegate { };

    private UIMainManager m_uiMenu;

    private BoltController m_boltController;
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
    public eTypeGame TypeG
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
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    void Start()
    {
        m_uiMenu = FindObjectOfType<UIMainManager>();
        m_uiMenu.Setup(this);
        TypeG =eTypeGame.MENU;
        State = eStateGame.MAIN_MENU;
    }
    internal void SetType(eTypeGame type)
    {
        TypeG = type;
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
    public void LoadGame(eLevelMode mode, eTypeGame type)
    {
        SetType(type);
        if (type == eTypeGame.NUTS_AND_BOLTS)
        {
            m_boltController = new GameObject("BoltController").AddComponent<BoltController>();
            var handle = Addressables.LoadAssetAsync<GameSettings>(Constant.NAB_EASY_LEVEL1);
            handle.Completed += (AsyncOperationHandle<GameSettings> task) =>
            {
                m_boltController.Setup(task.Result);
            };
        }
        Debug.Log("Hello yuou");
        State = eStateGame.GAME_STARTED;
    }

}
