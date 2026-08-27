using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildConfirmPopUpUIView : BaseUI
{
    [SerializeField] private Button Button_Bgcancel;
    [SerializeField] private Button Button_Build;
    [SerializeField] private Button Button_Exit;
    [SerializeField] private TextMeshProUGUI Text_Name;
    [SerializeField] private TextMeshProUGUI Text_Cost;
    [SerializeField] private TextMeshProUGUI Text_Cat;
    [SerializeField] private TextMeshProUGUI Text_Description;
    [SerializeField] private TextMeshProUGUI Text_Requirement;
    [SerializeField] private GameObject Slot_Name;
    [SerializeField] private GameObject Slot_Cost;
    [SerializeField] private GameObject Slot_Cat;
    [SerializeField] private GameObject Slot_Description;
    [SerializeField] private GameObject Slot_Requirement;

    private BuildConfirmPopUpUIViewModel _vm;
    private void OnEnable()
    {
        if (Button_Bgcancel != null)
        {
            Button_Bgcancel.onClick.AddListener(OnClickExitButton);
        }
        if (Button_Build != null)
        {
            Button_Build.onClick.AddListener(OnClickBuildButton);
        }
        if (Button_Exit != null)
        {
            Button_Exit.onClick.AddListener(OnClickExitButton);
        }
    }
    private void OnDisable()
    {
        Button_Bgcancel.onClick.RemoveListener(OnClickExitButton);
        Button_Build.onClick.RemoveListener(OnClickBuildButton);
        Button_Exit.onClick.RemoveListener(OnClickExitButton);
    }
    public void Init(BuildConfirmPopUpUIViewModel vm) 
    {
        _vm = vm;
        InitContents();
    }
    private void InitContents() 
    {
        Text_Name.text = _vm.Name;
        Text_Cost.text = _vm.Cost.ToString();
        MakeCatText();
        MakeDescriptionText();
        MakeRequirementText();
    }
    private void MakeDescriptionText() 
    {
        if (_vm.SpCatName != null)
        {
            Slot_Description.gameObject.SetActive(true);
            Text_Description.text = _vm.SpCatName + " 등장확률 증가!";
        }
        else 
        {
            Slot_Description.gameObject.SetActive(false);
        }
    }
    private void MakeRequirementText()
    {
        Slot_Requirement.gameObject.SetActive(true);
        switch (_vm.Type) 
        {
            case BuildingType.TownHall: Text_Requirement.text = "중복 설치 불가능"; break;
            case BuildingType.LandMark: Text_Requirement.text = "설치 조건 : ???"; break;
            default: Slot_Requirement.gameObject.SetActive(false);break;
        }
    }
    private void MakeCatText() 
    {
        if (_vm.Cat == 0)
        {
            Slot_Cat.gameObject.SetActive(false);
        }
        else 
        {
            Slot_Cat.gameObject.SetActive(true);
            Text_Cat.text = "+ "+_vm.Cat.ToString();
        }
    }
    private void OnClickExitButton() 
    {
        GameManager.Instance.UIManager.Close(UIType.BuildConfirmPopUpUI);
    }
    private void OnClickBuildButton() 
    {
        GameManager.Instance.BuildManager.StartBuild(_vm._data, BuildMode.Build).Forget();
        GameManager.Instance.UIManager.Close(UIType.BuildConfirmPopUpUI);
    }
}
