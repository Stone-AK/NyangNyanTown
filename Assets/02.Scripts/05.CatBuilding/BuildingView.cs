using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BuildingView : MonoBehaviour
{
    [SerializeField] private GameObject ParentBuildingInsideSlot;

    private List<BuildingInsideSlotView> _buildingInsideSlotList = new();
    private float _filledSlotRate = 0f;

    private void InitSlotList()
    {
        if(ParentBuildingInsideSlot == null)
            return;

        if(_buildingInsideSlotList.Count != 0)
        {
             _buildingInsideSlotList.Clear();
        }

        for (int i = 0; i < ParentBuildingInsideSlot.transform.childCount; i++)
        {
            Transform child = ParentBuildingInsideSlot.transform.GetChild(i);
            BuildingInsideSlotView slot = child.GetComponent<BuildingInsideSlotView>();

            if (slot != null)
            {
                _buildingInsideSlotList.Add(slot);

                BindSlotViewMdoel(slot.SlotViewModel);
            }
        }
    }

    public List<BuildingInsideSlotView> GetBuildingInsideSlotList()
    {
        return _buildingInsideSlotList;
    }

    private void BindSlotViewMdoel(BuildingInsideSlotViewModel slotViewModel)
    {
        if(slotViewModel == null)
            return;
        slotViewModel.PropertyChanged += OnPropChagned_View;
    }

    private void OnPropChagned_View(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(BuildingInsideSlotViewModel.IsSlotFilled):
                {
                    int filledSlotCount = 0;
                    foreach(var slot in _buildingInsideSlotList)
                    {
                        if (slot.SlotViewModel.IsSlotFilled)
                        {
                            filledSlotCount += 1;
                        }
                    }
                    _filledSlotRate = (float)filledSlotCount / _buildingInsideSlotList.Count;
                }
            break;

        }
    }
}
