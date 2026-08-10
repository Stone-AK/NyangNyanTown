using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildUIView : MonoBehaviour
{
    [SerializeField] Button Button_Build;
    [SerializeField] Transform _layoutGroup;
    [SerializeField] BuildingSlotItemView _slotPrefab;
    [SerializeField] private GameObject _buildPanel;

    private BuildUIViewModel _viewModel;
    
    private void Awake()
    {
        _viewModel = new BuildUIViewModel();//TODO : 매니저에서 생성하도록 변경
        CreateSlots();
        _buildPanel.SetActive(false);
        BindOnClickButtonEvent(OnClickBuildButton);

    }
    public void BindOnClickButtonEvent(Action onClickCallback)
    {
        if (Button_Build == null) return;

        Button_Build.onClick.AddListener(new UnityEngine.Events.UnityAction(onClickCallback));

    }
    public void UnBindOnClickButtonEvent(Action onClickCallback)
    {
        if (Button_Build == null) return;

        Button_Build.onClick.RemoveListener(new UnityEngine.Events.UnityAction(onClickCallback));
    }
    private void OnClickBuildButton() 
    {
        _buildPanel.SetActive(!_buildPanel.activeSelf);
    }
    private void OnDestroy()
    {
        UnBindOnClickButtonEvent(OnClickBuildButton);
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
