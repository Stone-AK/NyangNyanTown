using System.Collections.Generic;
using UnityEngine;


public class EconomyService_DH
// 고양이 수의 체크는 건물이 늘고 줄때 계산식 돌려서 체크, 현재는 버튼을 하나 만들어서 고양이 수를 강제적으로 늘리는 방식을 써보자
// 골드 수 체크는 일정 간격으로 돌아가는 업데이트로 고양이 수에 계산식 돌려서 체크
// 계산식은 나중에 static 클래스로 관리
{

    private EconomyViewModel_DH _economyViewModel; // 뷰모델 선언

    private readonly Dictionary<string,CatEncyclopediaViewModel> _catEncyclopediaList = new();

    public IReadOnlyDictionary<string, CatEncyclopediaViewModel> CatEncyclopediaList => _catEncyclopediaList;


    // EconomyService 생성 시 초기화에 필요한 메서드 입력 필요
    public void InitEconomyService()
    {
        InitCatEncyclopediaList();
    }

    public void InitCatEncyclopediaList()
    {
        _catEncyclopediaList.Clear();

            if (!GameManager.Instance.DataManager
                .TryGetDataTable<CatInfoData>(out var dataTable))
            {
                return;
            }

        foreach (var data in dataTable)
        {
            CatEncyclopediaViewModel catViewModel = new()
            {
                CatInfoDataId = data.Key
            };
            _catEncyclopediaList.Add(catViewModel.CatInfoDataId, catViewModel);
        }
    }

    public void SetCatEncyclopediaList(IEnumerable<CatEncyclopediaViewModel> catList)
    {
            _catEncyclopediaList.Clear();

            if (catList == null)
                return;

            foreach (var catViewModel in catList)
            {
                if (catViewModel == null)
                    continue;

                _catEncyclopediaList[catViewModel.CatInfoDataId] = catViewModel;
            }
    }

    public void UpdateSpecialCatEffects()
    {
        if (_economyViewModel == null)
        {
            Debug.LogError("[EconomyService_DH] ViewModel이 null입니다.");
            return;
        }
        int totalAdd = 0;
        float totalMulti = 0.0f;
        // 1. 도감 리스트 순회
        foreach (var pair in _catEncyclopediaList)
        {
            var catVm = pair.Value;

            // 수집되지 않은 고양이는 제외
            if (catVm == null || !catVm.IsCollected) continue;
            // 2. DataManager에서 해당 고양이의 CatInfoData 조회
            if (GameManager.Instance.DataManager.TryGetData<CatInfoData>(catVm.CatInfoDataId, out var catData))
            {
                // 3. CatEffect 타입에 따라 Add / Multi 누적
                if (catData.CatEffect == "Add")
                {
                    totalAdd += (int)catData.EffectValue;
                }
                else if (catData.CatEffect == "Multiplies")
                {
                    totalMulti += catData.EffectValue;
                }
            }
        }
        // 4. ViewModel 프로퍼티에 합산 결과 반영
        _economyViewModel.SpecialCatAdd = totalAdd;
        _economyViewModel.SpecialCatMultiply = totalMulti;
    }


    public EconomyViewModel_DH GetEconomyViewModel()
    {
        if (_economyViewModel == null)
        {
            _economyViewModel = CreateEconomyViewModel();
        }

        return _economyViewModel;
    }
    public EconomyViewModel_DH CreateEconomyViewModel()
    {
        var economyViewModel = new EconomyViewModel_DH();
        economyViewModel.CurrentGold = 10;
        economyViewModel.CatCurrentCount = 0;
        economyViewModel.SpecialCatAdd = 0;
        economyViewModel.SpecialCatMultiply = 0.0f;
        economyViewModel.BuildingCount = 0;

        return economyViewModel;
    }



    public void AddCurrentGold(int Gold)
    {
        if (_economyViewModel != null)
        {
            _economyViewModel.CurrentGold += Gold;
        }
    }

    public void RemoveCurrentGold(int Gold)
    {
        if (_economyViewModel != null)
        {
            _economyViewModel.CurrentGold -= Gold;
        }
    }



    public void AddCurrentFish(int Fish)
    {
        if (_economyViewModel != null)
        {
            _economyViewModel.CurrentFish += Fish;
        }
    }

    public void RemoveCurrentFish(int Fish)
    {
        if (_economyViewModel != null)
        {
            _economyViewModel.CurrentFish -= Fish;
        }
    }



    public void AddCatCurrentCount(int CatCurrentCount)
    {
        if (_economyViewModel != null)
        {
            _economyViewModel.CatCurrentCount += CatCurrentCount;
        }
    }

    public void RemoveCatCurrentCount(int CatCurrentCount)
    {
        if (_economyViewModel != null)
        {
            _economyViewModel.CatCurrentCount -= CatCurrentCount;
        }
    }



    public void AddCatFromBuilding(int addAmount)
    {
        if (_economyViewModel != null)
        {
            _economyViewModel.CatCurrentCount += addAmount;
            _economyViewModel.BuildingCount += 1;
        }
    }


    public void RemoveCatFromBuilding(int removeAmount)
    {
        if (_economyViewModel != null)
        {
            _economyViewModel.CatCurrentCount -= removeAmount;
            _economyViewModel.BuildingCount -= 1;
        }
    }

    public bool CheckClickCatIsNew(string catDataId)
    {
        if (!_catEncyclopediaList.TryGetValue(catDataId, out CatEncyclopediaViewModel catViewModel))
            return false;

        if (catViewModel.IsCollected)
            return false;

        catViewModel.IsCollected = true;

        UpdateSpecialCatEffects();

        return true;
    }



    //public void AddSpecialCat(SepcialCatType catType) // 추후 특수 고양이 추가에 따른 계산식 변경을 담당할 메서드
    //{

    //}
}