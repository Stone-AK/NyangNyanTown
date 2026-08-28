using UnityEngine;

public class BuildService
{
    private MapManager _mapManager;
    private EconomyService_DH _economyService;
    private EconomyViewModel_DH _economyViewModel;

    public BuildService(MapManager mapManager, EconomyService_DH economyService)
    {
        _mapManager = mapManager;
        _economyService = economyService;
        _economyViewModel = _economyService.GetEconomyViewModel();
    }
    public bool CanPlaceOnMove(float rootX, BuildingData data, string ignoreInstanceId = null) 
    {
        return _mapManager.CanBuildingPlace(rootX, data.Width, ignoreInstanceId);
    }
    public bool CanBuildOnThisPlace(BuildingData data, float rootX) 
    {
        return _mapManager.CanBuildingPlace(rootX, data.Width) 
            && CanBuildThisBuildingType(data) 
            && IsGoldEnough(data.Cost);
    }
    public bool CanBuildOnUI(BuildingData data) 
    {
        return IsGoldEnough(data.Cost) && CanBuildThisBuildingType(data);
    }
    private bool IsGoldEnough(int cost) 
    {
        bool isGoldEnough = _economyViewModel.CurrentGold >= cost;

        return isGoldEnough;
    }
    private bool CanBuildThisBuildingType(BuildingData data) 
    {
        switch ((BuildingType)data.BuildingType) 
        {
            case BuildingType.TownHall: return !IsBuildingBuilt(data.Id);
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
        int spCount = 0;

        foreach (CatEncyclopediaViewModel vm in _economyService.CatEncyclopediaList.Values) 
        {
            if (vm.IsCollected) 
            {
                spCount++;
            }
        }
       return _economyService.CatEncyclopediaList.Count == spCount;
      // return  spCount>=1;
    }
}
