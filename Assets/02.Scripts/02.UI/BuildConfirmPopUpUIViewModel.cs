using UnityEngine;

public class BuildConfirmPopUpUIViewModel : MonoBehaviour
{
    public BuildingData _data;
    public BuildConfirmPopUpUIViewModel(BuildingData data) 
    {
        _data = data;
        Cost = _data.Cost;
        Cat = data.CatCapacity;
        Name = data.Name;
        Type = (BuildingType)data.BuildingType;
        HasSpCatEffect = data.SpCatId != null;
        if (data.SpCatId != null)
        {
            GameManager.Instance.DataManager.TryGetData<CatInfoData>(data.SpCatId, out CatInfoData spCatData);
            SpCatName = spCatData.Name;
        }
    }
    public int Cost { get; private set; }
    public int Cat { get; private set; }
    public string Name { get; private set; }
    public BuildingType Type { get; private set; }   
    public bool HasSpCatEffect { get; private set; }
    public string SpCatName { get; private set; }
}
