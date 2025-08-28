using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelMenuNAB : MonoBehaviour, IMenuNAB
{
    private UIPanelNutsAndBoltsManager m_mngr;

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Setup(UIMainManager mngr)
    {
        
    }

    public void SetupNAB(UIPanelNutsAndBoltsManager nab)
    {
        m_mngr = nab;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
}
