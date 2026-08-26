using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseData
{
    public string Id { get; set; }
}
[System.Serializable]
public class BuildingData : BaseData
{
    public string Name;
    public int Cost;
   
    public float Width;
    public float Height;

    public float GroundOffset;

    public int CatCapacity;
    public int BuildingType;

    public string[] ModelAddresses;

    public string SpCatId;
    public int SpCatValue;
}
[System.Serializable]
public class CatInfoData : BaseData
{
    public string Name;
    public string Description;
    public string CatEffect;
    public float EffectValue;
    public string CatIconImgPath;
    public int CatAppearanceWeight;
    public string SpecialCatBody;
    public string SpecialCatEye;
    public string SpecialCatMouth;
}

public class CatBodySkinData : BaseData
{
    public string Name;
    public string AddressableString;
}

public class CatEyeSkinData : BaseData
{
    public string Name;
    public string AddressableString;
}

public class CatMouthSkinData : BaseData
{
    public string Name;
    public string AddressableString;
}

[System.Serializable]
public class LandUpGradeData : BaseData
{
    public int NeedGold;
    public int NeedCat;
    public string NeedBuildingId; 
    public string NeedSpecialCatId;
}