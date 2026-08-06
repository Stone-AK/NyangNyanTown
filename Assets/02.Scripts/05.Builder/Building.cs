using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData _buildingData;
    private Renderer _renderer;
    [SerializeField] private MeshFilter _meshFilter;
    private void Start()
    {
       
    }

    public void InitaizeData(float rootX,Mesh mesh) 
    {
        _renderer = GetComponentInChildren<Renderer>();
        _buildingData.width = _renderer.bounds.size.x;
        _buildingData.rootX = rootX;
        BuildManager.Instance._currentBuildingList.Add( _buildingData );//추후에 빌딩 데이터가 생기면 매니저에서 등록
        Debug.Log($"너비:{_buildingData.width}루트x{_buildingData.rootX}");
        _meshFilter.sharedMesh = mesh;
    }
}
