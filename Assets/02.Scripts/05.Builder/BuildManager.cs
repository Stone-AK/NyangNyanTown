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
    private const float GRID_WIDTH = 0.1f; 
    [SerializeField] GameObject _previewBuildingPrefab;
    [SerializeField] GameObject _realBuildingPrefab;
    [SerializeField] List<Mesh> _meshList = new List<Mesh>();
    PreviewBuilding _previewBuilding;
    private int _meshIndex = 0;
    public List<BuildingData> _currentBuildingList = new List<BuildingData>();
    private GameObject _currentPreviewBuilding;
    private GameObject _currentBuilding;
    private Vector3 _worldPos;
    private bool _isBuilding = false;
    private float _currentGridX ;
    private BuildingData _currentPreviewBuildingData;
    public static BuildManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        _worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreen.x, mouseScreen.y, 0));

        _worldPos.z = 0;
        float newGridX = GetGridX(_worldPos.x);

        if (Keyboard.current.aKey.wasPressedThisFrame && !_isBuilding) 
        {
            
            StartBuild();

        }
        if (_isBuilding && (_currentGridX != newGridX)) 
        {
            if (CanBuildOnThisPlace(newGridX))
            {
                _previewBuilding.SetBuildable(true);
            }
            else
            {
                _previewBuilding.SetBuildable(false);
            }
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && _isBuilding)
        {
            if (CanBuildOnThisPlace(newGridX))
            {
                BuildBuilding(new Vector3(newGridX, 0f, 0f));
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
        if (_currentPreviewBuilding != null)
        {
            if (_currentGridX != newGridX)
            {
                _currentGridX = newGridX;
                _currentPreviewBuilding.transform.position = new Vector3(newGridX, 0f, 0f);
            }
        }
    }
    private void StartBuild()
    {
        _isBuilding = true;
        _currentPreviewBuilding = Instantiate(_previewBuildingPrefab, new Vector3(_worldPos.x, 0, 0), Quaternion.identity);
        _previewBuilding= _currentPreviewBuilding.GetComponent<PreviewBuilding>();
        _previewBuilding.Initialize(_meshList[_meshIndex]);
        
        _currentPreviewBuildingData.width =1f;
    }
    private void EndBuild() 
    {
        _isBuilding = false;
        Destroy(_currentPreviewBuilding);
    }
    private void BuildBuilding(Vector3 buildPositon)
    {
        EndBuild();
        _currentBuilding = Instantiate(_realBuildingPrefab, buildPositon, Quaternion.identity);
        Building currentBuilding = _currentBuilding.GetComponent<Building>();
        currentBuilding.InitaizeData(buildPositon.x, _meshList[_meshIndex]);
        _meshIndex++;
        _meshIndex = _meshIndex % _meshList.Count;
    }
    private bool CanBuildOnThisPlace(float rootX) 
    {
        float leftX = rootX - (_currentPreviewBuildingData.width / 2f);
        float rightX = rootX + (_currentPreviewBuildingData.width / 2f);
        foreach (BuildingData data in _currentBuildingList) 
        {
            float dataLeftX = data.rootX - (data.width / 2f);
            float dataRightX = data.rootX + (data.width / 2f);
            if (dataLeftX<=rightX && dataRightX >= leftX) 
            {
                return false;
            }
        
        }
        return true;
    }
    private float GetGridX(float worldPosX) //그리드를 가운데가 아니라 왼쪽끝에 맞추게 할 수도 있음
    {
        return Mathf.Round(worldPosX / GRID_WIDTH) * GRID_WIDTH;
    }
}
