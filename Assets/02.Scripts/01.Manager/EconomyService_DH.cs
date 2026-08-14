using UnityEngine;


public class EconomyService_DH
// 고양이 수의 체크는 건물이 늘고 줄때 계산식 돌려서 체크, 현재는 버튼을 하나 만들어서 고양이 수를 강제적으로 늘리는 방식을 써보자
// 골드 수 체크는 일정 간격으로 돌아가는 업데이트로 고양이 수에 계산식 돌려서 체크
// 계산식은 나중에 static 클래스로 관리
{


    private EconomyViewModel_DH _economyViewModel; // 뷰모델 선언


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
        economyViewModel.CurrentGold = 0;
        economyViewModel.CatCurrentCount = 0;
        economyViewModel.SpecialCatCount = 0;
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



    //public void AddSpecialCat(SepcialCatType catType) // 추후 특수 고양이 추가에 따른 계산식 변경을 담당할 메서드
    //{

    //}
}