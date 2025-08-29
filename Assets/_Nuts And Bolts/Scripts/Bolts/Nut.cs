using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum eNutColor
{
    BROWN,
    RED,
    ORANGE,
    YELLOW,
    BLUE,
    GREEN,
    PURPLE
};

public enum eNutType{
    NORMAL,
    HIDE
}
public class Nut
{
    public eNutColor nutColor { get; private set; }

    public eNutType nutType { get; private set; } 

    public Bolt boltParent { get; private set; }

    private Transform root;

    private Vector3 pos;

    private GameObject View;

    
    public static async Task<Nut> Create(Transform transform, Vector3 postion, Bolt bolt, eNutColor color, Vector3 posStart, eNutType type = eNutType.NORMAL)
    {
        Nut nut = new Nut(transform, postion, bolt, color, type);
        await nut.CreateNut(color, type, postion, posStart);
        return nut;
    }

    private Nut(Transform transform, Vector3 postion, Bolt bolt, eNutColor color, eNutType type = eNutType.NORMAL)
    {
        root = transform;
        pos = postion;
        boltParent = bolt;
        nutColor = color;
        nutType = type;
    }

    

    private async Task CreateNut(eNutColor color, eNutType type, Vector3 pos, Vector3 posStart)
    {
        string path = Constant.GetNutPrefabPath(color);
        var handle = Addressables.LoadAssetAsync<GameObject>(path);
        var prefab = await handle.Task;

        View = GameObject.Instantiate(prefab, posStart, Quaternion.identity, root);
        await AnimateNutSpawn(0.2f, 0.4f, pos);

        if (type == eNutType.HIDE)
            View.transform.GetChild(1).gameObject.SetActive(true);
    }
    //
    public void SetBoltParent(Bolt bolt)
    {
        this.boltParent = bolt;
    }

    //
    public void AnimateClockwise(Vector3 targetPos, float duration)
    {
        View.transform.DOMove(targetPos, duration)
                 .SetEase(Ease.Linear);

        View.transform.DORotate(
            new Vector3(0, 360, 0),
            0.4f,                     
            RotateMode.LocalAxisAdd 
        )
        .SetEase(Ease.Linear)
        .SetLoops(Mathf.CeilToInt(duration), LoopType.Incremental);
    }
    public void Animatecounterclockwise(Vector3 targetPos, float duration)
    {
        View.transform.DOMove(targetPos, duration)
                 .SetEase(Ease.Linear);

        View.transform.DORotate(
            new Vector3(0, -360, 0),
            0.4f,
            RotateMode.LocalAxisAdd
        )
        .SetEase(Ease.Linear)
        .SetLoops(Mathf.CeilToInt(duration), LoopType.Incremental);
    }
    public void AnimateMove(Vector3 targetPos, float duration)
    {
        View.transform.DOMove(targetPos, duration)
                 .SetEase(Ease.Linear);
    }

    //
    public void Hidden()
    {
        nutType = eNutType.HIDE;
        View.transform.GetChild(1).transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                View.transform.GetChild(1).transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.2f)
                    .SetEase(Ease.Linear);
            });
    }
    public void Unhidden()
    {
        nutType = eNutType.NORMAL;
        View.transform.GetChild(1).transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                View.transform.GetChild(1).transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f)
                    .SetEase(Ease.Linear);
            });
    }

    public void MatchParticle()
    {
        View.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f)
            .SetEase(Ease.Linear)
            .SetDelay(0.05f)
            .OnComplete(() =>
            {
                View.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.2f)
                    .SetEase(Ease.Linear);
            });
        View.transform.GetChild(11).gameObject.SetActive(true);
    }
    public void UndoMatchParticle()
    {
        View.transform.GetChild(11).gameObject.SetActive(false);

    }

    private async Task AnimateNutSpawn(float spawDuration, float goToPosDuration, Vector3 pos)
    {
        BoltController.instance.isBusy = true;
        Vector3 current = View.transform.localScale;
        View.transform.localScale = Vector3.zero;
        View.transform.DOScale(current, spawDuration).SetEase(Ease.Linear);
        await Task.Delay((int) (spawDuration*1000));
        Animatecounterclockwise(pos, goToPosDuration);
        await Task.Delay((int) (goToPosDuration*1000));
        BoltController.instance.isBusy = false;
        
    }
}
