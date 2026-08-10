using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData _buildingData;
    private Renderer _renderer;
    [SerializeField] private MeshFilter _meshFilter;
    private void Start()
    {
       
    }

    public void InitaizeData(float rootX, BuildingData data) 
    {
        _renderer = GetComponentInChildren<Renderer>();
        _buildingData = data;
        _buildingData.RootX = rootX;
        MapManager.Instance._currentBuildingList.Add( _buildingData );//추후에 빌딩 데이터가 생기면 매니저에서 등록
        Debug.Log($"너비:{_buildingData.Width}루트x{_buildingData.RootX}");
        _meshFilter.sharedMesh = _buildingData.Mesh;
    }
}
