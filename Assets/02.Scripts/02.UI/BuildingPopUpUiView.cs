using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPopUpUIView : BaseUI
{
    [SerializeField] Button Button_Move;
    [SerializeField] Button Button_Destroy;
    [SerializeField] Button Button_Exit;
    [SerializeField] Button Button_CancelBg;
    [SerializeField] Button Button_BuyLand;
    [SerializeField] TextMeshProUGUI Text_Description;
    [SerializeField] GameObject TownHallContainer;
    private Building _building;
    private void OnEnable()
    {
        if (Button_Move != null)
        {
            Button_Move.onClick.AddListener(OnClickMoveButton);
        }
        if (Button_Destroy != null)
        {
            Button_Destroy.onClick.AddListener(OnClickDestroyButton);
        }
        if (Button_Exit != null)
        {
            Button_Exit.onClick.AddListener(OnClickExitButton);
        }
        if (Button_CancelBg != null)
        {
            Button_CancelBg.onClick.AddListener(OnClickExitButton);
        }
        if (Button_BuyLand != null)
        {
            Button_BuyLand.onClick.AddListener(OnClickBuyLandButton);
        }

    }
    private void OnDisable()
    {
        Button_Move.onClick.RemoveListener(OnClickMoveButton);
        Button_Destroy.onClick.RemoveListener(OnClickDestroyButton);
        Button_Exit.onClick.RemoveListener(OnClickExitButton);
        Button_CancelBg.onClick.RemoveListener(OnClickExitButton);
        Button_BuyLand.onClick.RemoveListener(OnClickBuyLandButton);
    }
    public void Initialize(Building building)
    {
        gameObject.SetActive(true);
        Debug.Log($"{building._buildingData.Name},{building.InstanceId}");
        _building = building;
        InitDescription();
        if (_building._buildingData.BuildingType == (int)BuildingType.TownHall)
        {
            TownHallContainer.gameObject.SetActive(true);
        }
        else
        {
            TownHallContainer.gameObject.SetActive(false);
        }
    }
    private void OnClickMoveButton() 
    {
        GameManager.Instance.BuildManager.MoveBuilding(_building);
        GameManager.Instance.UIManager.Close(UIType.BuildingPopUpUI); 
    }
    private void OnClickDestroyButton() 
    { 
        GameManager.Instance.BuildManager.DestroyBuilding(_building); 
        GameManager.Instance.UIManager.Close(UIType.BuildingPopUpUI); 
    }
    private void OnClickExitButton() 
    {
        GameManager.Instance.UIManager.Close(UIType.BuildingPopUpUI);
    }
    private void OnClickBuyLandButton() 
    { 
        GameManager.Instance.UIManager.OpenLandUpGradeUIAsync().Forget(); 
        GameManager.Instance.UIManager.Close(UIType.BuildingPopUpUI); 
    }

    public void InitDescription()
    {
        Text_Description.text = $"{_building._buildingData.Name}\n\n인구수 + {_building._buildingData.CatCapacity}\n";
        Debug.Log($"1.{Text_Description.text}");
        if (_building._buildingData.SpCatId != null) 
        {
            GameManager.Instance.DataManager.TryGetData(_building._buildingData.SpCatId, out CatInfoData data);
            Text_Description.text += $"{data.Name} 등장확률 증가!";
            Debug.Log($"2.{Text_Description.text}");
        }
    }
}
