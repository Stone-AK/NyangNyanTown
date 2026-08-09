using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CatView : MonoBehaviour
{
    private CatViewModel _catViewModel;
    private GameObject _targetObject;

    private void FixedUpdate()
    {
        // TODO(안우재/08.09) : 추후 CatState에 따라 이동, 행동, Spawner로 이동 구현
        if (_catViewModel.CatState == CatState.TargetMove || _catViewModel.CatState == CatState.EscapeMove)
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
                    if (_catViewModel.CatState == CatState.InBuildingAction)
                    {
                        // TODO(안우재/08.09) : slot에 이동 후 애니메이션 출력 구현 필요. 아래는 Test 모션
                        transform.Rotate(0f, 90f * Time.deltaTime, 0f);
                    }
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

        Vector3 direction = (_targetObject.transform.position - transform.position).normalized;
        direction.y = 0f;
        direction.z = 0f;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void CheckCatArriveTarget()
    {
        if (_targetObject == null)
            return;

        // TODO(안우재/08.09) : 나중에 entrance위치 초기화 부분 따로 만들어 주기
        if (_targetObject.TryGetComponent<BuildingView>(out BuildingView buildingView))
        {
            Transform targetTransform = buildingView.GetEntrance().transform;

            float remainingX = targetTransform.position.x - transform.position.x;

            if (remainingX * transform.forward.x <= 0f)
            {
                ArriveBuildingEntrance();
                return;
            }
        }
    }

    private void ArriveBuildingEntrance()
    {
        if(_targetObject.TryGetComponent<BuildingView>(out BuildingView buildingView))
        {
            BuildingInsideSlotView emptySlot = buildingView.GetEmptySlot();
            if (emptySlot == null)
            {
                Debug.Log("빈 Slot이 없습니다.");
                return;
            }

            this.gameObject.transform.position = emptySlot.gameObject.transform.position;
            _catViewModel.CatState = CatState.InBuildingAction;
        }
    }

    private void ArriveSpanwPositionAndEscape()
    {
        // TODO(안우재/08.09) : 가까운 Spawn지역에 도착 시 탈출하는 모션 출력 필요.

    }
}
