using System;
using UnityEngine;

public class LandViewModel
{
    
    public int LandLevel { get; private set; } = 0;
    public event Action<int> OnLandLevelUp;

    public void LandLevelUp()
    {
        LandLevel++;
        Debug.Log($"현재 레벨{LandLevel}");

        OnLandLevelUp?.Invoke(LandLevel);
    }

}
