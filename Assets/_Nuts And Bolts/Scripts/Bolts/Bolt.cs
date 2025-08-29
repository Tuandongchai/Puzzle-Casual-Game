using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum eBoltType
{
    BOLT_THREE,
    BOLT_FOUR,
    BOLT_FIVE,
    BOLT_SIX,
    BOLT_SEVEN,
};
public enum eBoltState { 
    NONE,
    READY,
    DIFFERENT_FULL,
    SIMILAR_FULL
};
public class Bolt
{
    private eBoltType b_type;

    private eBoltState boltState;

    private Stack<Nut> nutsStack=new Stack<Nut>();

    public List<Vector3> availablePos = new List<Vector3>() ;

    private GameSettings settings;

    private Transform root;

    private Vector3 thisPosition;

    private int currentNutsHas;

    private int index;

    public Bolt(Transform transform ,int index, GameSettings settings)
    {
        this.root = transform;

        this.index = index;

        thisPosition = settings.listBolt[index].pos ;

        this.settings = settings;

        b_type = settings.listBolt[index].type;

        currentNutsHas = settings.listBolt[index].nuts.Count();

        if(currentNutsHas ==0 )
            boltState = eBoltState.NONE;
        else if (currentNutsHas < settings.listBolt[index].size)
            boltState = eBoltState.READY;
        else
            boltState = eBoltState.DIFFERENT_FULL;
        CreateBolt(b_type, settings.listBolt[index].nuts);
    }
    private void CreateBolt(eBoltType b_type, NutData[] nutsType)
    {
        string path = Constant.GetBoltPrefabPath(b_type);
        var handle = Addressables.LoadAssetAsync<GameObject>(path);

        handle.Completed += async (AsyncOperationHandle<GameObject> task) =>
        {
            GameObject go = GameObject.Instantiate(task.Result, thisPosition, Quaternion.identity, root);
            go.GetComponent<BoltHodler>().boltData = this;

            await BoltSpaw(go, 0.2f);
            foreach (Transform child in go.transform)
            {
                if(child?.GetComponent<AvailablePos>() != null)
                {
                    //availablePos[0] this is the position where nut was spawned
                    //availablePos[1->length] they are available position
                    availablePos.Add(child.GetComponent<AvailablePos>().pos);
                }
            }
            for (int i = 0; i < nutsType.Length; i++)
            {
                Nut nut = await Nut.Create(root, availablePos[i + 1], this, nutsType[i].Color, availablePos[0], nutsType[i].Type);
                nutsStack.Push(nut);
            }

        };
    }
    
    public Nut GetTopNut()
    {
        if (boltState == eBoltState.NONE) return null;
        if(boltState==eBoltState.SIMILAR_FULL) return null;
        return nutsStack.Pop();
    }
    public Nut RemoveTopNut()
    {
        return nutsStack.Pop();
    }
    public Nut CheckTopNut()
    {
        if (boltState == eBoltState.NONE) return null;
        if (boltState == eBoltState.SIMILAR_FULL) return null;
        return nutsStack.Peek();
    }
    public async Task<bool> AddTopNut(Nut nut)
    {
        if (nut.boltParent == this)
        {
            nutsStack.Push(nut);
            await BoltController.instance.NutComeBack(nut, availablePos[nutsStack.Count]);
            return true;
        }
        if (boltState == eBoltState.SIMILAR_FULL) return false;
        else if (boltState == eBoltState.DIFFERENT_FULL) return false;
        else if (boltState == eBoltState.NONE)
        {
            nutsStack.Push(nut);
            nut.SetBoltParent(this);
            await BoltController.instance.NutMoveToNewBolt(nut, this,availablePos[nutsStack.Count]);

            return true;
        }
        else if(boltState == eBoltState.READY)
        {
            if (nut.nutColor == nutsStack.Peek().nutColor)
            {
                nutsStack.Push(nut);
                nut.SetBoltParent(this);

                await BoltController.instance.NutMoveToNewBolt(nut, this,availablePos[nutsStack.Count]);

                return true;
            }
        }
        return false;
    }

    public eBoltState GetBoltState()
    {
        boltState = SetBoltState();
        if (boltState == eBoltState.SIMILAR_FULL)
            CompleteEffect();
        else
            UndoCompleteEffect();
        return boltState;
    }

    private eBoltState SetBoltState()
    {
        if (nutsStack.Count == 0) return eBoltState.NONE;
        else if (nutsStack.Count < settings.listBolt[index].size) return eBoltState.READY;
        else if (nutsStack.Count == settings.listBolt[index].size)
        {
            Nut top = nutsStack.Pop();
            Nut second = nutsStack.Peek();
            eNutColor nType = second.nutColor;
            nutsStack.Push(top);
            foreach (Nut nut in nutsStack)
            {
                if (nut.nutColor != nType)
                    return eBoltState.DIFFERENT_FULL;
            }
        }
        return eBoltState.SIMILAR_FULL;
    }

    //undo
    public async Task NutComeBack(Nut nut)
    {
        nutsStack.Push(nut);
        nut.SetBoltParent(this);

        await BoltController.instance.NutMoveToNewBolt(nut, this, availablePos[nutsStack.Count]);
    }
    public async void CompleteEffect()
    {
        await BoltCompleteEffect();
    }
    public async Task BoltCompleteEffect()
    {
        foreach (Nut nut in nutsStack)
        {
            nut.MatchParticle();
            await Task.Delay(100);
        }
        
    }
    public void UndoCompleteEffect()
    {
        foreach (Nut nut in nutsStack)
        {
            nut.UndoMatchParticle();
        }
    }

    public async Task BoltSpaw(GameObject go,float duration)
    {
        Vector3 current = go.transform.localScale;
        go.transform.localScale = Vector3.zero;
        go.transform.DOScale(current, duration).SetEase(Ease.Linear);
        await Task.Delay((int)(duration*1000));
    }
}
