using System;

public class BuildingSlotItemViewModel : ViewModelBase
{
    public string Name { get; private set; }
    public int Cost { get; private set; }
  
    private bool _canBuild;
    public bool CanBuild
    {
        get => _canBuild;

        private set
        {
            if (_canBuild == value)
                return;

            _canBuild = value;

            OnPropertyChanged(nameof(CanBuild));
        }
    }
    private BuildingData _buildingData; 
    public event Action<BuildingData> OnBuildingSlotButtonClicked;
    public void Initialize(BuildingData data) 
    {
        _buildingData = data;
        Cost = data.Cost;
        Name = data.Name;
        var vm = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        RefreshCanBuild(vm.CurrentGold);
    }
    public void OnClickSlotViewButton() 
    {
        OnBuildingSlotButtonClicked?.Invoke(_buildingData);
    }
    public void RefreshCanBuild(int currentGold) 
    {
        CanBuild = GetCanBuild(currentGold);
    }
    public bool GetCanBuild(int currentGold) 
    {
        if (Cost <= currentGold) 
        {
            return true;
        }
        return false;    
    }
}
