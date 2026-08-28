using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Move")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _focusMoveDuration = 0.35f;
    [SerializeField] private Vector2 _focusTargetScreenOffset = new Vector2(-150f, 0f);
    [Header("Move Range")]
    [SerializeField] private const float MOVE_MIN_Y = -3f;
    [SerializeField] private const float MOVE_MAX_Y = 10f;
    [SerializeField] private const float DEFAULT_MOVE_MIN_X = -60f;
    [SerializeField] private const float DEFAULT_MOVE_MAX_X = 60f;

    [Header("Zoom")]
    [SerializeField] private float _zoomSpeed = 20f;
    [SerializeField] private float _minZoom = 10f;
    [SerializeField] private float _maxZoom = 40f;

    [Header("Layer Cull Settings")]
    [SerializeField] private string _outsideLayerName = "Outside";
    [SerializeField] private float _outsideLayerCullDistance = 15f;
    private int _outsideLayerIndex = -1;


    // 현재 카메라가 실제로 비추고 있는 X 범위
    public float VisibleMinX { get; private set; } 
    public float VisibleMaxX { get; private set; }

    // 카메라가 이동할 수 있는 월드 범위
    public float MoveMinX { get; private set; }
    public float MoveMaxX { get; private set; }
    public float MoveMinY { get; private set; }
    public float MoveMaxY { get; private set; }
    public float CurrentZoom => _camera.orthographicSize;
    private bool _isLandViewModelConnected;
    // 시야가 변경되었을 때 알림
    public event Action<float, float> OnVisibleRangeChanged;

    private bool _isDragging;
    private Vector2 _lastMousePosition;

    private Transform _focusTarget;
    private Vector3 _focusStartPosition;
    private float _focusMoveElapsed;

    private void Awake()
    {
        Instance = this;

        if (_camera == null)
            _camera = Camera.main;

        _outsideLayerIndex = LayerMask.NameToLayer(_outsideLayerName);
        if (_outsideLayerIndex == -1)
        {
            Debug.LogWarning($"[CameraController] '{_outsideLayerName}' 레이어가 프로젝트에 등록되어 있지 않습니다.");
        }
    }

    private void Start()
    {
        UpdateMoveRange();
        UpdateVisibleRange();
        UpdateOutsideLayerVisibility(_camera.orthographicSize);
    }

    private void Update()
    {
        if (!_isLandViewModelConnected)
        {
            var landVM = GameManager.Instance?.MapManager?._lvm;

            if (landVM != null)
            {
                landVM.OnLandLevelUp += AddCameraRange;
                AddCameraRange(landVM.LandLevel);
                _isLandViewModelConnected = true;
            }
        }
        HandleDrag();
        HandleZoom();
        
        FollowFocusTargetOnUpdate();
        if (GameManager.Instance.BuildManager.IsBuilding)
        {
            HandleBuilding();
        }
        else 
        {
            TouchObject();
        }
    }
    private void TouchObject() {
        if (Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"클릭한 오브젝트: {hit.collider.name}");

            CatView catView = hit.collider.GetComponent<CatView>();

            if (catView != null)
            {
                Debug.Log("고양이 클릭");
                GameManager.Instance.UIManager.OpenCatInfoPopupAsync(catView).Forget();
                if(GameManager.Instance.EconomyService_DH.CheckClickCatIsNew(catView.CatViewModelProp.CatId))
                {
                    Debug.Log("새로운 고양이 습득");
                }
                else
                {
                    Debug.Log("이미 습득한 고양이");
                }
                return;
            }

            Building building = hit.collider.GetComponentInParent<Building>();

            if (building != null)
            {
                Debug.Log($"건물 클릭 성공 ID: {building.InstanceId}");
                GameManager.Instance.UIManager.OpenBuildingPopupAsync(building) .Forget();
            }
        }
    }

    private void HandleZoom()
    {
        if (Mouse.current == null)
            return;

        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Approximately(scroll, 0f))
            return;

        _camera.orthographicSize -= scroll * _zoomSpeed * 0.01f;

        _camera.orthographicSize = Mathf.Clamp( _camera.orthographicSize, _minZoom, _maxZoom );

        // 줌을 하면 화면에 보이는 X 범위가 변한다.
        // 카메라 위치도 다시 보정
        ClampCameraPosition();

        UpdateVisibleRange();
        UpdateOutsideLayerVisibility(_camera.orthographicSize);
    }

    private void UpdateOutsideLayerVisibility(float currentZoom)
    {
        if (_camera == null)
        {
            Debug.LogError("[CameraController] Camera 참조가 없어 레이어 컬링을 갱신할 수 없습니다.");
            return;
        }

        if (_outsideLayerIndex == -1)
            return;

        // 줌 거리(orthographicSize)가 기준값 이하일 때 레이어 끄기, 초과일 때 켜기
        if (currentZoom <= _outsideLayerCullDistance)
        {
            _camera.cullingMask &= ~(1 << _outsideLayerIndex);
        }
        else
        {
            _camera.cullingMask |= (1 << _outsideLayerIndex);
        }
    }

    private void UpdateMoveRange()
    {
        //  GameManager.Instance.MapManager.GetCameraBounds();
        Vector2 bounds = new Vector2(DEFAULT_MOVE_MIN_X, DEFAULT_MOVE_MAX_X);

        MoveMinX = bounds.x;
        MoveMaxX = bounds.y;

        MoveMinY = MOVE_MIN_Y;
        MoveMaxY = MOVE_MAX_Y;
    }


    private void UpdateVisibleRange()
    {
        float halfWidth = GetHalfCameraWidth();

        VisibleMinX = _camera.transform.position.x - halfWidth;
        VisibleMaxX = _camera.transform.position.x + halfWidth;

        OnVisibleRangeChanged?.Invoke(VisibleMinX, VisibleMaxX);
    }

    private float GetHalfCameraWidth()
    {
        return _camera.orthographicSize * _camera.aspect;
    }
    private float GetHalfCameraHeight()
    {
        return _camera.orthographicSize;
    }
    private void ClampCameraPosition()
    {
        Vector3 position = _camera.transform.position;

        float halfWidth = GetHalfCameraWidth(); 
        float halfHeight = GetHalfCameraHeight();

        float minX = MoveMinX + halfWidth;
        float minY = MoveMinY - halfHeight;

        if (minX > minY) // 카메라가 이동할 수 있는 범위가 화면보다 좁을 때, 카메라를 중앙에 위치시킴
        {
            position.x = (MoveMinX + MoveMaxX) * 0.5f;
        }
        else
        {
            position.x = Mathf.Clamp(position.x, MoveMinX + halfWidth, MoveMaxX - halfWidth);
        }

        position.y = Mathf.Clamp( position.y,MoveMinY, MoveMaxY );

        _camera.transform.position = position;
    }

    public bool IsInVisibleRange(float minX, float maxX)
    {
        return maxX >= VisibleMinX && minX <= VisibleMaxX;
    }
    private void HandleDrag()
    {
        if (Mouse.current == null)
            return;

        Vector2 currentMousePosition =
            Mouse.current.position.ReadValue();

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            _isDragging = true;
            _lastMousePosition = currentMousePosition;
        }

        if (Mouse.current.rightButton.isPressed && _isDragging)
        {
            Vector2 mouseDelta =  currentMousePosition - _lastMousePosition;

            float worldPerPixel =  (_camera.orthographicSize * 2f) / Screen.height;

            Vector3 cameraDelta = new Vector3( -mouseDelta.x * worldPerPixel, -mouseDelta.y * worldPerPixel, 0f);

            _camera.transform.position += cameraDelta;

            ClampCameraPosition();
            UpdateVisibleRange();

            _lastMousePosition = currentMousePosition;
        }

        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            _isDragging = false;
        }
    }
    private void AddCameraRange(int landLv) 
    {
        float boundFactor = landLv * 25f;

        Vector2 bounds = new Vector2(DEFAULT_MOVE_MIN_X - boundFactor, DEFAULT_MOVE_MAX_X + boundFactor);

        MoveMinX = bounds.x;
        MoveMaxX = bounds.y;
    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.MapManager._lvm != null)
        {
            GameManager.Instance.MapManager._lvm.OnLandLevelUp -= AddCameraRange;
        }
    }

    private void FollowFocusTargetOnUpdate()
    {
        if (_focusTarget == null)
            return;

        float worldPerPixel = (_camera.orthographicSize * 2f) / Screen.height;

        Vector3 targetPosition = new Vector3(
            _focusTarget.position.x
                - (_focusTargetScreenOffset.x * worldPerPixel),
            _focusTarget.position.y
                - (_focusTargetScreenOffset.y * worldPerPixel),
            _camera.transform.position.z);

        if (_focusMoveElapsed < _focusMoveDuration)
        {
            _focusMoveElapsed += Time.deltaTime;

            float ratio = Mathf.Clamp01(_focusMoveElapsed / _focusMoveDuration);

            ratio = Mathf.SmoothStep(0f, 1f, ratio);

            _camera.transform.position = Vector3.Lerp(_focusStartPosition, targetPosition, ratio);
        }
        else
        {
            _camera.transform.position = targetPosition;
        }

        UpdateVisibleRange();
    }

    public void SetFollowingTarget(Transform target)
    {
        _focusTarget = target;
        _focusStartPosition = _camera.transform.position;
        _focusMoveElapsed = 0f;
    }

    public void UnassignedFollowingTarget()
    {
        _focusTarget = null;
    }
    private void HandleBuilding() 
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current.IsPointerOverGameObject())//버튼 중복입력 방지
                return;
            GameManager.Instance.BuildManager.PressLeftMouseButtonToConfirmBuild();
         
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            GameManager.Instance.BuildManager.PressRightMouseButtonCancelBuild();
        }
    }
}
