using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class CatEncyclopediaPopUp : BaseUI
{
    [SerializeField] private TextMeshProUGUI CatNameText;
    [SerializeField] private TextMeshProUGUI CatDescriptionText;
    [SerializeField] private CatEncyclopediaSlotBtn CatSlotBtnPrefab;
    [SerializeField] private Transform SlotContent;

    private List<CatEncyclopediaViewModel> _catEncyclopediaList = new();

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
                    
                }
                break;
        }
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
            }
        }
    }
}
