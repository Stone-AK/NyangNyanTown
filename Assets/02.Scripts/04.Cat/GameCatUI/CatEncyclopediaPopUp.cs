using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatEncyclopediaPopUp : BaseUI
{
    [SerializeField] private TextMeshProUGUI CatNameText;
    [SerializeField] private TextMeshProUGUI CatDescriptionText;
    [SerializeField] private CatEncyclopediaSlotBtn CatSlotBtnPrefab;
    [SerializeField] private Transform SlotContent;
    [SerializeField] private Image CollectImage;

    // TODO(안우재/08.14) : 추후 EconomyService의 리스트 사용 예정 아래 리스트는 임시 리스트
    private List<CatEncyclopediaViewModel> _catEncyclopediaList = new();
    private Dictionary<string, CatEncyclopediaSlotBtn> _catListDictionary = new();
    private bool _isInitialized = false;

    private void BindCatEncyclopedViewModel(CatEncyclopediaViewModel catEncyclopedVM)
    {
        if (catEncyclopedVM == null)
            return;
        catEncyclopedVM.PropertyChanged += OnPropChagned_View;
    }

    private void OnPropChagned_View(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(CatEncyclopediaViewModel.IsCollected):
                {
                    // TODO(안우재/08.14) : isCollected의 값에 따라 slot이 달라져야함
                    if (sender is not CatEncyclopediaViewModel catViewModel)
                        return;
                    SetCatSlot(catViewModel.CatInfoDataId, catViewModel.IsCollected);
                }
                break;
        }
    }

    public void InitiCatEncyclopediaPopUp()
    {
        if (_isInitialized)
            return;

        _isInitialized = true;

        // TODO(안우재/08.14) : 추후 EconomyService의 리스트 사용 예정 임시초기화(InitCatEncyclopedList()메서드 삭제)
        InitCatEncyclopedList();
        CollectImage.gameObject.SetActive(false);

    }

    private void InitCatEncyclopedList()
    {
        if (GameManager.Instance.DataManager.TryGetDataTable<CatInfoData>(out var dataTable))
        {
            foreach (var data in dataTable)
            {
                CatEncyclopediaViewModel newCatData = new();
                newCatData.CatInfoDataId = data.Key;
                BindCatEncyclopedViewModel(newCatData);
                _catEncyclopediaList.Add(newCatData);
                SetCatSlot(data.Key, newCatData.IsCollected);
            }
        }
    }

    private void SetCatSlot(string catId, bool isCollected)
    {
        // 기존 Dictionary가 없는 상황
        if (!_catListDictionary.TryGetValue(catId, out CatEncyclopediaSlotBtn catSlot))
        {
            catSlot = Instantiate(CatSlotBtnPrefab, SlotContent);

            _catListDictionary.Add(catId, catSlot);
        }

        if (GameManager.Instance.DataManager.TryGetData(catId, out CatInfoData catInfoData))
        {
            catSlot.SetSlotImageAsync(catInfoData.CatIconImgPath, isCollected).Forget();
        }
    }

    public void ClosePopUp()
    {
        GameManager.Instance.UIManager.Close(UIType.CatEncyclopediaPopUp);
    }
}
