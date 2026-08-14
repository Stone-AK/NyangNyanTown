using UnityEngine;
using UnityEngine.UI;

public class BuildingPopUpUIView : BaseUI
{
    [SerializeField] Button Button_Move;
    [SerializeField] Button Button_Destroy;
    [SerializeField] Button Button_Exit;
    [SerializeField] Button Button_CancelBg;

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
    }
    private void OnDisable()
    {
        Button_Move.onClick.RemoveListener(OnClickMoveButton);
        Button_Destroy.onClick.RemoveListener(OnClickDestroyButton);
        Button_Exit.onClick.RemoveListener(OnClickExitButton);
        Button_CancelBg.onClick.RemoveListener(OnClickExitButton);
    }
    public void Initialize(Building building) 
    {
        gameObject.SetActive(true);
        Debug.Log($"{building._buildingData.Name},{building.InstanceId}");
        _building = building;
    }
    private void OnClickMoveButton() { GameManager.Instance.BuildManager.MoveBuilding(_building); gameObject.SetActive(false); }
    private void OnClickDestroyButton() { GameManager.Instance.BuildManager.DestroyBuilding(_building); gameObject.SetActive(false); }
    private void OnClickExitButton() { gameObject.SetActive(false); }

}
public class BuildingPopUpUIViewModel : ViewModelBase
{





}