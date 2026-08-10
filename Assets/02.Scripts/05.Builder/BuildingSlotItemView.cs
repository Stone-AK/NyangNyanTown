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
    public void Initalize(BuildingSlotItemViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += OnPropertyChanged;

        CostText.text = _viewModel.Cost.ToString();
        NameText.text = _viewModel.Name.ToString();

        CantBuildImage.gameObject.SetActive(!_viewModel.CanBuild);

        BindOnClickButtonEvent(OnClickSlotButton);
    }
    public void BindOnClickButtonEvent(Action onClickCallback)
    {
        if (Button_Slot == null) return;

        Button_Slot.onClick.AddListener(new UnityEngine.Events.UnityAction(onClickCallback));

    }
    public void UnBindOnClickButtonEvent(Action onClickCallback)
    {
        if (Button_Slot == null) return;

        Button_Slot.onClick.RemoveListener(new UnityEngine.Events.UnityAction(onClickCallback));
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
        UnBindOnClickButtonEvent(OnClickSlotButton);
    }
}
