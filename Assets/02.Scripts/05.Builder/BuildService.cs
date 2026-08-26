using UnityEditor.Build.Pipeline.Utilities;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildService
{
    private MapManager _mapManager;
    private GameDataManager _dataManager;
    private EconomyService_DH _economyService;
    private EconomyViewModel_DH _economyViewModel;

    public BuildService(MapManager mapManager, EconomyService_DH economyService, GameDataManager dataManager)
    {
        _mapManager = mapManager;
        _dataManager = dataManager;
        _economyService = economyService;
        _economyViewModel = _economyService.GetEconomyViewModel();
    }
    public bool CanBuildAndPlace(Building building, float rootX) 
    {
        return _mapManager.CanBuildOnThisPlace(rootX, building._buildingData.Width, building.InstanceId) 
            && IsGoldEnough(building._buildingData.Cost);
    }
    public bool CanBuildOnThisPlace(Building building, float rootX) 
    {
        return _mapManager.CanBuildOnThisPlace(rootX, building._buildingData.Width, building.InstanceId) 
            && CanBuildBuildingType(building._buildingData) 
            && IsGoldEnough(building._buildingData.Cost);
    }
    public bool CanBuildOnUI(Building building) 
    {
        return IsGoldEnough(building._buildingData.Cost) && CanBuildBuildingType(building._buildingData);
    }
    private bool IsGoldEnough(int cost) 
    {
        bool isGoldEnough = _economyViewModel.CurrentGold >= cost;

        return isGoldEnough;
    }
    private bool CanBuildBuildingType(BuildingData data) 
    {
        switch ((BuildingType)data.BuildingType) 
        {
            case BuildingType.TownHall: return IsBuildingBuilt(data.Id);
            case BuildingType.LandMark: return IsSpCatAllCollected();
            default: return true;
        }
    }
    private bool IsBuildingBuilt(string buildingId) 
    {
        return _mapManager.IsBuildingBuilt(buildingId);
    }
    private bool IsSpCatAllCollected() 
    {
        _dataManager.TryGetDataTable<CatInfoData>(out var dataTable);
        return _economyService.CatEncyclopediaList.Count == dataTable.Count;
    }
}
