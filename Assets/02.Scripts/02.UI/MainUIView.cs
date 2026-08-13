using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUIView : BaseUI
{
    [SerializeField] private Button Button_Building;
    [SerializeField] private Button Button_CatList;
    [SerializeField] private Button Button_CatCheat;
    [SerializeField] private Button Button_FishCheat;

    [SerializeField] private TMP_Text GoldText;
    [SerializeField] private TMP_Text FishText;
    [SerializeField] private TMP_Text CatCountText;

    private EconomyViewModel_DH _vm;

    public void BindViewModel(EconomyViewModel_DH vm)
    {
        _vm = vm;
        _vm.PropertyChanged += OnPropChagned_View;
        _vm.InvokeOnceOnInit();

    }
    private void OnEnable()
    {
        var _vm = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        BindViewModel(_vm);

        if (Button_Building != null) 
        {
            Button_Building.onClick.AddListener(OnClickBuildingButton);
        }
        if (Button_CatList != null)
        {
            Button_CatList.onClick.AddListener(OnClickCatListButton);
        }

        if (Button_CatCheat != null)
        {
            Button_CatCheat.onClick.AddListener(OnClickCatCheatButton);
        }
        if (Button_FishCheat != null)
        {
            Button_FishCheat.onClick.AddListener(OnClickFishCheatButton);
        }
    }


   
    private void OnDisable()
    {
        Button_Building.onClick.RemoveListener(OnClickBuildingButton);
        Button_CatList.onClick.RemoveListener(OnClickCatListButton);
    }

    private void OnDestroy()
    {
        if (_vm != null)
        {
            _vm.PropertyChanged -= OnPropChagned_View;
        }
    }

    

    private void OnClickBuildingButton()
    {
        Debug.Log("Button_Building");
    }
    private void OnClickCatListButton()
    {
        Debug.Log("Button_CatList");

    }

    private void OnClickCatCheatButton()
    {
        GameManager.Instance.EconomyService_DH.AddCatCurrentCount(1);


    }

    private void OnClickFishCheatButton()
    {
        GameManager.Instance.EconomyService_DH.AddCurrentFish(1);

    }

    private void OnPropChagned_View(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(EconomyViewModel_DH.CurrentGold):
                {
                    GoldText.text = _vm.CurrentGold.ToString();
                    break;
                }
            case nameof(EconomyViewModel_DH.CurrentFish):
                {
                    FishText.text = _vm.CurrentFish.ToString();
                    break;
                }
            case nameof(EconomyViewModel_DH.CatCurrentCount):
                {
                    CatCountText.text = $"{_vm.CatCurrentCount.ToString()}";
                    break;
                }
           


        }
    }
}
