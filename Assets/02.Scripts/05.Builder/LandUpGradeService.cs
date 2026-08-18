using System.Collections.Generic;
using UnityEngine;

public class LandUpGradeService
{
    private EconomyService_DH _economyService;
    private Dictionary<string, PlacedBuildingData> _currentBuildingLDic;
    private LandUpGradeData _landUpGradeData;
    private LandViewModel _landViewModel;
    public LandUpGradeService(EconomyService_DH economyService, MapManager mapManager,LandUpGradeData landUpGradeData) 
    {
        _economyService = economyService;
        _currentBuildingLDic = mapManager._currentBuildingLDic;
        _landViewModel = mapManager._lvm;
        _landUpGradeData = landUpGradeData;
    }

}
