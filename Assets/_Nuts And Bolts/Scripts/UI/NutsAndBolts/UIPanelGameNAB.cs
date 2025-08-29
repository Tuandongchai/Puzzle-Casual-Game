using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelGameNAB : MonoBehaviour,IMenuNAB
{
    private UIPanelNutsAndBoltsManager m_mngr;

    [SerializeField] private Button btnUndo;

    private bool canUndo = true;

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Setup(UIMainManager mngr, GameManager gmn)
    {

    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void OnDestroy()
    {
        btnUndo.onClick.RemoveListener(Undo);
    }
    public void SetupNAB(UIPanelNutsAndBoltsManager nab)
    {
        m_mngr = nab;

        btnUndo.onClick.AddListener(()=> {
            if (!canUndo) return;
            Undo();
        });
    }

    private async void Undo()
    {
        canUndo = false;
        await BoltController.instance.Undo();
        canUndo = true;
    }
}