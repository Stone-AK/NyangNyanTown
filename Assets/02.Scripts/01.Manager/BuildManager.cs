using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum BuildMode
{
    None,
    Build,
    Move
}
public class BuildManager : BaseManager<BuildManager>
{
    [SerializeField] GameObject _previewBuildingPrefab;
    [SerializeField] GameObject _realBuildingPrefab;
    PreviewBuilding _previewBuilding;
 
    private GameObject _currentPreviewBuilding;
    private GameObject _currentBuildingObject;
    private Building _currentBuilding;
    private BuildingData _currentPreviewBuildingData;
    public int TotalGold { get; set; } = 1000;//임시
    private const float GRUOND_Y = 1.5f;//임시 보정

    private Vector3 _worldPos;
    private bool _isBuilding = false;
    private float _currentGridX ;

    private BuildMode _currentBuildMode = BuildMode.None;
    private string _currentBuildingInstaceId = null;

    public event Action<int> OnTotalGoldChanged;
    public override UniTask InitializeAsync()
    {
        return UniTask.CompletedTask;
    }
   
    private void Update()
    {
       

        if (!_isBuilding)
            return;
        UpdateMouseWorldPosition();
        UpdatePreview();
        HandleBuildInput();


    }
    public void StartBuild(BuildingData data, BuildMode mode)//프리뷰 건물 생성후 초기화
    {
        Debug.Log("startbuildind 호출");

        if (_isBuilding)
        {
            return;
        }

        _isBuilding = true;
        UpdateMouseWorldPosition();

        _currentPreviewBuildingData = data;
        _currentBuildMode = mode;
        _currentPreviewBuilding = Instantiate(_previewBuildingPrefab, new Vector3(_worldPos.x, (_currentPreviewBuildingData.ScaleY / 2f)- GRUOND_Y, 0), Quaternion.identity);
       
        Debug.Log($"프리뷰{data.ScaleY / 2f}");
        _previewBuilding= _currentPreviewBuilding.GetComponent<PreviewBuilding>();
        _previewBuilding.Initialize(data);
        
       
    }
    private void EndBuild() // 프리뷰 건물 삭제
    {
        _isBuilding = false;
        _currentBuildMode = BuildMode.None;
        _currentBuildingInstaceId = null;

        Destroy(_currentPreviewBuilding);
        _currentPreviewBuilding = null;
        _previewBuilding = null;
    }
    private void ConfirmBuilding(Vector3 buildPositon) //건물설치
    {
        if (!HasEnoughGold())
            return;
        if (_currentBuildMode == BuildMode.Build)
        {
            AddGold(-(_currentPreviewBuildingData.Cost));
            EndBuild();
            _currentBuildingObject = Instantiate(_realBuildingPrefab, buildPositon, Quaternion.identity);
            _currentBuilding = _currentBuildingObject.GetComponent<Building>();
            _currentBuilding.InitaizeData(buildPositon.x, _currentPreviewBuildingData);
        }
        else if (_currentBuildMode == BuildMode.Move) 
        {
            EndBuild();
            _currentBuilding.MoveBuilding(buildPositon);
        }
        _currentBuildingObject = null;
        _currentBuilding = null;
    }
    private void OnGridChanged() //프리뷰 건물을 옮길때 마다
    {
        if (_currentPreviewBuilding != null)
        {
          _currentPreviewBuilding.transform.position = new Vector3(_currentGridX, (_currentPreviewBuildingData.ScaleY / 2f) - GRUOND_Y, 0f);
        }
        bool canBuild = GameManager.Instance.MapManager.CanBuildOnThisPlace(_currentGridX, _currentPreviewBuildingData.Width, _currentBuildingInstaceId);
        _previewBuilding.SetBuildable(canBuild&& HasEnoughGold());
    }
    private void AddGold(int addedGold) 
    {
        TotalGold += addedGold;
        OnTotalGoldChanged?.Invoke(TotalGold);
    }
    private bool HasEnoughGold() 
    {
        if (_currentBuildMode == BuildMode.Move)
            return true;
        
        return _currentPreviewBuildingData.Cost <= TotalGold; 
    }
    public void DestroyBuilding(Building building) 
    {
        if (building == null)
            return;

        GameManager.Instance.MapManager.DeleteBuilding(building.InstanceId);
        Destroy(building.gameObject);
    }
    public void MoveBuilding(Building building) 
    {
        _currentBuildingInstaceId = building.InstanceId;
        _currentBuilding = building;
        StartBuild(building._buildingData,BuildMode.Move);
    }
    private void UpdateMouseWorldPosition() {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        _worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 0f));
    }
    private void UpdatePreview()
    {
        if (!_isBuilding)
            return;

        float newGridX = GameManager.Instance.MapManager.GetGridX(_worldPos.x);

        if (_currentGridX == newGridX)
        {
            return;
        }

        _currentGridX = newGridX;

        OnGridChanged();
    }
    private void HandleBuildInput()
    {
       
        if (Mouse.current.leftButton.wasPressedThisFrame)
        { 
            if (EventSystem.current.IsPointerOverGameObject())//버튼 중복입력 방지
            return;

            if (GameManager.Instance.MapManager.CanBuildOnThisPlace(_currentGridX, _currentPreviewBuildingData.Width, _currentBuildingInstaceId))
            {
                ConfirmBuilding(new Vector3(_currentGridX, (_currentPreviewBuildingData.ScaleY / 2f) - GRUOND_Y, 0f));
            }
            else
            {
                Debug.Log("건설 불가능");
            }
            return;
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            EndBuild();
        }
    }
}
