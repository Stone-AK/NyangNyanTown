using UnityEngine;

public class PreviewBuilding : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Material _canBuildMaterial;
    [SerializeField] private Material _cannotBuildMaterial;
    public void SetBuildable(bool canBuild)
    {
        Material material = canBuild ? _canBuildMaterial : _cannotBuildMaterial;
        _renderer.material = material;
    }
    public void Initialize(Mesh mesh)
    {
        _meshFilter.sharedMesh = mesh;
    }
}
