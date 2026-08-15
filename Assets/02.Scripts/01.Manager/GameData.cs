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
   
    public int ScaleX;
    public int ScaleY;

    public float Width;
    public int CatCapacity;
    public int BuildingType;
}

public class CatInfoData : BaseData
{
    public string Name;
    public string Description;
    //public string CatEffect;
    //public float EffectValue;
    public string CatIconImgPath;
    public int CatAppearanceWeight;
}