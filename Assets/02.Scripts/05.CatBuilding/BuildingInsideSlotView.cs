using UnityEngine;

public class BuildingInsideSlotView : MonoBehaviour
{
    private BuildingInsideSlotViewModel _slotViewModel;

    public BuildingInsideSlotViewModel SlotViewModel{ get => _slotViewModel; }

    public void SetSlot(bool isFilled)
    {
        _slotViewModel.IsSlotFilled = isFilled;
    }
}
