using UnityEngine;


public class EconomyService_DH 
// 고양이 수의 체크는 건물이 늘고 줄때 계산식 돌려서 체크, 현재는 버튼을 하나 만들어서 고양이 수를 강제적으로 늘리는 방식을 써보자
// 골드 수 체크는 일정 간격으로 돌아가는 업데이트로 고양이 수에 계산식 돌려서 체크
// 계산식은 나중에 static 클래스로 관리
{
    [Header("Economy Calculation Settings")]
     private int _incomeGoldBase = 10;
     private int _specialCatCount = 0;
     private float _specialCatsMultiply = 0.1f;
     private float _goldInterval = 2.0f;

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
        economyViewModel.CatMaxCount = 0;
        economyViewModel.SpecialCatCount = 0;
        economyViewModel.BuildingCount = 0;

        return economyViewModel;
    }

    private float _timer = 0.0f;

    

    public void Update()
    {
        if (_economyViewModel == null) return;

        _timer += Time.deltaTime;

        if (_timer >= _goldInterval) // UI를 직접 수정하는 것이 아닌, 뷰모델 데이터만 변경
        {
            _timer -= _goldInterval;
            var addGold = GameManager.Instance.EconomyService_DH.GetIncomeCurrentGOld(_incomeGoldBase);
            GameManager.Instance.EconomyService_DH.AddCurrentGold(addGold);
            Debug.Log($"자동 골드 {addGold}");
        }

        //현재 EconomyService_DH 는 GameManager 안에서만 보관하고 씬에서는 살아있지 않아서 Update가 안도는 중!
        //나중에 Update 안에 있는 구문을 옮겨서 다른 곳에서도 작동하게 해야 함
    }

    public int GetIncomeCurrentGOld(int incomeGoldBase)
    {
        int IncomeGold = GameUtil.CalcEconomyGold(_economyViewModel.CatCurrentCount, incomeGoldBase, _economyViewModel.SpecialCatCount, _specialCatsMultiply);

        return IncomeGold;
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

    public void AddCatMaxCount(int CatMaxCount)
    {
        if (_economyViewModel != null)
        {
            _economyViewModel.CatMaxCount += CatMaxCount;
        }
    }

    public void RemoveCatMaxCount(int CatMaxCount)
    {
        if (_economyViewModel != null)
        {
            _economyViewModel.CatMaxCount -= CatMaxCount;
        }
    }

}