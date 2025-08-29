using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NutData
{
    public eNutColor Color;
    public eNutType Type;

    public NutData(eNutColor color, eNutType type=eNutType.NORMAL)
    {
        Color = color;
        Type = type;
    }
}
[System.Serializable]
public class InfoBolt
{
    public eBoltType type;
    public int size;
    public Vector3 pos;
    public NutData[] nuts;
}
[CreateAssetMenu(menuName ="GameSettings")]
public class GameSettings : ScriptableObject
{
    public List<InfoBolt> listBolt= new List<InfoBolt> ();
}
