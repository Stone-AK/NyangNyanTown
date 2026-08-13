using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
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
        // TODO(안우재/08.09) : 테스트 초기화. 나중에 건물 짓기에서 건물 오브젝트 매니저 생길 시 
        // 오브젝트 매니저에서 건물리스트 가져와서 처리 해야함(완전 Test코드)
        // 처음에는 건물(Building)을 목표, Building에서 Action 후 에는 Spawner로 위치 변경 필요
        //=====================================================
        GameObject[] objects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        List<GameObject> testPositions = new();

        foreach (GameObject obj in objects)
        {
            if (obj.name.Contains("BuildingTest"))
            {
                testPositions.Add(obj);
            }
        }

        if (testPositions.Count == 0)
            return;

        int randomIndex = GameUtil.Random.Next(0, testPositions.Count);
        _targetObject = testPositions[randomIndex];
        //=====================================================

        if (_targetObject == null)
            return;

        SettingTargetPosition(_targetObject);

        Vector3 direction = (_targetObject.transform.position - transform.position).normalized;
        direction.y = 0f;
        direction.z = 0f;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void SettingTargetPosition(GameObject targetObject)
    {
        if (targetObject == null)
        {
            Debug.Log("목표 오브젝트를 찾을 수 없습니다.");
            return;
        }

        if (targetObject.TryGetComponent<Building>(out Building buildingObject))
        {
            _targetTransform = buildingObject.GetEntrancePoint().transform.position;
            _targetTransform.y = 0f;
            _targetTransform.z = 0f;
        }
        else if(targetObject.TryGetComponent<CatSpawner>(out CatSpawner spawner))
        {
            _targetTransform = spawner.transform.position;
            _targetTransform.y = 0f;
            _targetTransform.z = 0f;
        }
    }

    private void CheckCatArriveTarget()
    {
        if (_targetObject == null)
            return;

        float remainingX = _targetTransform.x - transform.position.x;

        if (remainingX * transform.forward.x <= 0f)
        {
            if(!CheckTargetExistenceORChanged())
            {
                // 목표지점이 옮겨지거나 철거된 상태
                _catViewModel.CatState = CatState.TargetMissing;
                return;
            }

            if (_targetObject.TryGetComponent<Building>(out Building buildingObject))
            {
                if(transform.position.x == buildingObject.GetEntrancePoint().transform.position.x)
                {
                    ArriveBuildingEntrance(buildingObject);
                }
            }
            else if(_targetObject.GetComponent<CatSpawner>())
            {
                if(transform.position.x == _targetObject.transform.position.x)
                {
                    ArriveSpanwPositionAndEscape();
                }
            }

            return;
        }
    }

    private bool CheckTargetExistenceORChanged()
    {
        if(_targetObject == null || _targetObject.activeInHierarchy)
            return false;

        if(_targetObject.transform.position.x == _targetTransform.x)
            return false;

        return true;
    }

    private void ArriveBuildingEntrance(Building building)
    {
        if(building == null)
            return;

        if(building.GetAvailableCatPointCount() == 0)
        {
            Debug.Log("해당 건물은 빈 자리가 없습니다.");
            // 대기 모션 출력 관련 메서드
            return;
        }

        this.gameObject.transform.position = building.GetAvailableCatPoint().position;
        _catViewModel.CatState = CatState.InBuildingAction;
    }

    private void ArriveSpanwPositionAndEscape()
    {
        // TODO(안우재/08.09) : 가까운 Spawn지역에 도착 시 탈출하는 모션 출력 필요.
        GameManager.Instance.ObjectManager.Despawn(this.gameObject);
    }

    private async void ActionFromStatus()
    {
        if(_catViewModel == null)
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

    public void SearchDespawnPoint()
    {
        if (_targetObject != null)
        {
            this.gameObject.transform.position = _targetTransform;

        }

        if (GameManager.Instance.CatManager.CatSpanweList.Count == 0)
        {
            // 고양이가 필드에 있지만 Despawn될 Spawn 지역이 없는 상태 현재는 그냥 바로 Despawn
            GameManager.Instance.CatManager.DespawnCat(this.gameObject);
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
            _catViewModel.CatState = CatState.MoveToTarget;
        }
    }
}
