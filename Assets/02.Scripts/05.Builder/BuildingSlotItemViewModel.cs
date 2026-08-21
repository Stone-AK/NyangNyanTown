using System;
using System.ComponentModel;


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
    private EconomyViewModel_DH _vm;
    public void Initialize(BuildingData data) 
    {
        _buildingData = data;
        Cost = data.Cost;
        Name = data.Name;
        _vm = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        RefreshCanBuild(_vm.CurrentGold);
        _vm.PropertyChanged += OnViewModelPropertyChanged;
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
    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(EconomyViewModel_DH.CurrentGold):
                RefreshCanBuild(_vm.CurrentGold);
                break;
        }
    }
}
