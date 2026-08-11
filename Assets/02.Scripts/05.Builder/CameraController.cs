using System;
using UnityEngine;
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
        HandleDrag();
        HandleZoom();
        Buildingtest();

    }
    private void Buildingtest() {
        if (Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"클릭한 오브젝트: {hit.collider.name}");

            Building building = hit.collider.GetComponentInParent<Building>();

            if (building != null)
            {
                Debug.Log($"건물 클릭 성공 ID: {building.InstanceId}");
                GameManager.Instance.BuildManager.DestroyBuilding(building);
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
        Vector2 bounds = new Vector2(-20f, 20f);

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
}
