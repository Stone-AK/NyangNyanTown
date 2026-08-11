using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUIView : BaseUI
{
    [SerializeField] private Button Button_Building;
    [SerializeField] private Button Button_CatList;
    [SerializeField] private TMP_Text GoldText;
    [SerializeField] private TMP_Text FishText;
    [SerializeField] private TMP_Text CatCountText;

    private CurrencyViewModel _vm;

    public void BindViewModel(CurrencyViewModel vm)
    {
        _vm = vm;
        _vm.PropertyChanged += OnPropChagned_View;
        _vm.InvokeOnceOnInit();

    }
    private void OnEnable()
    {
        var _vm = GameManager.Instance.CurrencyService.GetCurrencyViewModel();
        BindViewModel(_vm);

        if (Button_Building != null) 
        {
            Button_Building.onClick.AddListener(OnButtonClickedBuilding);
        }
        if (Button_CatList != null)
        {
            Button_CatList.onClick.AddListener(OnButtonClickedCatList);
        }
    }


   
    private void OnDisable()
    {
        Button_Building.onClick.RemoveListener(OnButtonClickedBuilding);
        Button_CatList.onClick.RemoveListener(OnButtonClickedCatList);
    }

    private void OnDestroy()
    {
        if (_vm != null)
        {
            _vm.PropertyChanged -= OnPropChagned_View;
        }
    }

    

    private void OnButtonClickedBuilding()
    {
        Debug.Log("Button_Building");
    }
    private void OnButtonClickedCatList()
    {
        Debug.Log("Button_CatList");

    }

    private void OnPropChagned_View(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(CurrencyViewModel.Gold):
                {
                    GoldText.text = _vm.Gold.ToString();
                    break;
                }
            case nameof(CurrencyViewModel.Fish):
                {
                    FishText.text = _vm.Fish.ToString();
                    break;
                }
            case nameof(CurrencyViewModel.CatCurrentCount):
                {
                    CatCountText.text = $"{_vm.CatCurrentCount.ToString()} / {_vm.CatMaxCount.ToString()}";
                    break;
                }
            case nameof(CurrencyViewModel.CatMaxCount):
                {
                    CatCountText.text = $"{_vm.CatCurrentCount.ToString()} / {_vm.CatMaxCount.ToString()}";
                    break;
                }


        }
    }
}
