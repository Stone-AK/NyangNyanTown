using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlacedBuildingSaveData
{
    public string BuildingId;
    public string ModelAddress;
    public float RootX;
}

public class SaveDataModel
{
    public int Gold;
    public int Fish;
    public List<string> CollectedCatIdList = new();
    public int LandLevel;
    public List<PlacedBuildingSaveData> Buildings = new();
}
