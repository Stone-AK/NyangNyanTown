using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class LandUpGradeUIView : BaseUI
{
    [SerializeField] TextMeshProUGUI GoldText;
    [SerializeField] TextMeshProUGUI CatText;
    [SerializeField] TextMeshProUGUI BuildingText;
    [SerializeField] TextMeshProUGUI SpecialCatText;
    [SerializeField] Slider GoldSlider;
    [SerializeField] Slider CatSlider;
    [SerializeField] Button Button_UpGrade;
    [SerializeField] Button Button_Exit;

    private LandUpGradeUIViewModel _vm;
    private void OnEnable()
    {

        if (Button_UpGrade != null)
        {
            Button_UpGrade.onClick.AddListener(OnClickUpGradeButton);
        }

        if (Button_Exit != null)
        {
            Button_Exit.onClick.AddListener(OnClickExitButton);
        }
        InitializeUI();
    }



    private void OnDisable()
    {
        Button_UpGrade.onClick.RemoveListener(OnClickUpGradeButton);
        Button_Exit.onClick.RemoveListener(OnClickExitButton);
    }
    private void OnClickUpGradeButton() { }
    private void OnClickExitButton() { }

    private void InitializeUI() 
    {
        GoldText.text = $"{_vm.CurrentGold} / {_vm.NeedGold}";
        CatText.text = $"{_vm.CurrentCat} / {_vm.NeedCat}";
        BuildingText.text = $"{_vm.NeedBuilding}";
        SpecialCatText.text = $"{_vm.NeedSpecialCat}";
    }
    private void RefreshGoldText() 
    {
        GoldText.text = $"{_vm.CurrentGold} / {_vm.NeedGold}";
    }

}
