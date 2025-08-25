using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="GameSettings")]
public class GameSettings : ScriptableObject
{
    public eBoltType BoltType;

    public int boltCount;

    public int boltSize;
}
