using System.Collections.Generic;
using UnityEngine;


public class BuildUIViewModel : ViewModelBase
{
    public List<BuildingSlotItemViewModel> _itemSlots { get; } = new List<BuildingSlotItemViewModel>();
    public BuildUIViewModel()
    {
        if (GameManager.Instance.DataManager.TryGetDataTable<BuildingData>(out var dataTable))
        {
            foreach (var data in dataTable)
            {
                var newSlot = new BuildingSlotItemViewModel();

                newSlot.Initialize(data.Value, BuildManager.Instance.TotalGold);
                _itemSlots.Add(newSlot);
                newSlot.OnBuildingSlotButtonClicked += StartBuild;
            }
            BuildManager.Instance.OnTotalGoldChanged += OnTotalGoldChanged;
        }
    }
    public void StartBuild(BuildingData data) 
    {
        BuildManager.Instance.StartBuild(data);
    }
    private void OnTotalGoldChanged(int currentGold) 
    {
        foreach (var itemSlot in _itemSlots) 
        {
            itemSlot.RefreshCanBuild(currentGold);
        }
    }
}