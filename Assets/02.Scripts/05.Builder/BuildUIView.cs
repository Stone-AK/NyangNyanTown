using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildUIView : BaseUI
{
    [SerializeField] Transform _layoutGroup;
    [SerializeField] BuildingSlotItemView _slotPrefab;

    private BuildUIViewModel _viewModel;
    
    private void Awake()
    {
        _viewModel = new BuildUIViewModel();//TODO : 매니저에서 생성하도록 변경
        CreateSlots();
    }
   
    private void CreateSlots() 
    {
        foreach (var slotViewModel in _viewModel._itemSlots)
        {
            BuildingSlotItemView slotView = Instantiate(_slotPrefab, _layoutGroup);
            slotView.Initalize(slotViewModel);
        }
    }
}
