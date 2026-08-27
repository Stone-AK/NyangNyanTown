using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class BuildUIViewModel : ViewModelBase
{
    public List<BuildingSlotItemViewModel> _itemSlots { get; } = new List<BuildingSlotItemViewModel>();
    public BuildUIViewModel()
    {
        if (GameManager.Instance.DataManager.TryGetDataTable<BuildingData>(out var dataTable))
        {
            foreach (var data in dataTable)
            {
                var newSlot = new BuildingSlotItemViewModel();

                newSlot.Initialize(data.Value);
                _itemSlots.Add(newSlot);
                newSlot.OnBuildingSlotButtonClicked += OpenBuildConfirmPopUp;
            }
        }
    }
    public void OpenBuildConfirmPopUp(BuildingData data) 
    {
        
        GameManager.Instance.UIManager.OpenBuildConfirmPopUpUIAsync(data).Forget();
    }
}