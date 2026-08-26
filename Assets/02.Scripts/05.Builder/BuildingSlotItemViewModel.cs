using System;
using System.ComponentModel;


public class BuildingSlotItemViewModel : ViewModelBase
{
    public string Name { get; private set; }
    public int Cost { get; private set; }
  
    public BuildingType BuildingType { get; private set; }
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
        BuildingType = (BuildingType)data.BuildingType;
        _vm = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        RefreshCanBuild();
        _vm.PropertyChanged += OnViewModelPropertyChanged;
        GameManager.Instance.MapManager.OnBuildingChanged += RefreshCanBuild;
    }
    public void OnClickSlotViewButton() 
    {
        OnBuildingSlotButtonClicked?.Invoke(_buildingData);
    }
    public void RefreshCanBuild()
    {
        CanBuild = GetCanBuild(_vm.CurrentGold);
    }
    public bool GetCanBuild(int currentGold) 
    {
        bool hasEnoughGold = Cost <= currentGold;

        if (!hasEnoughGold)
        {
            return false;
        }
        if (BuildingType == BuildingType.TownHall)
        {
            bool isAlreadyBuilt =  GameManager.Instance.MapManager.IsBuildingBuilt(_buildingData.Id);

            if (isAlreadyBuilt)
            {
                return false;
            }
        }
        return true;
    }
    
    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(EconomyViewModel_DH.CurrentGold):
                RefreshCanBuild();
                break;
        }
    }
}
