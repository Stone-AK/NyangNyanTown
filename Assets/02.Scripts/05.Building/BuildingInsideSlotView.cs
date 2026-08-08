using UnityEngine;

public class BuildingInsideSlotView : MonoBehaviour
{
    private bool _isSlotFilled = false;

    public void SettingSlot(bool isFilled)
    {
        _isSlotFilled = isFilled;
    }

    public bool CheckSlotFilled()
    {
        return _isSlotFilled;
    }
}
