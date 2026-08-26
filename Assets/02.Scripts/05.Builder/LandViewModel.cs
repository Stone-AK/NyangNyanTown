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

    public void RestoreLandLevel(int landLevel)
    {
        LandLevel = Mathf.Max(0, landLevel);
        OnLandLevelUp?.Invoke(LandLevel);
    }
}
