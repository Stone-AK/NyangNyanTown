using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Building : MonoBehaviour
{
    //public BuildingData _buildingData;
    //private Renderer _renderer;

    public BuildingData _buildingData;   
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private Transform _visual;
    private Transform _entrancePoint;
    // 현재 비어 있는 입주 자리
    private Queue<Transform> _availableCatPoints = new Queue<Transform>();
    // 생성된 모든 입주 자리
    private List<Transform> _allCatPoints = new List<Transform>();

    private const float CELL_SIZE = 1f;
    private void Start()
    {
       
    }

    public void InitaizeData(float rootX, BuildingData data) 
    {
       // _renderer = GetComponentInChildren<Renderer>();
        _buildingData = data;
        GameManager.Instance.MapManager.AddToList(data, rootX);//추후에 빌딩 데이터가 생기면 매니저에서 등록
        Vector3 scale = new Vector3(data.ScaleX, data.ScaleY, 1f);
        _visual.localScale = scale;
        CreateCatPoints(scale);
        CreateEntrancePoint(scale);
        // 건물 타입 지정(제대로 작성 시 주석 제거)
        SetBuildType(_buildingData.BuildingType);
        //Debug.Log($"너비:{_buildingData.Width}루트x{rootX}");
        // _meshFilter.sharedMesh = _buildingData.Mesh; 건물 외형 초기화
    }
   

    /// <summary>
    /// 건물의 Scale을 기준으로 입주 가능한 위치를 생성한다.
    /// 예: Scale (3, 2, 1) → 3 x 2 = 6개의 자리
    /// </summary>

    private void SetBuildType(int buildType)
    {
        switch((BuildingType)buildType)
        {
            case BuildingType.Normal:

                break;
            case BuildingType.TownHall:

                break;
            case BuildingType.Spawner:
                this.gameObject.AddComponent<CatSpawner>();
                break;
            default:
                Debug.LogError("매칭되는 건물 타입이 존재하지 않습니다.");
                break;
        }
    }

    private void CreateCatPoints(Vector3 scale)
    {
        ClearCatPoints();

        int width = Mathf.RoundToInt(scale.x);
        int height = Mathf.RoundToInt(scale.y);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject pointObject = new GameObject(
                    $"CatPoint_{x}_{y}"
                );

                Transform point = pointObject.transform;

                point.SetParent(transform);

                // 건물 중앙을 기준으로 배치
                point.localPosition = new Vector3(
                    (x + 0.5f) * CELL_SIZE - (width * CELL_SIZE / 2f),
                    (y + 0.5f) * CELL_SIZE - (height * CELL_SIZE / 2f),
                    0f
                );

                _allCatPoints.Add(point);
                _availableCatPoints.Enqueue(point);
            }
        }
    }

    /// <summary>
    /// 입주 가능한 빈 자리를 하나 반환한다.
    /// </summary>
    public Transform GetAvailableCatPoint()
    {
        if (_availableCatPoints.Count == 0)
        {
            return null;
        }

        return _availableCatPoints.Dequeue();
    }

    /// <summary>
    /// 사용했던 자리를 다시 빈 자리로 돌려놓는다.
    /// </summary>
    public void ReturnCatPoint(Transform point)
    {
        if (point == null)
            return;

        if (!_allCatPoints.Contains(point))
            return;

        if (_availableCatPoints.Contains(point))
            return;

        _availableCatPoints.Enqueue(point);
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

        _entrancePoint.localPosition = new Vector3(0f,-scale.y / 2f, 0f);
    }
    public Transform GetEntrancePoint()
    {
        return _entrancePoint;
    }

    public float GetAvailableSpaceRate()
    {
        if (_allCatPoints.Count == 0)
            return 0f;

        return (float)_availableCatPoints.Count / _allCatPoints.Count;
    }
}
