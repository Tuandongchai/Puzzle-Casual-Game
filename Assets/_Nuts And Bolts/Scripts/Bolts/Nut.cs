using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

    public Nut(Transform transform,Vector3 postion, Bolt bolt, eNutColor color, eNutType type=eNutType.NORMAL)
    {
        root = transform;

        pos = postion;

        boltParent = bolt;

        nutColor = color;

        nutType = type;

        CreateNut(color, type,pos);
    }
    private void CreateNut(eNutColor color, eNutType type,Vector3 pos)
    {
        string path = Constant.GetNutPrefabPath(color);
        var handle = Addressables.LoadAssetAsync<GameObject>(path);
        handle.Completed += (AsyncOperationHandle<GameObject> task) =>
        {
            View = GameObject.Instantiate(task.Result,pos, Quaternion.identity, root);
            if(type ==eNutType.HIDE)
                View.transform.GetChild(1).gameObject.SetActive(true);
        };
    }

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
            0.5f,                     
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
            0.5f,
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
}
