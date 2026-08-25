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
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] Slider GoldSlider;
    [SerializeField] Slider CatSlider;
    [SerializeField] Button Button_UpGrade;
    [SerializeField] Button Button_Exit;
    [SerializeField] Button Button_ExitBg;
    [SerializeField] GameObject UpgradeCompleteContainer;

    private LandUpGradeUIViewModel _vm;
    public void Init(LandUpGradeUIViewModel vm) 
    {
        _vm = vm;
        Debug.Log("vm적용 완료.");  
        InitializeUI();
        _vm.OnGoldChanged += OnGoldChanged;
    }
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
        if (Button_ExitBg != null)
        {
            Button_ExitBg.onClick.AddListener(OnClickExitButton);
        }
        InitializeUI();
    }



    private void OnDisable()
    {
        Button_UpGrade.onClick.RemoveListener(OnClickUpGradeButton);
        Button_Exit.onClick.RemoveListener(OnClickExitButton);
        Button_ExitBg.onClick.RemoveListener(OnClickExitButton);
        _vm.OnGoldChanged -= OnGoldChanged;
    }
    private void OnClickUpGradeButton() { _vm.OnClickUpGradeButton(); GameManager.Instance.UIManager.Close(UIType.LandUpGradeUI); }
    private void OnClickExitButton() { GameManager.Instance.UIManager.Close(UIType.LandUpGradeUI); }

    private void InitializeUI() 
    {
        if (_vm == null) 
        { 
            Debug.Log("vm이 없습니다."); 
            return; 
        }
        if (IsUpgradeComplete()) 
        {
            return;
        }
        LvText.text = $"Lv.{_vm.GetCurrentLandLevel()} -> Lv.{_vm.GetCurrentLandLevel()+1}";
        GoldText.text = $"{_vm.GetCurrentGold()} / {_vm.GetNeedGold()}";
        CatText.text = $"{_vm.GetCurrentCat()} / {_vm.GetNeedCat()}";
        BuildingText.text = $"건설 필요 : {_vm.GetNeedBuildingName()}";
        if (_vm.IsBuildingEnough())
        {
            BuildingText.color = Color.green;
        }
        else 
        {
            BuildingText.color = Color.red;
        }
        if (_vm.GetNeedSpecialCatName() == null)
        {
            SpecialCatText.text = $"필요 고양이 없음!";
        }
        else
        {
            SpecialCatText.text = $"수집 필요 : {_vm.GetNeedSpecialCatName()}";
            if (_vm.IsSpecialCatEnough())
            {
                SpecialCatText.color = Color.green;
            }
            else
            {
                SpecialCatText.color = Color.red;
            }
        }
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
    private bool IsUpgradeComplete() 
    {
        if (_vm.CheckUpGradeComplete())
        {
            UpgradeCompleteContainer.SetActive(true);
            return true;
        }
        return false;
    }
}
