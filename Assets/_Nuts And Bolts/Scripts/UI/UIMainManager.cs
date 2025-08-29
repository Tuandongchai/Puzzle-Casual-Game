using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainManager : MonoBehaviour
{
    private List<IMenu> m_menuList= new List<IMenu>();
    private GameManager m_gameManager;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            IMenu menu = child.GetComponent<IMenu>();
            if (menu != null)
                m_menuList.Add(menu);
        }
    }
    void Start()
    {
        foreach (IMenu menu in m_menuList)
        {
            menu.Setup(this,GameManager.instance);
        }
    }
    internal void ShowMainMenu()
    {
        m_gameManager.SetType(GameManager.eTypeGame.MENU);
    }
    internal void ShowGameNutsAndBolts()
    {
        m_gameManager.SetType(GameManager.eTypeGame.NUTS_AND_BOLTS);

    }

    internal void Setup(GameManager gameManager)
    {
        m_gameManager = gameManager;
        m_gameManager.TypeChangedAction += OnGameTypeChange;
    }
    private void OnGameTypeChange(GameManager.eTypeGame type)
    {
        switch (type)
        {
            case GameManager.eTypeGame.MENU:
                ShowMenu<UIPanelMenu>();
                break;
            case GameManager.eTypeGame.NUTS_AND_BOLTS:
                ShowMenu<UIPanelNutsAndBoltsManager>();
                break;
        }
    }
    private void ShowMenu<T>() where T : IMenu
    {
        foreach (IMenu menu in m_menuList)
        {
            if(menu is T)
                menu.Show();
            else
                menu.Hide();    
        }
    }
}
