using Cysharp.Threading.Tasks;
using UnityEngine;
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
    private PreviewBuilding _previewBuilding;
    private Building _currentBuilding;              //건설에 필요한 프리팹과 빌딩오브젝트

    private GameObject _currentPreviewBuilding;
    private GameObject _currentBuildingObject;
    private BuildingData _currentPreviewBuildingData; 
    private string _currentBuildingInstaceId = null;// 현재 건설중인 건물과 데이터

    private GameObject _modelPrefab;
    private string _modelAddress;                     //건물 모델에 필요한 요소들    

    private Vector3 _worldPos;    
    private float _currentGridX;                    //화면에 표시되는 좌표
    private const float GRUOND_Y = 1.5f;//임시 보정
    private const float BUILDING_Z = 10f;

    public bool IsBuilding { get; private set; } = false;
    private BuildMode _currentBuildMode = BuildMode.None;//건설 정보  
  
    public override UniTask InitializeAsync()
    {
        return UniTask.CompletedTask;
    }
   
    private void Update()
    {
        if (!IsBuilding)
            return;
        UpdateMouseWorldPosition();
        UpdatePreview();
        //HandleBuildInput();
    }
    public async UniTask StartBuild(BuildingData data, BuildMode mode)//프리뷰 건물 생성후 초기화
    {
        Debug.Log("StartBuild 호출");

        if (IsBuilding)
        {
            return;
        }

        IsBuilding = true;

        UpdateMouseWorldPosition();

        _currentPreviewBuildingData = data;
        _currentBuildMode = mode;

        Vector3 buildPosition = new Vector3(_worldPos.x, (data.Height / 2f) - GRUOND_Y, 0f );

        // 1. 프리뷰 Root 생성
        _currentPreviewBuilding = Instantiate(_previewBuildingPrefab,buildPosition, Quaternion.identity);

        _previewBuilding = _currentPreviewBuilding.GetComponent<PreviewBuilding>();
        if (_currentBuildMode != BuildMode.Move)
        {
            SelectRandomModelAddress();
        }
        await LoadBuildingModel();
        CreatePreviewModel();
    }
    private async UniTask LoadBuildingModel()
    {
        // 3. 외형 Prefab 로드
        _modelPrefab = await GameManager.Instance.ResourceManager.LoadAssetAsync<GameObject>(_modelAddress);

        if (_modelPrefab == null)
        {
            Debug.LogError($"건물 외형 로드 실패: {_modelAddress}");

            EndBuild();
            return;
        }
    }
    private void SelectRandomModelAddress() 
    {
        int randomIndex = UnityEngine.Random.Range(0, _currentPreviewBuildingData.ModelAddresses.Length);

        _modelAddress = _currentPreviewBuildingData.ModelAddresses[randomIndex];
    }
    private void CreatePreviewModel()
    {
        GameObject model = Instantiate(_modelPrefab, _previewBuilding.transform );
        model.transform.localPosition = new Vector3(0f,-(_currentPreviewBuildingData.Height/2) , 0f);
        _previewBuilding.Initialize(_currentPreviewBuildingData, model);
    }
    private void CreateRealModel()
    {
        GameObject model = Instantiate(_modelPrefab, _currentBuilding.transform);
        model.transform.localPosition = new Vector3(0f, -(_currentPreviewBuildingData.Height / 2) , 0f);
    }
    private void EndBuild() // 프리뷰 건물 삭제
    {
        IsBuilding = false;
        _currentBuildMode = BuildMode.None;
        _currentBuildingInstaceId = null;

        GameManager.Instance.ResourceManager.ReleaseAsset(_modelAddress);
        Destroy(_currentPreviewBuilding);
        _currentPreviewBuilding = null;
        _previewBuilding = null;
    }
    private async UniTask ConfirmBuilding(Vector3 buildPositon) //건물설치
    {
        if (!HasEnoughGold())
            return;
        if (_currentBuildMode == BuildMode.Build)
        {
            GameManager.Instance.EconomyService_DH.RemoveCurrentGold((_currentPreviewBuildingData.Cost)); 
            //AddGold(-(_currentPreviewBuildingData.Cost));
            EndBuild();
            _currentBuildingObject = Instantiate(_realBuildingPrefab, buildPositon, Quaternion.identity);
            _currentBuilding = _currentBuildingObject.GetComponent<Building>();
            await LoadBuildingModel();
            CreateRealModel();
            _currentBuilding.InitaizeData(buildPositon.x, _currentPreviewBuildingData,_modelAddress);
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
          _currentPreviewBuilding.transform.position = new Vector3(_currentGridX, (_currentPreviewBuildingData.Height / 2f) - GRUOND_Y, 0f);
        }
        bool canBuild = GameManager.Instance.MapManager.CanBuildOnThisPlace(_currentGridX, _currentPreviewBuildingData.Width, _currentBuildingInstaceId);
        _previewBuilding.SetBuildable(canBuild&& HasEnoughGold());
    }
    private bool HasEnoughGold() 
    {
        if (_currentBuildMode == BuildMode.Move)
            return true;
        
        var vm = GameManager.Instance.EconomyService_DH.GetEconomyViewModel();

        return _currentPreviewBuildingData.Cost <= vm.CurrentGold; 
    }
    public void DestroyBuilding(Building building)
    {
        if (building == null) { 
        return;
        }

        _modelAddress = building.ModelAddress;
        building.OnRemoveBuilding();
        
        GameManager.Instance.MapManager.DeleteBuilding(building.InstanceId);
        GameManager.Instance.ResourceManager.ReleaseAsset(_modelAddress);
        Destroy(building.gameObject);
    }
    public void MoveBuilding(Building building) 
    {
        _currentBuildingInstaceId = building.InstanceId;
        _currentBuilding = building;
        _modelAddress = building.ModelAddress;
        StartBuild(building._buildingData,BuildMode.Move).Forget();
    }
    private void UpdateMouseWorldPosition() {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        _worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 0f));
    }
    private void UpdatePreview()
    {
        if (!IsBuilding)
            return;

        float newGridX = GameManager.Instance.MapManager.GetGridX(_worldPos.x);

        if (_currentGridX == newGridX)
        {
            return;
        }

        _currentGridX = newGridX;

        OnGridChanged();
    }
    //private void HandleBuildInput()
    //{

    //    if (Mouse.current.leftButton.wasPressedThisFrame)
    //    {
    //        if (EventSystem.current.IsPointerOverGameObject())//버튼 중복입력 방지
    //            return;

    //        if (GameManager.Instance.MapManager.CanBuildOnThisPlace(_currentGridX, _currentPreviewBuildingData.Width, _currentBuildingInstaceId))
    //        {
    //            ConfirmBuilding(new Vector3(_currentGridX, (_currentPreviewBuildingData.ScaleY / 2f) - GRUOND_Y, 0f));
    //        }
    //        else
    //        {
    //            Debug.Log("건설 불가능");
    //        }
    //        return;
    //    }

    //    if (Mouse.current.rightButton.wasPressedThisFrame)
    //    {
    //        EndBuild();
    //    }
    //}
    public void PressLeftMouseButtonToConfirmBuild() 
    {
        if (GameManager.Instance.MapManager.CanBuildOnThisPlace(_currentGridX, _currentPreviewBuildingData.Width, _currentBuildingInstaceId))
        {
            ConfirmBuilding(new Vector3(_currentGridX, (_currentPreviewBuildingData.Height / 2f) - GRUOND_Y, BUILDING_Z)).Forget();
        }
        else
        {
            Debug.Log("건설 불가능");
        }
    }
    public void PressRightMouseButtonCancelBuild() 
    {
        EndBuild();
    }
}
