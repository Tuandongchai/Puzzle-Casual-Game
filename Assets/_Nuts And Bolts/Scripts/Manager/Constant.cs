using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant
{
    public const string BOLT_TYPE_THREE = "Assets/Resources_moved/Prefabs/Bolts3.prefab";
    public const string BOLT_TYPE_FOUR = "Prefabs/Bolts4";
    public const string BOLT_TYPE_FIRE = "Assets/Resources_moved/Prefabs/Bolts5.prefab";
    public const string BOLT_TYPE_SIX = "Assets/Resources_moved/Prefabs/Bolts6.prefab";
    public const string BOLT_TYPE_SEVEN = "Assets/Resources_moved/Prefabs/Bolts7.prefab";

    public const string NUT = "Prefabs/Nut";
    public const string NUT_TYPE_BULE = "Assets/_Nuts And Bolts/Prefabs/Blue_Nut .prefab";
    public const string NUT_TYPE_BROWN = "Assets/_Nuts And Bolts/Prefabs/Brown_Nut.prefab";
    public const string NUT_TYPE_GREEN = "Assets/_Nuts And Bolts/Prefabs/Green_Nut.prefab";
    public const string NUT_TYPE_ORANGE = "Assets/_Nuts And Bolts/Prefabs/Orange_Nut.prefab";
    public const string NUT_TYPE_PURPLE = "Assets/_Nuts And Bolts/Prefabs/Purple_Nut.prefab";
    public const string NUT_TYPE_RED = "Assets/_Nuts And Bolts/Prefabs/Red_Nut.prefab";
    public const string NUT_TYPE_YELLOW = "Assets/_Nuts And Bolts/Prefabs/Yellow_Nut.prefab";


    //Nuts And Bolt Level
    public const string NAB_EASY_LEVEL1 = "Assets/_Nuts And Bolts/Level/Easy_Level1.asset";

    public static string GetBoltPrefabPath(eBoltType type)
    {
        switch (type)
        {
            case eBoltType.BOLT_THREE:
                return BOLT_TYPE_THREE;
            case eBoltType.BOLT_FOUR:
                return BOLT_TYPE_FOUR;
            case eBoltType.BOLT_FIVE:
                return BOLT_TYPE_FIRE;
            case eBoltType.BOLT_SIX:
                return BOLT_TYPE_SIX;
            case eBoltType.BOLT_SEVEN:
                return BOLT_TYPE_SEVEN;
            default: 
                return "";
        }
    }
    public static string GetNutPrefabPath(eNutColor type)
    {
        switch (type)
        {
            case eNutColor.YELLOW:
                return NUT_TYPE_YELLOW;
            case eNutColor.GREEN:
                return NUT_TYPE_GREEN;
            case eNutColor.ORANGE:
                return NUT_TYPE_ORANGE;
            case eNutColor.PURPLE:
                return NUT_TYPE_PURPLE;
            case eNutColor.RED:
                return NUT_TYPE_RED;
            case eNutColor.BLUE:
                return NUT_TYPE_BULE;
            case eNutColor.BROWN: 
                return NUT_TYPE_BROWN;
            default:
                return "";
        }
    }

}
