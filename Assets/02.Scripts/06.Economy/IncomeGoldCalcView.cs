using UnityEngine;

public class IncomeGoldCalcView : MonoBehaviour
{
    [Header("Economy Calculation Settings")]
    private int _incomeGoldBase = 10;
    private int _specialCatsAdd = 1;
    private float _specialCatsMultiply = 0.1f;
    private float _goldInterval = 2.0f;


    private float _timer = 0.0f;

    private EconomyViewModel_DH _viewModel;

    public void BindViewModel(EconomyViewModel_DH viewModel)
    {
        if (viewModel == null)
        {
            Debug.LogError("[CatHUDView_DH] 전달받은 ViewModel이 null입니다.");
            return;
        }

        _viewModel = viewModel;
        viewModel.InvokeOnceOnInit();
    }

    private void Start()
    {
        var vm = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        BindViewModel(vm);
    }

    public void Update()
    {
        if (_viewModel == null) return;

        _timer += Time.deltaTime;

        if (_timer >= _goldInterval) // UI를 직접 수정하는 것이 아닌, 뷰모델 데이터만 변경
        {
            _timer -= _goldInterval;
            var addGold = GetIncomeCurrentGold(_incomeGoldBase);
            GameManager.Instance.EconomyService_DH.AddCurrentGold(addGold);
            Debug.Log($"자동 골드 {addGold}");
        }

        //현재 EconomyService_DH 는 GameManager 안에서만 보관하고 씬에서는 살아있지 않아서 Update가 안도는 중!
        //나중에 Update 안에 있는 구문을 옮겨서 다른 곳에서도 작동하게 해야 함
    }

    public int GetIncomeCurrentGold(int incomeGoldBase)
    {
        int IncomeGold = GameUtil.CalcEconomyGold(_viewModel.CatCurrentCount, incomeGoldBase, _viewModel.SpecialCatAdd, _viewModel.SpecialCatMultiply);

        return IncomeGold;
    }
}
