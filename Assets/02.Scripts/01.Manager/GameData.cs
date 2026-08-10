using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameDataBase
{
    public string Id;
}
[System.Serializable]
public class BuildingData : GameDataBase
{
    public string Name;
    public int Cost;
   
    public int ScaleX;
    public int ScaleY;

    public float Width;
    public int CatCapacity;
}