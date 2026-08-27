using Cysharp.Threading.Tasks;
using System;
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

    private IReadOnlyDictionary<string, CatEncyclopediaViewModel> _catEncyclopediaList;
    private Dictionary<string, CatEncyclopediaSlotBtn> _catListDictionary = new();
    private bool _isInitialized = false;

    private void OnPropChagned_View(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(CatEncyclopediaViewModel.IsCollected):
                {
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

        if (GameManager.Instance?.EconomyService_DH == null)
            return;

        _catEncyclopediaList = GameManager.Instance.EconomyService_DH.CatEncyclopediaList;

        _isInitialized = true;
        CollectImage.gameObject.SetActive(false);

        foreach (var catViewModel in _catEncyclopediaList.Values)
        {
            catViewModel.PropertyChanged += OnPropChagned_View;

            SetCatSlot(catViewModel.CatInfoDataId, catViewModel.IsCollected);
        }
    }

    public void SetCatSlot(string catId, bool isCollected)
    {
        // 기존 Dictionary가 없는 상황
        if (!_catListDictionary.TryGetValue(catId, out CatEncyclopediaSlotBtn catSlot))
        {
            catSlot = Instantiate(CatSlotBtnPrefab, SlotContent);

            catSlot.BindOnClickSlotButton(() => RenewalText(catId));
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

    private void RenewalText(string dataId)
    {
        if (!_catEncyclopediaList.TryGetValue(dataId, out CatEncyclopediaViewModel textCatViewModel))
            return;

        if (textCatViewModel.IsCollected == false)
        {
            CollectImage.gameObject.SetActive(false);

            CatNameText.text = "???";
            CatDescriptionText.text = "수집되지 않았습니다.";
            return;
        }

        CollectImage.gameObject.SetActive(true);

        if (GameManager.Instance.DataManager.TryGetData(dataId, out CatInfoData catInfoData))
        {
            CatNameText.text = catInfoData.Name;
            CatDescriptionText.text = catInfoData.Description;
        }
    }
}
