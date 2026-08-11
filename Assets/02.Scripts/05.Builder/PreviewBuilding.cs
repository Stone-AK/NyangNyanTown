using UnityEngine;

public class PreviewBuilding : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Material _canBuildMaterial;
    [SerializeField] private Material _cannotBuildMaterial;

    public BuildingData BuildingDataModel;
    public void SetBuildable(bool canBuild)
    {
        Material material = canBuild ? _canBuildMaterial : _cannotBuildMaterial;
        _renderer.material = material;
    }
    public void Initialize(BuildingData data)
    {
        BuildingDataModel=data;
        transform.localScale = new Vector3(data.ScaleX, data.ScaleY,1f);
    }
}
