using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSlotItemView : MonoBehaviour
{
    public BuildingSlotItemViewModel _viewModel;
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI CostText;
    [SerializeField] Image CantBuildImage;
    [SerializeField] Button Button_Slot;
    private void OnEnable()
    {
        if (Button_Slot != null)
        {
            Button_Slot.onClick.AddListener(OnClickSlotButton);
        }    
    }
    private void OnDisable()
    {
        Button_Slot.onClick.RemoveListener(OnClickSlotButton);
    }
    public void Initalize(BuildingSlotItemViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += OnPropertyChanged;

        CostText.text = _viewModel.Cost.ToString();
        NameText.text = _viewModel.Name.ToString();

        CantBuildImage.gameObject.SetActive(!_viewModel.CanBuild);
    }

    private void OnClickSlotButton()
    {
        _viewModel.OnClickSlotViewButton();
    }
    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BuildingSlotItemViewModel.CanBuild))
        {
            CantBuildImage.gameObject.SetActive(!_viewModel.CanBuild);
        }
    }
    private void OnDestroy()
    {
        if (_viewModel != null)
        {
            _viewModel.PropertyChanged -= OnPropertyChanged;
        }
    }
}
