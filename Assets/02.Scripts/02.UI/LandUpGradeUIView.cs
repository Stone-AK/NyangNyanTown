using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public void Init(LandUpGradeUIViewModel vm) { _vm = vm; Debug.Log("vm적용 완료.");  InitializeUI();
        _vm.OnGoldChanged += OnGoldChanged;}
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
       
    }



    private void OnDisable()
    {
        Button_UpGrade.onClick.RemoveListener(OnClickUpGradeButton);
        Button_Exit.onClick.RemoveListener(OnClickExitButton);
        _vm.OnGoldChanged -= OnGoldChanged;
    }
    private void OnClickUpGradeButton() { _vm.OnClickUpGradeButton(); GameManager.Instance.UIManager.Close(UIType.LandUpGradeUI); }
    private void OnClickExitButton() { GameManager.Instance.UIManager.Close(UIType.LandUpGradeUI); }

    private void InitializeUI() 
    {
        if (_vm == null) { Debug.Log("vm이 없습니다."); }
        GoldText.text = $"{_vm.GetCurrentGold()} / {_vm.GetNeedGold()}";
        CatText.text = $"{_vm.GetCurrentCat()} / {_vm.GetNeedCat()}";
        BuildingText.text = $"{_vm.GetNeedBuildingName()}";
        SpecialCatText.text = $"{_vm.GetNeedSpecialCatName()}";
        RefreshUIContents();
        CheckCanUpGrade();
    }
    private void RefreshGoldText() 
    {
        GoldText.text = $"{_vm.GetCurrentGold()} / {_vm.GetNeedGold()}";
    }
    private void RefreshUIContents() 
    {
        GoldSlider.value = (float)_vm.GetCurrentGold() / (float)_vm.GetNeedGold();
        GoldSlider.fillRect.GetComponent<Image>().color = GoldSlider.value >= 1f ? Color.green : Color.red;

        CatSlider.value = (float)_vm.GetCurrentCat() / (float)_vm.GetNeedCat();
        CatSlider.fillRect.GetComponent<Image>().color = CatSlider.value >= 1f ? Color.green : Color.red;

    }
    private void OnGoldChanged() 
    {
        RefreshGoldText();
        RefreshUIContents();
        CheckCanUpGrade();
    }
    private void CheckCanUpGrade() 
    {
        if (_vm.CheckUpGradeAvailable()) 
        {
            Button_UpGrade.interactable = true;
        }
        else 
        {
            Button_UpGrade.interactable = false;
        }
    
    }
}
