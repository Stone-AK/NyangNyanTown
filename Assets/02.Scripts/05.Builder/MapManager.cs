using System.Collections.Generic;
using UnityEngine;
public class MapManager : MonoBehaviour
{
    private const float GRID_WIDTH = 0.1f;
    public List<BuildingData> _currentBuildingList = new List<BuildingData>();
    
    public static MapManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public bool CanBuildOnThisPlace(float rootX, float width) //좌표를 주면 해당위치에 설치할 수 있는지 반환
    {
        float leftX = rootX - (width / 2f);
        float rightX = rootX + (width / 2f);
        foreach (BuildingData data in _currentBuildingList)
        {
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
}
