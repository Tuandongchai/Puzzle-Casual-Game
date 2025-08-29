using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMenu : MonoBehaviour, IMenu
{
    private UIMainManager m_mngr;

    [SerializeField] private Button btnNutsAndBolts;

    private void Awake()
    {
        btnNutsAndBolts.onClick.AddListener(OnClickNutsAndBolts);
    }

    private void OnDestroy()
    {
        if (btnNutsAndBolts) btnNutsAndBolts.onClick.RemoveAllListeners();
    }

    private void OnClickNutsAndBolts()
    {
        m_mngr.ShowGameNutsAndBolts();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Setup(UIMainManager mngr, GameManager gmn)
    {
        m_mngr = mngr;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    
}
