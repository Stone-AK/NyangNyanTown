using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public struct BuildingData 
{
   public float width;
   public float rootX;
}
public class BuildManager : MonoBehaviour
{
    [SerializeField] GameObject _previewBuildingPrefab;
    [SerializeField] GameObject _realBuildingPrefab;
    [SerializeField] List<Mesh> _meshList = new List<Mesh>();
    PreviewBuilding _previewBuilding;
    private int _meshIndex = 0;
    private GameObject _currentPreviewBuilding;
    private GameObject _currentBuilding;
    private BuildingData _currentPreviewBuildingData;


    private Vector3 _worldPos;
    private bool _isBuilding = false;
    private float _currentGridX ;
    
    public static BuildManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        _worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 0));
        
        float newGridX = MapManager.Instance.GetGridX(_worldPos.x);

        if (Keyboard.current.aKey.wasPressedThisFrame && !_isBuilding) 
        {
            StartBuild();
        }
        if (_isBuilding && (_currentGridX != newGridX)) 
        {
            _currentGridX = newGridX;
            OnGridChanged();
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && _isBuilding)
        {
            if (MapManager.Instance.CanBuildOnThisPlace(_currentGridX, _currentPreviewBuildingData.width))
            {
                BuildBuilding(new Vector3(_currentGridX, 0f, 0f));
            }
            else 
            {
                Debug.Log("건설 불가능");
            }
        }
        if (Mouse.current.rightButton.wasPressedThisFrame && _isBuilding)
        {
            EndBuild();
        }
        //if (Keyboard.current.aKey.wasPressedThisFrame && _isBuilding)
        //{

        //    EndBuild();

        //}
    
    }
    private void StartBuild()//프리뷰 건물 생성후 초기화
    {
        _isBuilding = true;
        _currentPreviewBuilding = Instantiate(_previewBuildingPrefab, new Vector3(_worldPos.x, 0, 0), Quaternion.identity);
        _previewBuilding= _currentPreviewBuilding.GetComponent<PreviewBuilding>();
        _previewBuilding.Initialize(_meshList[_meshIndex]);
        
        _currentPreviewBuildingData.width =1f;
    }
    private void EndBuild() // 프리뷰 건물 삭제
    {
        _isBuilding = false;
        Destroy(_currentPreviewBuilding);
    }
    private void BuildBuilding(Vector3 buildPositon) //건물설치
    {
        EndBuild();
        _currentBuilding = Instantiate(_realBuildingPrefab, buildPositon, Quaternion.identity);
        Building currentBuilding = _currentBuilding.GetComponent<Building>();
        currentBuilding.InitaizeData(buildPositon.x, _meshList[_meshIndex]);
        _meshIndex++;
        _meshIndex = _meshIndex % _meshList.Count;
    }
    private void OnGridChanged() //프리뷰 건물을 옮길때 마다
    {
        if (_currentPreviewBuilding != null)
        {
          _currentPreviewBuilding.transform.position = new Vector3(_currentGridX, 0f, 0f);
        }
        bool canBuild = MapManager.Instance.CanBuildOnThisPlace(_currentGridX, _currentPreviewBuildingData.width);
        _previewBuilding.SetBuildable(canBuild);
    }
}
