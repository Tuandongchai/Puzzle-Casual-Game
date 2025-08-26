using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public List<Vector3> availablePos=new List<Vector3>();

    private GameSettings settings;

    private Transform root;

    private Vector3 thisPosition;

    private int currentNutsHas;

    public Bolt(Transform transform ,Vector3 position, GameSettings settings, List<NutData> nutsType = null)
    {
        this.root = transform;

        thisPosition = position;

        this.settings = settings;

        b_type = settings.BoltType;

        currentNutsHas = nutsType.Count;

        if(currentNutsHas ==0 )
            boltState = eBoltState.NONE;
        else if (currentNutsHas < settings.boltSize)
            boltState = eBoltState.READY;
        else
            boltState = eBoltState.DIFFERENT_FULL;
        CreateBolt(b_type, nutsType);
    }
    private void CreateBolt(eBoltType b_type, List<NutData> nutsType)
    {
        string path = Constant.GetBoltPrefabPath(b_type);
        var handle = Addressables.LoadAssetAsync<GameObject>(path);

        handle.Completed += (AsyncOperationHandle<GameObject> task) =>
        {
            GameObject go = GameObject.Instantiate(task.Result, thisPosition, Quaternion.identity, root);
            go.GetComponent<BoltHodler>().boltData = this;
            foreach (Transform child in go.transform)
            {
                if(child?.GetComponent<AvailablePos>() != null)
                {
                    //availablePos[0] this is the position where nut was spawned
                    //availablePos[1->length] they are available position
                    availablePos.Add(child.GetComponent<AvailablePos>().pos);
                }
            }
            for (int i = 0; i < nutsType.Count; i++)
            {
                Nut nut = new Nut(root, availablePos[i + 1], this, nutsType[i].Color, nutsType[i].Type);
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
            BoltController.instance.NutComeBack(nut, availablePos[nutsStack.Count]);
            BoltController.instance.isBusy = true;
            await Task.Delay(500);
            BoltController.instance.isBusy = false;
            return true;
        }
        if (boltState == eBoltState.SIMILAR_FULL) return false;
        else if (boltState == eBoltState.DIFFERENT_FULL) return false;
        else if (boltState == eBoltState.NONE)
        {
            nutsStack.Push(nut);
            nut.SetBoltParent(this);
            BoltController.instance.NutMoveToNewBolt(nut, this,availablePos[nutsStack.Count]);
            BoltController.instance.isBusy = true;
            await Task.Delay(1000);
            BoltController.instance.isBusy = false;
            return true;
        }
        else if(boltState == eBoltState.READY)
        {
            if (nut.nutColor == nutsStack.Peek().nutColor)
            {
                nutsStack.Push(nut);
                nut.SetBoltParent(this);
                BoltController.instance.NutMoveToNewBolt(nut, this,availablePos[nutsStack.Count]);
                BoltController.instance.isBusy = true;
                await Task.Delay(1000);
                BoltController.instance.isBusy = false;
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
        else if (nutsStack.Count < settings.boltSize) return eBoltState.READY;
        else if (nutsStack.Count == settings.boltSize)
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

        BoltController.instance.NutMoveToNewBolt(nut, this, availablePos[nutsStack.Count]);
        //nut.AnimateMove(availablePos[nutsStack.Count],1f);
        BoltController.instance.isBusy = true;
        await Task.Delay(1000);
        BoltController.instance.isBusy = false;
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
}
