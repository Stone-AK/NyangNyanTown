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
[System.Serializable]
public class CatInfoData : BaseData
{
    public string Name;
    public string Description;
    //public string CatEffect;
    //public float EffectValue;
    public string CatIconImgPath;
}
[System.Serializable]
public class LandUpGradeData : BaseData
{
    public int NeedGold;
    public int NeedCat;
    public string NeedBuildingId; 
    public string NeedSpecialCatId;
}