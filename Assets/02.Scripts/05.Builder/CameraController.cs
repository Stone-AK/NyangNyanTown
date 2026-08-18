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
    [Header("Move Range")]
    [SerializeField] private const float MOVE_MIN_Y = -1f;
    [SerializeField] private const float MOVE_MAX_Y = 3f;
    [SerializeField] private const float DEFAULT_MOVE_MIN_X = -20f;
    [SerializeField] private const float DEFAULT_MOVE_MAX_X = 20f;

    [Header("Zoom")]
    [SerializeField] private float _zoomSpeed = 20f;
    [SerializeField] private float _minZoom = 3f;
    [SerializeField] private float _maxZoom = 10f;

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

    private void Awake()
    {
        Instance = this;

        if (_camera == null)
            _camera = Camera.main;
       
    }

    private void Start()
    {
        UpdateMoveRange();
        UpdateVisibleRange();
    }

    private void Update()
    {
        if (!_isLandViewModelConnected)
        {
            var landVM = GameManager.Instance?.MapManager?._lvm;

            if (landVM != null)
            {
                landVM.OnLandLevelUp += AddCameraRange;
                _isLandViewModelConnected = true;
            }
        }
        HandleDrag();
        HandleZoom();
        TouchObject();
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
        
        position.x = Mathf.Clamp(position.x, MoveMinX + halfWidth,  MoveMaxX - halfWidth );

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
        float boundFactor = landLv * 10f;

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
}
