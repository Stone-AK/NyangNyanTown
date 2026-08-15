using Cysharp.Threading.Tasks;
using System;
using System.ComponentModel;
using UnityEngine;

public class CatView : MonoBehaviour
{
    private CatViewModel _catViewModel;
    private GameObject _targetObject;
    private Vector3 _targetTransform;

    private void FixedUpdate()
    {
        if(_targetObject == null)
        {
            SearchDespawnPoint();
        }

        if (_catViewModel.CatState == CatState.MoveToTarget)
        {
            MoveCatOnFixedUpdate();
            CheckCatArriveTarget();
        }
    }

    private void BindSlotViewMdoel(CatViewModel catVM)
    {
        if (catVM == null)
            return;
        catVM.PropertyChanged += OnPropChagned_View;
    }

    private void OnPropChagned_View(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(CatViewModel.CatState):
                {
                    ActionFromStatus();
                }
                break;
        }
    }

    private void MoveCatOnFixedUpdate()
    {
        if(_catViewModel == null)
            return;

        transform.Translate(Vector3.forward * _catViewModel.CatSpeed * Time.deltaTime);
    }

    public void InitCatView(CatViewModel catViewModel)
    {
        _catViewModel = catViewModel;
        BindSlotViewMdoel(_catViewModel);
        CatDetectTarget();
    }

    private void CatDetectTarget()
    {
        // TODO(안우재/08.13) : MapManager의 _currentBuildingList<PlacedBuildingData>에서 Building과 Spawner검사해서
        // 건물 갈때와 철수 할때를 구분해서 해당 위치로 가도록 설정 필요
        if (GameManager.Instance.MapManager._currentBuildingLDic == null)
            return;

        if (GameManager.Instance.MapManager._currentBuildingLDic.Count == 0)
        {
            Debug.Log("지어진 건물이 없습니다.");
            // 추후 건물이 없어도 고양이가 생성되어야 한다면 해당 위치에 정의 필요
            return;
        }

        _targetObject = SearchTargetBuildingObject();

        // 아무 알맞은 건물을 못찾은 상태, 0으로 가도록 함
        if (_targetObject == null)
        {
            Vector3 temporaryDirection = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.LookRotation(temporaryDirection);
            return;
        }

        SettingTargetPosition();
    }

    private GameObject SearchTargetBuildingObject()
    {
        Building candidateTargetBuilding = null;
        float nowSpaceOccupancyRate = 0f;
        float newSpaceOccupancyRate = 0f;
        foreach (var placeBuildingData in GameManager.Instance.MapManager._currentBuildingLDic)
        {
            Vector3 searchTargetPosition = new Vector3(placeBuildingData.Value.RootX, 0, 0);
            Collider[] buildingChildCollider = Physics.OverlapSphere(searchTargetPosition, 0.01f);

            foreach (var buildingChild in buildingChildCollider)
            {
                if (buildingChild == null) 
                    continue;

                Building building = buildingChild.GetComponentInParent<Building>();
                if (building == null)
                    continue;

                if (building.GetComponent<CatSpawner>() != null)
                    continue;

                newSpaceOccupancyRate = building.GetAvailableSpaceRate();

                if (nowSpaceOccupancyRate < newSpaceOccupancyRate)
                {
                    nowSpaceOccupancyRate = newSpaceOccupancyRate;
                    candidateTargetBuilding = building;
                }
            }
        }
        if (candidateTargetBuilding == null)
            return null;

        return candidateTargetBuilding.gameObject;
    }

    private void SettingTargetPosition()
    {
        if (_targetObject == null)
        {
            Debug.Log("목표 오브젝트를 찾을 수 없습니다.");
            return;
        }

        if (_targetObject.TryGetComponent<Building>(out Building buildingObject))
        {
            _targetTransform = buildingObject.GetEntrancePoint().transform.position;
            _targetTransform.y = 0f;
            _targetTransform.z = 0f;
        }

        Vector3 direction = (_targetObject.transform.position - transform.position).normalized;
        direction.y = 0f;
        direction.z = 0f;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void CheckCatArriveTarget()
    {
        if (_targetObject == null)
            return;

        float remainingX = _targetTransform.x - transform.position.x;

        if (remainingX * transform.forward.x <= 0f)
        {
            if(CheckTargetExistenceORChanged())
            {
                // 목표지점이 옮겨지거나 철거된 상태
                _catViewModel.CatState = CatState.TargetMissing;
                return;
            }

            if (_targetObject.GetComponent<CatSpawner>())
            {
                ArriveSpanwPositionAndEscape();
            }
            else if (_targetObject.TryGetComponent<Building>(out Building buildingObject))
            {
                ArriveBuildingEntrance(buildingObject);
            }

            return;
        }
    }

    private bool CheckTargetExistenceORChanged()
    {
        if(_targetObject == null || !_targetObject.activeInHierarchy)
            return true;

        if (!Mathf.Approximately(_targetObject.transform.position.x, _targetTransform.x))
        {
            return true;
        }

        return false;
    }

    private void ArriveBuildingEntrance(Building building)
    {
        if(building == null)
            return;

        if(building.GetAvailableCatPointCount() == 0)
        {
            Debug.Log("해당 건물은 빈 자리가 없습니다.");
            _catViewModel.CatState = CatState.TargetMissing;
            // 대기 모션 출력 관련 메서드
            return;
        }

        this.gameObject.transform.position = building.GetAvailableCatPoint().position;
        _catViewModel.CatState = CatState.InBuildingAction;
    }

    private void ArriveSpanwPositionAndEscape()
    {
        // TODO(안우재/08.09) : 가까운 Spawn지역에 도착 시 탈출하는 모션 출력 필요.
        GameManager.Instance.CatManager.DespawnCat(this.gameObject);
    }

    private async void ActionFromStatus()
    {
        try
        {
            if (_catViewModel == null)
                return;

            if (_catViewModel.CatState == CatState.InBuildingAction)
            {
                // TODO(안우재/08.09) : slot에 이동 후 애니메이션 출력 구현 필요. 
                // 현재는 4초 가만히로 설정
                await UniTask.Delay(TimeSpan.FromSeconds(4f), cancellationToken: this.GetCancellationTokenOnDestroy());
                _catViewModel.CatState = CatState.SearchTarget;
            }
            else if (_catViewModel.CatState == CatState.TargetMissing)
            {
                // TODO(안우재/08.10) : Missing애니메이션 출력 구현, 애니메이션 후 상태 SearchTarget으로 변경
                // 지금은 타겟을 못찾아서 두리번 거리는 애니메이션 2초로 가정하여 구현
                await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: this.GetCancellationTokenOnDestroy());
                _catViewModel.CatState = CatState.SearchTarget;
            }
            else if (_catViewModel.CatState == CatState.SearchTarget)
            {
                // TODO(안우재/08.10) : 가까운 Spawner 오브젝트를 찾고, _targetTransform에 위치 설정
                SearchDespawnPoint();
            }
        }
        catch(OperationCanceledException) 
        { 

        }
    }

    public void SearchDespawnPoint()
    {
        if (_targetObject != null)
        {
            if(_targetObject.GetComponent<CatSpawner>() == null)
                this.gameObject.transform.position = _targetObject.GetComponent<Building>().GetEntrancePoint().position;
        }

        if (GameManager.Instance.CatManager.CatSpanweList.Count == 0)
        {
            // 고양이가 필드에 있지만 Despawn될 Spawn 지역이 없는 상태 현재는 그냥 바로 Despawn
            GameManager.Instance.CatManager.DespawnCat(this.gameObject);
            return;
        }

        CatSpawner targetSpawner = null;
        float nearDistance = float.MaxValue;
        for(int i = 0; i < GameManager.Instance.CatManager.CatSpanweList.Count; i++)
        {
            CatSpawner spawner = GameManager.Instance.CatManager.CatSpanweList[i];

            if (spawner == null || !spawner.gameObject.activeInHierarchy)
                continue;

            float distance = Mathf.Abs(
                spawner.transform.position.x - transform.position.x
            );

            if (distance < nearDistance)
            {
                nearDistance = distance;
                targetSpawner = spawner;
            }
        }
        _targetObject = targetSpawner.gameObject;
        SettingTargetPosition();
        _catViewModel.CatState = CatState.MoveToTarget;
    }
}
