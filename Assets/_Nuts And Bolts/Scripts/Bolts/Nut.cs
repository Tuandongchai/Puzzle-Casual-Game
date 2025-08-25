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
public class Nut
{
    public eNutColor nutType { get; private set; }

    public Bolt boltParent { get; private set; }

    private Transform root;

    private Vector3 pos;

    private GameObject View;

    public Nut(Transform transform,Vector3 postion, Bolt bolt, eNutColor type)
    {
        root = transform;

        pos = postion;

        boltParent = bolt;

        nutType = type;

        CreateNut(type, pos);
    }
    private void CreateNut(eNutColor type, Vector3 pos)
    {
        string path = Constant.GetNutPrefabPath(type);
        var handle = Addressables.LoadAssetAsync<GameObject>(path);
        handle.Completed += (AsyncOperationHandle<GameObject> task) =>
        {
            View = GameObject.Instantiate(task.Result);
            View.transform.parent = root;
            View.transform.position = pos;
        };
    }

    public void SetBoltParent(Bolt bolt)
    {
        this.boltParent = bolt;
    }

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
}
