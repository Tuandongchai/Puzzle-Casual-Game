using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMenuNAB : MonoBehaviour, IMenuNAB
{
    private UIPanelNutsAndBoltsManager m_mngr;
    [SerializeField] Button btnPlay;

    void Awake()
    {
        btnPlay.onClick.AddListener(OnClickPlay);
    }
    private void OnDestroy()
    {
        if (btnPlay) btnPlay.onClick.RemoveAllListeners();
    }
    private void OnClickPlay()
    {
        GameManager.instance.LoadGame(GameManager.eLevelMode.EASY, GameManager.eTypeGame.NUTS_AND_BOLTS);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Setup(UIMainManager mngr, GameManager gmn)
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
