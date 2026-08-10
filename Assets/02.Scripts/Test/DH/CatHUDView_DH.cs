using System.ComponentModel;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatHUDView_DH : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI Text_catCount;
    [SerializeField] private TextMeshProUGUI Text_goldCount;
    [SerializeField] private TextMeshProUGUI Text_BuildingCount;

    private CatViewModel_DH _viewModel; // 뷰모델 선언

    public void BindViewModel(CatViewModel_DH viewModel)
    {
        if (_viewModel != null)
        {
            Debug.LogError("[CatHUDView_DH] 전달받은 ViewModel이 null입니다.");
            return;
        }

        _viewModel = viewModel;
        _viewModel.PropertyChanged += OnViewModelPropertyChanged; // 이벤트 감시
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(CatViewModel_DH.CatCount):
                if (Text_catCount != null) Text_catCount.text = _viewModel.CatCount.ToString();
                break;
            case nameof(CatViewModel_DH.CurrentGold):
                if (Text_goldCount != null) Text_goldCount.text = _viewModel.CurrentGold.ToString();
                break;
            case nameof(CatViewModel_DH.BuildingCount):
                if (Text_BuildingCount != null) Text_BuildingCount.text = _viewModel.BuildingCount.ToString();
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
