using Cysharp.Threading.Tasks;

using System.Collections.Generic;
using UnityEngine;
public struct PlacedBuildingData
{
    public string InstanceId;
    public string BuildingID;
    public float RootX;
    public float Width;
}
public class MapManager : BaseManager<MapManager>
{
    private const float GRID_WIDTH = 0.1f;
    //public List<BuildingData> _currentBuildingList = new List<BuildingData>();
    //public List<PlacedBuildingData> _currentBuildingList = new List<PlacedBuildingData>();
    public Dictionary<string, PlacedBuildingData> _currentBuildingLDic = new Dictionary<string, PlacedBuildingData>();

    public override UniTask InitializeAsync()
    {
        return UniTask.CompletedTask;
    }
    public bool CanBuildOnThisPlace(float rootX, float width, string ignoreInstanceId = null) //좌표를 주면 해당위치에 설치할 수 있는지 반환
    {
        float leftX = rootX - (width / 2f);
        float rightX = rootX + (width / 2f);
        foreach (PlacedBuildingData data in _currentBuildingLDic.Values)
        {
            if (data.InstanceId == ignoreInstanceId)
            {
                continue;
            }

            float dataLeftX = data.RootX - (data.Width / 2f);
            float dataRightX = data.RootX + (data.Width / 2f);

            if (dataLeftX <= rightX && dataRightX >= leftX)
            {
                return false;
            }

        }
        return true;
    }
    public float GetGridX(float worldPosX) //좌표를 그리드형식으로 반환(그리드를 가운데가 아니라 왼쪽끝에 맞추게 할 수도 있음)
    {
        return Mathf.Round(worldPosX / GRID_WIDTH) * GRID_WIDTH;
    }
    public void RegisterBuilding(BuildingData data, float rootX,string instanceId)
    {
        if (_currentBuildingLDic.ContainsKey(instanceId))
        {
            Debug.LogError($"이미 존재하는 Building InstanceId입니다: {instanceId}");
            return ;
        }
        PlacedBuildingData placedBuildingData;
        placedBuildingData.BuildingID = data.Id;
        placedBuildingData.RootX = rootX;
        placedBuildingData.Width = data.Width;
        placedBuildingData.InstanceId = instanceId;
        _currentBuildingLDic.Add(instanceId,placedBuildingData);
    }
    public bool ModifyBuildingData(string instanceId, float rootX)
    {
        if (!_currentBuildingLDic.TryGetValue(instanceId, out PlacedBuildingData data))
        {
            Debug.LogWarning($"존재하지 않는 건물입니다. ID: {instanceId}");
            return false;
        }

        data.RootX = rootX;
        _currentBuildingLDic[instanceId] = data;

        return true;
    }
    public void DeleteBuilding(string instanceId) 
    {
        if (_currentBuildingLDic.TryGetValue(instanceId,out PlacedBuildingData data)) 
        {
            _currentBuildingLDic.Remove(instanceId);
        }
    
    }
}
