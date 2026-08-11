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

                newSlot.Initialize(data.Value, GameManager.Instance.BuildManager.TotalGold);
                _itemSlots.Add(newSlot);
                newSlot.OnBuildingSlotButtonClicked += StartBuild;
            }
            GameManager.Instance.BuildManager.OnTotalGoldChanged += OnTotalGoldChanged;
        }
    }
    public void StartBuild(BuildingData data) 
    {
        GameManager.Instance.BuildManager.StartBuild(data);
    }
    private void OnTotalGoldChanged(int currentGold) 
    {
        foreach (var itemSlot in _itemSlots) 
        {
            itemSlot.RefreshCanBuild(currentGold);
        }
    }
}