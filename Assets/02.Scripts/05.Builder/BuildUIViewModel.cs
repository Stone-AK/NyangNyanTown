using System.Collections.Generic;


public class BuildUIViewModel : DNViewModelBase
{
    public List<BuildingSlotItemViewModel> _itemSlots { get; } = new List<BuildingSlotItemViewModel>();
    public BuildUIViewModel()
    {
        foreach (var data in TestBuildingDatabase.Instance.BuildingDatas) 
        {
            var newSlot = new BuildingSlotItemViewModel();
            newSlot.Initialize(data,BuildManager.Instance.TotalGold);
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