using Cysharp.Threading.Tasks;
using System;
using System.Buffers.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUIView : BaseUI
{
    [SerializeField] private Button Button_Building;
    [SerializeField] private Button Button_CatBook;
    [SerializeField] private Button Button_CatCheat;
    [SerializeField] private Button Button_FishCheat;
    [SerializeField] private Button Button_Gacha;

    [SerializeField] private TMP_Text GoldText;
    [SerializeField] private TMP_Text FishText;
    [SerializeField] private TMP_Text CatCountText;

    [SerializeField] private TMP_Text GoldChangeText;
    [SerializeField] private TMP_Text FishChangeText;
    [SerializeField] private TMP_Text CatCountChangeText;

    private EconomyViewModel_DH _vm;
    private bool _isBuildUISet = false;

    private float _onChangePropTime = 1.5f;
    public void BindViewModel(EconomyViewModel_DH vm)
    {
        _vm = vm;
        _vm.PropertyChanged += OnPropChagned_View;

    }
    private void OnEnable()
    {
        var _vm = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        BindViewModel(_vm);

        if (Button_Building != null) 
        {
            Button_Building.onClick.AddListener(OnClickBuildingButton);
        }

        if (Button_CatBook != null)
        {
            Button_CatBook.onClick.AddListener(OnClickCatBookButton);
        }

        if (Button_CatCheat != null)
        {
            Button_CatCheat.onClick.AddListener(OnClickCatCheatButton);
        }
        if (Button_FishCheat != null)
        {
            Button_FishCheat.onClick.AddListener(OnClickFishCheatButton);
        }

        if (Button_Gacha != null)
        {
            Button_Gacha.onClick.AddListener(OnClickGachaButton);
        }
        
    }


   
    private void OnDisable()
    {
        Button_Building.onClick.RemoveListener(OnClickBuildingButton);
        Button_CatBook.onClick.RemoveListener(OnClickCatBookButton);
        Button_CatCheat.onClick.RemoveListener(OnClickCatCheatButton);
        Button_FishCheat.onClick.RemoveListener(OnClickFishCheatButton);
        Button_Gacha.onClick.RemoveListener(OnClickGachaButton);


    }

    private void OnDestroy()
    {
        if (_vm != null)
        {
            _vm.PropertyChanged -= OnPropChagned_View;
        }
    }

    

    private async void OnClickBuildingButton()
    {
        if (_isBuildUISet == false)
        {
            await GameManager.Instance.UIManager.OpenBuildUIAsync();
            _isBuildUISet = !_isBuildUISet;
        }
        else
        {
            GameManager.Instance.UIManager.CloseBuildUI();
            _isBuildUISet = !_isBuildUISet;
        }
    }
    private async void OnClickCatBookButton()
    {
        BaseUI baseUI = await GameManager.Instance.UIManager.OpenPopupRootAsync(UIType.CatEncyclopediaPopUp);

        if (baseUI is CatEncyclopediaPopUp catEncyclopediaPopUp)
        {
            catEncyclopediaPopUp.InitiCatEncyclopediaPopUp();
        }
    }
    private void OnClickCatCheatButton()
    {
        GameManager.Instance.EconomyService_DH.AddCatCurrentCount(1);
    }
    private void OnClickFishCheatButton()
    {
        GameManager.Instance.EconomyService_DH.AddCurrentFish(1);
    }
    private async void OnClickGachaButton()
    {
        await GameManager.Instance.UIManager.OpenGachaUIAsync();
    }
    private void OnPropChagned_View(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(EconomyViewModel_DH.CurrentGold):
                {
                    OnChange(GoldChangeText, GoldText, _vm.CurrentGold).Forget();

                    GoldText.text = _vm.CurrentGold.ToString();
                    break;
                }
            case nameof(EconomyViewModel_DH.CurrentFish):
                {
                    OnChange(FishChangeText, FishText, _vm.CurrentFish).Forget();

                    FishText.text = _vm.CurrentFish.ToString();

                    break;
                }
            case nameof(EconomyViewModel_DH.CatCurrentCount):
                {
                    OnChange(CatCountChangeText, CatCountText, _vm.CatCurrentCount).Forget();

                    CatCountText.text = $"{_vm.CatCurrentCount.ToString()}";

                    break;
                }
           


        }
    }

    private async UniTaskVoid OnChange(TMP_Text changedText, TMP_Text beforeText, int Value)
    {

        if (int.TryParse(beforeText.text, out int beforeTextValue))
        {
            int result = Value - beforeTextValue;
            if(result > 0)
            {
                changedText.text = ("+")+result.ToString();
            }
            else
            {
                changedText.text = result.ToString();
            }

        }

        if (!changedText.gameObject.activeInHierarchy) 
        {
            changedText.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(_onChangePropTime));
            changedText.gameObject.SetActive(false);
        }
        

    }
}
