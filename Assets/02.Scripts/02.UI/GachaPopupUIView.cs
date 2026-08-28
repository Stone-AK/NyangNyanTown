using Cysharp.Threading.Tasks;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaPopupUIView : BaseUI
{
    [SerializeField] private Button ExitBackButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button OneTimeGachaButton;
    [SerializeField] private Button TenTimeGachaButton;
    [SerializeField] private TMP_Text OneTimeGachaText;
    [SerializeField] private TMP_Text TenTimeGachaText;

    private EconomyViewModel_DH _economyVM;

    private int OneTime = 1;
    private int TenTime = 10;
    public void BindViewModel(EconomyViewModel_DH viewModel)
    {
        if (viewModel == null)
        {
            Debug.LogError("[CatHUDView_DH] 전달받은 ViewModel이 null입니다.");
            return;
        }

        _economyVM = viewModel;
        _economyVM.PropertyChanged += OnPropChagned_View; // 이벤트 감시
        viewModel.InvokeOnceOnInit();
    }

    private void Awake()
    {
        var vm = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        BindViewModel(vm);
    }
    private void OnEnable()
    {
        if (ExitBackButton != null)
        {
            ExitBackButton.onClick.AddListener(OnClickExitButton);
        }
        if (ExitButton != null)
        {
            ExitButton.onClick.AddListener(OnClickExitButton);
        }

        if (OneTimeGachaButton != null)
        {
            OneTimeGachaButton.onClick.AddListener(OnClickOneTimeGachaButton);
        }

        if (TenTimeGachaButton != null)
        {

            TenTimeGachaButton.onClick.AddListener(OnClickTenTimeGachaButton);
        }
      
    }
   
    private void OnDisable()
    {
        if (ExitBackButton != null)
        {
            ExitBackButton.onClick.RemoveListener(OnClickExitButton);
        }
        if (ExitButton != null)
        {
            ExitButton.onClick.RemoveListener(OnClickExitButton);
        }

        if (OneTimeGachaButton != null)
        {
            OneTimeGachaButton.onClick.RemoveListener(OnClickOneTimeGachaButton);
        }

        if (TenTimeGachaButton != null)
        {

            TenTimeGachaButton.onClick.RemoveListener(OnClickTenTimeGachaButton);
        }
    }
    private void OnDestroy()
    {
        if (_economyVM != null)
        {
            _economyVM.PropertyChanged -= OnPropChagned_View;
        }
    }

    private void OnClickExitButton()
    {
        GameManager.Instance.UIManager.CloseGacha();
    }

    private void OnClickOneTimeGachaButton()
    {
        _economyVM = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        if (_economyVM.CurrentFish >= 1)
        {
            GameManager.Instance.EconomyService_DH.RemoveCurrentFish(OneTime);
            GameManager.Instance.GachaManager.TryGachaByCount(OneTime).Forget();
        }
        else
        {

        }
    }

    private void OnClickTenTimeGachaButton()
    {
        _economyVM = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();
        if (_economyVM.CurrentFish >= 10)
        {
            GameManager.Instance.EconomyService_DH.RemoveCurrentFish(TenTime);
            GameManager.Instance.GachaManager.TryGachaByCount(TenTime).Forget();

        }
        else
        {

        }
    }

    private void OnPropChagned_View(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {

            case nameof(EconomyViewModel_DH.CurrentFish):
                {
                    if(_economyVM.CurrentFish >= 1)
                    {
                        OneTimeGachaText.color = Color.white;
                    }
                    else
                    {
                        OneTimeGachaText.color = Color.red;

                    }

                    if (_economyVM.CurrentFish >= 10)
                    {
                        TenTimeGachaText.color = Color.white;
                    }
                    else
                    {
                        TenTimeGachaText.color = Color.red;

                    }

                    break;
                }

        }
    }
}
