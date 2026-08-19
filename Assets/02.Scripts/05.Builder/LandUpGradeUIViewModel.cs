using System;
using System.ComponentModel;
using UnityEngine;

public class LandUpGradeUIViewModel : ViewModelBase
{
    public int GetCurrentGold() { return _landUpGradeService.GetCurrentGold(); } 
    public int GetNeedGold() { return _landUpGradeService.GetNeedUpGradeGold(); }
    public int GetCurrentCat() { return _landUpGradeService.GetCurrentCat(); }
    public int GetNeedCat() { return _landUpGradeService.GetNeedUpGradeCat(); }
    public string GetNeedBuildingName() 
    {
        if (GameManager.Instance.DataManager.TryGetData(_landUpGradeService.GetNeedBuildingId(), out BuildingData data)) 
        {
            return data.Name;
        }
        return null;
    }
    public string GetNeedSpecialCatName() 
    {
        if (GameManager.Instance.DataManager.TryGetData(_landUpGradeService.GetNeedSpecialCatId(), out CatInfoData data))
        {
            return data.Name;
        }
        return null;
    }

    public event Action OnGoldChanged;

    private LandUpGradeService _landUpGradeService;
    public bool IsGoldEnough()
    {
        return _landUpGradeService.IsGoldEnough();
    }
    public bool IsCatEnough()
    {
        return _landUpGradeService.IsCatEnough();
    }
    public bool IsBuildingEnough()
    {
        return _landUpGradeService.IsBuildingEnough();
    }
    public bool IsSpecialCatEnough() { return _landUpGradeService.IsCatEnough(); }

    public bool CheckUpGradeAvailable()
    {
        return _landUpGradeService.CanUpGradeLand();
    }
    public void OnClickUpGradeButton() 
    {
        _landUpGradeService.LandUpGrade();
    }
    public LandUpGradeUIViewModel(LandUpGradeService landUpGradeService)
    {
        _landUpGradeService = landUpGradeService;
        var _economyVm = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        _economyVm.PropertyChanged += OnPropChagned;
    }
    private void OnPropChagned(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(EconomyViewModel_DH.CurrentGold))
        {
            OnGoldChanged?.Invoke();
        }
    }
}
