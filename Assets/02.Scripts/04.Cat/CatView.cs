using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CatView : MonoBehaviour
{
    private CatViewModel _catViewModel;

    private Transform _targetPosition;

    private void FixedUpdate()
    {
        MoveCat();
    }

    private void MoveCat()
    {
        if(_catViewModel == null)
            return;

        // TODO(안우재/08.06) : 목표 방향으로 갈 수 있도록 rotation 부분 추가 필요
        transform.Translate(Vector3.forward * _catViewModel.CatSpeed * Time.deltaTime);
    }

    public void InitCatView(CatViewModel catViewModel)
    {
        _catViewModel = catViewModel;

        // TODO(안우재/08.09) : 테스트 초기화. 나중에 건물 짓기에서 건물 오브젝트 매니저 생길 시 
        // 오브젝트 매니저에서 건물리스트 가져와서 처리 해야함(완전 Test코드)

        GameObject[] objects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        List<Transform> testPositions = new();

        foreach (GameObject obj in objects)
        {
            if (obj.name.Contains("BuildingTest"))
            {
                testPositions.Add(obj.transform);
            }
        }

        if (testPositions.Count == 0)
            return;

        int randomIndex = GameUtil.Random.Next(0, testPositions.Count);
        _targetPosition = testPositions[randomIndex];

        //

        if (_targetPosition == null)
            return;

        Vector3 direction = (_targetPosition.position - transform.position).normalized;
        direction.y = 0f;
        direction.z = 0f;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
