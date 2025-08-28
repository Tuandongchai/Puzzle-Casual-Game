using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelNutsAndBoltsManager : MonoBehaviour,IMenu
{
    private UIMainManager m_mngr;
    private GameManager m_gameManager;
    private List<IMenu> m_menus = new List<IMenu>();

    [SerializeField] private Button btnNutsAndBolts;
    private void Awake()
    {
        btnNutsAndBolts.onClick.AddListener(OnClickPlay);
        foreach (Transform child in transform)
        {
            IMenuNAB menu = child.GetComponent< IMenuNAB>();
            if (menu != null)
                m_menus.Add(menu);
        }
    }

    private void OnDestroy()
    {
        if (btnNutsAndBolts) btnNutsAndBolts.onClick.RemoveAllListeners();
    }
    void Start()
    {
        foreach (IMenuNAB menu in m_menus)
        {
            menu.SetupNAB(this);
        }
    }
    private void OnClickPlay()
    {
        
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Setup(UIMainManager mngr)
    {
        m_mngr = mngr;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    internal void Setup(GameManager gameManager)
    {
        m_gameManager = gameManager;
        m_gameManager.StateChangedAction += OnGameStateChange;
    }
    private void OnGameStateChange(GameManager.eStateGame state)
    {
        switch (state)
        {
            case GameManager.eStateGame.MAIN_MENU:
                ShowMenu<UIPanelMenuNAB>();
                break;
            case GameManager.eStateGame.GAME_STARTED:
                ShowMenu<UIPanelGameNAB>();
                break;
            case GameManager.eStateGame.GAME_OVER:
                ShowMenu<UIPanelOverNAB>();
                break;
            
        }
    }
    private void ShowMenu<T>() where T : IMenu
    {
        foreach (IMenu menu in m_menus)
        {
            if (menu is T)
                menu.Show();
            else
                menu.Hide();
        }
    }
}

