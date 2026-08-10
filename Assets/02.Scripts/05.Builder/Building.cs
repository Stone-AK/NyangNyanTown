using UnityEngine;

public class Building : MonoBehaviour
{
    //public BuildingData _buildingData;
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
        MapManager.Instance.AddToList(data, rootX);//추후에 빌딩 데이터가 생기면 매니저에서 등록
        transform.localScale = new Vector3(data.ScaleX, data.ScaleY, 1f);
        Debug.Log($"너비:{_buildingData.Width}루트x{rootX}");
       // _meshFilter.sharedMesh = _buildingData.Mesh; 건물 외형 초기화
    }
}
