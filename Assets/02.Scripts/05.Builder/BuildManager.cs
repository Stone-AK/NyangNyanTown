using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public struct BuildingData
{
    public float Width;
    public float RootX;
    public int Id;
    public string Name;
    public int Cost;
    public Mesh Mesh;
}
public class BuildManager : MonoBehaviour
{
    [SerializeField] GameObject _previewBuildingPrefab;
    [SerializeField] GameObject _realBuildingPrefab;
    PreviewBuilding _previewBuilding;
 
    private GameObject _currentPreviewBuilding;
    private GameObject _currentBuilding;
    private BuildingData _currentPreviewBuildingData;
    public int TotalGold { get; set; } = 1000;//임시

    private Vector3 _worldPos;
    private bool _isBuilding = false;
    private float _currentGridX ;

    public event Action<int> OnTotalGoldChanged;
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

        if (_isBuilding && (_currentGridX != newGridX)) 
        {
            _currentGridX = newGridX;
            OnGridChanged();
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && _isBuilding)
        {
            if (EventSystem.current.IsPointerOverGameObject())//버튼 중복입력 방지
                return;
            if (MapManager.Instance.CanBuildOnThisPlace(_currentGridX, _currentPreviewBuildingData.Width))
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
    public void StartBuild(BuildingData data)//프리뷰 건물 생성후 초기화
    {
        Debug.Log("startbuildind 호출");
        if (_isBuilding) { return;}
        _isBuilding = true;
        _currentPreviewBuilding = Instantiate(_previewBuildingPrefab, new Vector3(_worldPos.x, 0, 0), Quaternion.identity);
        _previewBuilding= _currentPreviewBuilding.GetComponent<PreviewBuilding>();
        _previewBuilding.Initialize(data.Mesh);
        
        _currentPreviewBuildingData = data;
    }
    private void EndBuild() // 프리뷰 건물 삭제
    {
        _isBuilding = false;
        Destroy(_currentPreviewBuilding);
    }
    private void BuildBuilding(Vector3 buildPositon) //건물설치
    {
        if (!HasEnoughGold())
            return;
        EndBuild();
        _currentBuilding = Instantiate(_realBuildingPrefab, buildPositon, Quaternion.identity);
        Building currentBuilding = _currentBuilding.GetComponent<Building>();
        currentBuilding.InitaizeData(buildPositon.x, _currentPreviewBuildingData);
        AddGold(- (_currentPreviewBuildingData.Cost));
    }
    private void OnGridChanged() //프리뷰 건물을 옮길때 마다
    {
        if (_currentPreviewBuilding != null)
        {
          _currentPreviewBuilding.transform.position = new Vector3(_currentGridX, 0f, 0f);
        }
        bool canBuild = MapManager.Instance.CanBuildOnThisPlace(_currentGridX, _currentPreviewBuildingData.Width);
        _previewBuilding.SetBuildable(canBuild&& HasEnoughGold());
    }
    private void AddGold(int addedGold) 
    {
        TotalGold += addedGold;
        OnTotalGoldChanged?.Invoke(TotalGold);
    }
    private bool HasEnoughGold() 
    {
        return _currentPreviewBuildingData.Cost <= TotalGold; 
    }
}
