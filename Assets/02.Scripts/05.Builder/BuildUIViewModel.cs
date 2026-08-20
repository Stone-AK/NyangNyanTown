using System.Collections.Generic;

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

                newSlot.Initialize(data.Value);
                _itemSlots.Add(newSlot);
                newSlot.OnBuildingSlotButtonClicked += StartBuild;
            }
            GameManager.Instance.BuildManager.OnTotalGoldChanged += OnTotalGoldChanged;
        }
    }
    public void StartBuild(BuildingData data) 
    {
        GameManager.Instance.BuildManager.StartBuild(data, BuildMode.Build);
    }
    private void OnTotalGoldChanged(int currentGold) 
    {
        foreach (var itemSlot in _itemSlots) 
        {
            itemSlot.RefreshCanBuild(currentGold);
        }
    }
}