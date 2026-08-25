using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandUpGradeService
{
    private EconomyService_DH _economyService;
    private Dictionary<string, PlacedBuildingData> _currentBuildingLDic;
    private Dictionary<string, LandUpGradeData> _landUpGradeDataTable;
    private LandUpGradeData _landUpGradeData;
    private LandViewModel _landViewModel;
    private EconomyViewModel_DH _economyViewModel;
    public LandUpGradeService(EconomyService_DH economyService, MapManager mapManager) 
    {
        _economyService = economyService;
        _currentBuildingLDic = mapManager._currentBuildingLDic;
        _landViewModel = mapManager._lvm;
        if (GameManager.Instance.DataManager.TryGetDataTable( out Dictionary<string, LandUpGradeData> landUpGradeDataTable))
        {
          _landUpGradeDataTable = landUpGradeDataTable;
        }
        _economyViewModel = _economyService.GetEconomyViewModel();
        GetCurrentLandUpGradeData(_landViewModel.LandLevel);
        _landViewModel.OnLandLevelUp += GetCurrentLandUpGradeData;
    }
    public bool IsGoldEnough() 
    {
        if (_economyViewModel.CurrentGold >= _landUpGradeData.NeedGold)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
    public bool IsCatEnough() 
    {
        if (_economyViewModel.CatCurrentCount >= _landUpGradeData.NeedCat)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsBuildingEnough() 
    {
        foreach (PlacedBuildingData data in _currentBuildingLDic.Values)
        {
            if (data.BuildingID == _landUpGradeData.NeedBuildingId)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsSpecialCatEnough() 
    {
        if (_landUpGradeData.NeedSpecialCatId == null) 
        {
            return true;
        }
        if (_economyService.CatEncyclopediaList.TryGetValue(_landUpGradeData.NeedSpecialCatId, out var vm)) 
        {
            if (vm.IsCollected == true) 
            {
                return true;
            }
        }
        return false; 
    }
    public int GetNeedUpGradeGold() 
    {
        return _landUpGradeData.NeedGold;
    }
    public int GetNeedUpGradeCat()
    {
        return _landUpGradeData.NeedCat;
    }
    public int GetCurrentGold() 
    {
        return _economyViewModel.CurrentGold;
    }
    public int GetCurrentCat()
    {
        return _economyViewModel.CatCurrentCount;
    }
    public int GetCurrentLandLevel() 
    {
        return _landViewModel.LandLevel;
    }
    public string GetNeedBuildingId() 
    {
    return _landUpGradeData.NeedBuildingId;
    }
    public string GetNeedSpecialCatId() 
    {
        return _landUpGradeData.NeedSpecialCatId;
    }
    public bool CanUpGradeLand() 
    {
        return IsGoldEnough() && IsCatEnough() && IsBuildingEnough() && IsSpecialCatEnough();
    }
    public bool IsUpgradeComplete() 
    {
        if (_landUpGradeData == null) 
        {
            return true;
        }
        return false;
    
    }
    public void GetCurrentLandUpGradeData(int landLevel) 
    {
        if (landLevel < 0 || landLevel >= _landUpGradeDataTable.Count)
        {
            Debug.LogWarning($"랜드 레벨 {landLevel}에 해당하는 데이터가 없습니다.");
            _landUpGradeData = null;
            return;
        }

        _landUpGradeData = _landUpGradeDataTable.Values.ElementAt(landLevel);
    }

    public bool LandUpGrade()
    {
        if (!CanUpGradeLand())
            return false;

        _economyService.RemoveCurrentGold(GetNeedUpGradeGold());
        _landViewModel.LandLevelUp();

        return true;
    }
}
