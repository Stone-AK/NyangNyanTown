using Cysharp.Threading.Tasks;
using System;
using System.ComponentModel;
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
    [SerializeField] private Button Button_LayoutOpen;

    [SerializeField] private TMP_Text GoldText;
    [SerializeField] private TMP_Text FishText;
    [SerializeField] private TMP_Text CatCountText;

    [SerializeField] private TMP_Text GoldChangeText;
    [SerializeField] private TMP_Text FishChangeText;
    [SerializeField] private TMP_Text CatCountChangeText;

    [SerializeField] private GameObject ButtonLayout;

    private EconomyViewModel_DH _vm;
    private bool _isBuildUISet = false;
    private bool _isButtonLayoutSet = false;

    private float _onChangePropTime = 1.5f;

    private int _previousGold;
    private int _previousFish;
    private int _previousCatCount;
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

        if (Button_LayoutOpen != null)
        {
            Button_LayoutOpen.onClick.AddListener(OnclickLayoutOpenButton);
        }


    }



    private void OnDisable()
    {
        Button_Building.onClick.RemoveListener(OnClickBuildingButton);
        Button_CatBook.onClick.RemoveListener(OnClickCatBookButton);
        Button_CatCheat.onClick.RemoveListener(OnClickCatCheatButton);
        Button_FishCheat.onClick.RemoveListener(OnClickFishCheatButton);
        Button_Gacha.onClick.RemoveListener(OnClickGachaButton);
        Button_LayoutOpen.onClick.RemoveListener(OnclickLayoutOpenButton);


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
        GameManager.Instance.EconomyService_DH.AddCatCurrentCount(10);
    }
    private void OnClickFishCheatButton()
    {
        GameManager.Instance.EconomyService_DH.AddCurrentFish(10);
    }
    private async void OnClickGachaButton()
    {
        await GameManager.Instance.UIManager.OpenGachaUIAsync();
    }
    private void OnclickLayoutOpenButton()
    {
        if (_isButtonLayoutSet == false)
        {
            ButtonLayout.SetActive(true);
            _isButtonLayoutSet = !_isButtonLayoutSet;
        }
        else
        {
            ButtonLayout.SetActive(false);
            _isButtonLayoutSet = !_isButtonLayoutSet;
        }
    }
    private void OnPropChagned_View(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(EconomyViewModel_DH.CurrentGold):
                {
                    int current = _vm.CurrentGold;

                    OnChange(GoldChangeText, _previousGold, _vm.CurrentGold).Forget();
                    _previousGold = current;

                    GoldText.text = GameUtil.ToCompact(current);
                    break;
                }
            case nameof(EconomyViewModel_DH.CurrentFish):
                {
                    int current = _vm.CurrentFish;

                    OnChange(FishChangeText, _previousFish, current).Forget();
                    _previousFish = current;

                    FishText.text = GameUtil.ToCompact(current);

                    break;
                }
            case nameof(EconomyViewModel_DH.CatCurrentCount):
                {
                    int current = _vm.CatCurrentCount;

                    OnChange(CatCountChangeText, _previousCatCount, current).Forget();
                    _previousCatCount = current;

                    CatCountText.text = GameUtil.ToCompact(current);

                    break;
                }



        }
    }

    private async UniTaskVoid OnChange(TMP_Text changedText, int beforeVelue, int Value)
    {


        int result = Value - beforeVelue;
        string resultString = GameUtil.ToCompact(result);

        if (result > 0)
        {
            changedText.text = ("+") + resultString;
        }
        else
        {
            changedText.text = resultString;
        }

        if (!changedText.gameObject.activeInHierarchy)
        {
            changedText.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(_onChangePropTime));
            changedText.gameObject.SetActive(false);
        }


    }
}
