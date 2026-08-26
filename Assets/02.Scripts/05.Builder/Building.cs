
using System;
using System.Collections.Generic;

using UnityEngine;


public class Building : MonoBehaviour
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private MeshRenderer _mesh;


    [Header("Gizmo Settings")]
    [SerializeField] private bool _showGizmo = true;
    [SerializeField] private Color _gizmoBoxColor = new Color(1f, 0f, 0f, 0.3f);
    [SerializeField] private Color _gizmoWireColor = Color.red;
    [SerializeField] private Vector3 _gizmocentor;

    public BuildingData _buildingData;
    private Transform _entrancePoint;
    public string InstanceId { get; private set; }
    public string ModelAddress { get; private set; }
    // 현재 비어 있는 입주 자리
    //private Queue<Transform> _availableCatPoints = new Queue<Transform>();
    // 생성된 모든 입주 자리
    private List<Transform> _allCatPoints = new List<Transform>();

    private List<CatView> _movedInCatList = new();

    private readonly Queue<Transform> _availableCatPoints = new();

    public void InitaizeData(float rootX, BuildingData data, string modelAddress)
    {
        _buildingData = data;
        InstanceId = Guid.NewGuid().ToString();
        ModelAddress = modelAddress;
        
        OnBuildBuilding(rootX);
        SetBuildType(_buildingData.BuildingType);
    }
    private void OnBuildBuilding(float rootX)
    {
        Vector3 scale = new Vector3(_buildingData.Width, _buildingData.Height, 1f);
        CreatePoints(scale);
        _boxCollider.size = scale;

        GameManager.Instance.MapManager.RegisterBuilding(_buildingData, rootX, InstanceId, ModelAddress);
        GameManager.Instance.EconomyService_DH.AddCatCurrentCount(_buildingData.CatCapacity);

        if (_buildingData.SpCatId != null)
        {
            GameManager.Instance.CatManager.TryChangeSpawnWeight(_buildingData.SpCatId, _buildingData.SpCatValue);
        }
    }
    private void CreatePoints(Vector3 scale)
    {
        //CreateCatPoints(scale);
        CreateEntrancePoint(scale);
        InitializeCatSlots(this.gameObject);
    }
    public void OnRemoveBuilding()
    {

        GameManager.Instance.EconomyService_DH.RemoveCatCurrentCount(_buildingData.CatCapacity);
        if (_buildingData.SpCatId != null)
        {
            GameManager.Instance.CatManager.TryChangeSpawnWeight(_buildingData.SpCatId, -_buildingData.SpCatValue);
        }
    }

    /// <summary>
    /// 건물의 Scale을 기준으로 입주 가능한 위치를 생성한다.
    /// 예: Scale (3, 2, 1) → 3 x 2 = 6개의 자리s
    /// </summary>

    private void SetBuildType(int buildType)
    {
        switch ((BuildingType)buildType)
        {
            case BuildingType.Normal:

                break;
            case BuildingType.TownHall:

                break;
            case BuildingType.Spawner:
                this.gameObject.AddComponent<CatSpawner>();
                break;
            case BuildingType.LandMark:
                LandMarkBuilding landMarkBuilding = GetComponentInChildren<LandMarkBuilding>();
                landMarkBuilding.OnBuild();
                break;
            default:
                Debug.LogError("매칭되는 건물 타입이 존재하지 않습니다.");
                break;
        }
    }

    /// <summary>
    /// 입주 가능한 빈 자리를 하나 반환한다.
    /// </summary>
    public Transform GetAvailableCatPoint(CatView movedInCat)
    {
        if (_availableCatPoints.Count == 0)
        {
            return null;
        }

        _movedInCatList.Add(movedInCat);
        return _availableCatPoints.Dequeue();
    }

    /// <summary>
    /// 사용했던 자리를 다시 빈 자리로 돌려놓는다.
    /// </summary>
    public void ReturnCatPoint(Transform point, CatView movedOutCat)
    {
        if (point == null)
            return;

        if (!_allCatPoints.Contains(point))
            return;

        if (_availableCatPoints.Contains(point))
            return;

        _movedInCatList.Remove(movedOutCat);
        _availableCatPoints.Enqueue(point);
    }

    private void GetOutAllCat()
    {
        if (_entrancePoint != null)
        {
            Vector3 escapePoint = _entrancePoint.position;
            escapePoint.z = 0f;

            foreach (var outCat in _movedInCatList)
            {
                if (outCat == null)
                    continue;

                outCat.transform.position = escapePoint;
                outCat.EscapeDestroyBuilding();
            }
        }

        _movedInCatList.Clear();
        ResetAvailableCatPoints();
    }

    private void ResetAvailableCatPoints()
    {
        _availableCatPoints.Clear();

        foreach (Transform point in _allCatPoints)
        {
            if (point != null)
            {
                _availableCatPoints.Enqueue(point);
            }
        }
    }

    /// <summary>
    /// 현재 입주 가능한 자리의 개수
    /// </summary>
    public int GetAvailableCatPointCount()
    {
        return _availableCatPoints.Count;
    }

    /// <summary>
    /// 건물이 가지고 있는 전체 자리 개수
    /// </summary>
    public int GetTotalCatPointCount()
    {
        return _allCatPoints.Count;
    }

    private void ClearCatPoints()
    {
        foreach (Transform point in _allCatPoints)
        {
            if (point != null)
            {
                Destroy(point.gameObject);
            }
        }

        GetOutAllCat();

        _allCatPoints.Clear();
        _availableCatPoints.Clear();
    }

    private void OnDestroy()
    {
        ClearCatPoints();
    }
    private void CreateEntrancePoint(Vector3 scale)
    {
        if (_entrancePoint != null)
        {
            Destroy(_entrancePoint.gameObject);
        }

        GameObject pointObject = new GameObject("EntrancePoint");

        _entrancePoint = pointObject.transform;
        _entrancePoint.SetParent(transform, false);

        _entrancePoint.localPosition = new Vector3(0f, -scale.y / 2f, 0f);
    }
    public Transform GetEntrancePoint()
    {
        return _entrancePoint;
    }

    public void MoveBuilding(Vector3 movePosition)
    {
        GetOutAllCat();
        transform.position = movePosition;
        GameManager.Instance.MapManager.ModifyBuildingData(InstanceId, movePosition.x);
    }

    public float GetAvailableSpaceRate()
    {
        return (float)_availableCatPoints.Count / _allCatPoints.Count;
    }



    /// <summary>
    /// BuildingData의 Width와 Height(실제 충돌/배치 판정 크기)를 감지하여 빨간색 상자 기즈모를 그립니다.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!_showGizmo) return;
        // BuildingData가 없으면 실제 판정 수치를 알 수 없으므로 방어 처리
        if (_buildingData == null)
        {
            if (_boxCollider != null)
            {
                Gizmos.color = _gizmoWireColor;
                Gizmos.DrawWireCube(transform.TransformPoint(_boxCollider.center), _boxCollider.size);
            }
            return;
        }
        // 실질적인 충돌/배치 판정 크기 (Width, Height)
        Vector3 size = new Vector3(_buildingData.Width, _buildingData.Height, 1f);
        Vector3 center = new Vector3(transform.position.x, transform.position.y + (_buildingData.Height / 2) + _buildingData.GroundOffset, -15f);
        if (_boxCollider == null)
        {
            center = transform.TransformPoint(_boxCollider.center);
        }
        _gizmocentor = center;
        // 빨간색 반투명 상자 + 외곽 테두리 렌더링
        Gizmos.color = _gizmoBoxColor;
        Gizmos.DrawCube(_gizmocentor, size);

        Gizmos.color = _gizmoWireColor;
        Gizmos.DrawWireCube(_gizmocentor, size);
    }
    private void InitializeCatSlots(GameObject model)
    {
        _availableCatPoints.Clear();

        CatSlotRoot[] slots = model.GetComponentsInChildren<CatSlotRoot>(true);

        foreach (CatSlotRoot slot in slots)
        {
            _availableCatPoints.Enqueue(slot.transform);
            _allCatPoints.Add(slot.transform);
            Debug.Log($"{slot.name}");
        }
    }
}
