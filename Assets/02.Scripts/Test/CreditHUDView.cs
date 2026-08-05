using System.ComponentModel;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class CreditHUDView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextMesh_Credit;
    [SerializeField] private Button UpButton;
    [SerializeField] private Button DownButton;


    private GameViewModel _hudVM;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        GameViewModel gameModel = new GameViewModel();
        //GameViewModel gameModel = GameManager.Inst.CreditService.GetCreditViewModel();
        gameModel.HadCredit = 0;
        BindViewModel(gameModel);

        UpButton.onClick.AddListener(OnClick_UpButton);
        DownButton.onClick.AddListener(OnClick_DownButton);
    }

    public void BindViewModel(GameViewModel vm)
    {
        _hudVM = vm;
        _hudVM.PropertyChanged += OnPropChagned_View;
        _hudVM.InvokeInitProperty();
    }

    private void OnDestroy()
    {
        if (_hudVM != null)
        {
            _hudVM.PropertyChanged -= OnPropChagned_View;
        }
    }

    private void OnPropChagned_View(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(GameViewModel.HadCredit):
                {
                    TextMesh_Credit.text = $"{_hudVM.HadCredit} Won";
                }
                break;
            
        }
    }

    public void OnClick_UpButton()
    {
        _hudVM.HadCredit++;
    }

    public void OnClick_DownButton()
    {
        _hudVM.HadCredit--;
    }
}
