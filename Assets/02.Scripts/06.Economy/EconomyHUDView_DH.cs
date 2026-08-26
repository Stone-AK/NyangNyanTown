using System.ComponentModel;
using TMPro;
using UnityEngine;

public class EconomyHUDView_DH : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI Text_catCount;
    [SerializeField] private TextMeshProUGUI Text_goldCount;
    [SerializeField] private TextMeshProUGUI Text_specialCatCount;

    private EconomyViewModel_DH _viewModel; // 뷰모델 선언

    public void BindViewModel(EconomyViewModel_DH viewModel)
    {
        if (viewModel == null)
        {
            Debug.LogError("[CatHUDView_DH] 전달받은 ViewModel이 null입니다.");
            return;
        }

        _viewModel = viewModel;
        _viewModel.PropertyChanged += OnViewModelPropertyChanged; // 이벤트 감시
        viewModel.InvokeOnceOnInit();
    }

    private void OnEnable()
    {
        var vm = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        BindViewModel(vm);
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(EconomyViewModel_DH.CatCurrentCount):
                if (Text_catCount != null) Text_catCount.text = _viewModel.CatCurrentCount.ToString();
                break;
            case nameof(EconomyViewModel_DH.CurrentGold):
                if (Text_goldCount != null) Text_goldCount.text = _viewModel.CurrentGold.ToString();
                break;
            case nameof(EconomyViewModel_DH.SpecialCatAdd):
                if (Text_specialCatCount != null) Text_specialCatCount.text = _viewModel.SpecialCatAdd.ToString();
                break;
            case nameof(EconomyViewModel_DH.SpecialCatMultiply):
                if (Text_specialCatCount != null) Text_specialCatCount.text = _viewModel.SpecialCatMultiply.ToString();
                break;
        }
    }

    private void OnDestroy() // 메모리 누수 방지를 위한 이벤트 해제
    {

        if (_viewModel != null)
        {
            _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }
    }
}
