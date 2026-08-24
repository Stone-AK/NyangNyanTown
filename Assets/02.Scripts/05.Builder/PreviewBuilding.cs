using UnityEngine;

public class PreviewBuilding : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Material _canBuildMaterial;
    [SerializeField] private Material _cannotBuildMaterial;
    [SerializeField] private BoxCollider _boxCollider;
    public BuildingData BuildingDataModel;
    private GameObject _model;
    private MeshRenderer _modelRenderer;
    public void SetBuildable(bool canBuild)
    {
        Material material = canBuild ? _canBuildMaterial : _cannotBuildMaterial;
        if (_modelRenderer != null)
        {
            _modelRenderer.material = material;
        }
    }
    public void Initialize(BuildingData data, GameObject model)
    {
        BuildingDataModel=data;
        _model=model;
        _modelRenderer = _model.GetComponentInChildren<MeshRenderer>();
        // transform.localScale = new Vector3(data.ScaleX, data.ScaleY,1f);
        _boxCollider.size = new Vector3(data.Width, data.Height,1f);
    }
}
