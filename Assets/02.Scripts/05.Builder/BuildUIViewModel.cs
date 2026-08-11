using System.Collections.Generic;
using UnityEngine;


public class BuildUIViewModel : ViewModelBase
{
    public List<BuildingSlotItemViewModel> _itemSlots { get; } = new List<BuildingSlotItemViewModel>();
    public BuildUIViewModel()
    {
        Debug.Log($"GameDataManager: {GameDataManager.Instance}");
        Debug.Log($"BuildManager: {BuildManager.Instance}");

        foreach (var data in GameDataManager.Instance._buildingDataModelList) 
        {
            var newSlot = new BuildingSlotItemViewModel();
            newSlot.Initialize(data.Value,BuildManager.Instance.TotalGold);
            _itemSlots.Add(newSlot);
            newSlot.OnBuildingSlotButtonClicked += StartBuild;
        }
        BuildManager.Instance.OnTotalGoldChanged += OnTotalGoldChanged;
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