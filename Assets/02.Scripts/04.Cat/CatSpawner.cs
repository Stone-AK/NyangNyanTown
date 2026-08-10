using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    // TODO(안우재/08.06)
    // 현재는 건물 짓기 기능이 없어 Unity생애 주기 부분에 Spanwer관리 리스트 추가,제외
    // 설정함. 추후 건물 짓기 기능이 추가되면 해당 Spanwer에 상속 시키고 지을 때
    // 리스트 추가 제외 실행되도록 할 예정

    private void OnEnable()
    {
        CreateCatSpawner();
    }

    private void OnDisable()
    {
        RemoveCatSpawner();
    }

    private void Start()
    {
        AutoSpawnCatINWhile().Forget();
    }

    private void CreateCatSpawner()
    {
        if(CatManager.Instance == null)
            return;

        CatManager.Instance.CatSpanweList.Add(this);
    }

    private void RemoveCatSpawner()
    {
        if (CatManager.Instance == null)
            return;

        CatManager.Instance.CatSpanweList.Remove(this);
    }

    private async UniTask AutoSpawnCatINWhile()
    {
        while(true)
        {
            double delaySeconds = GameUtil.Random.NextDouble() + 1.0;

            await UniTask.Delay(
                TimeSpan.FromSeconds(delaySeconds),
                cancellationToken: this.GetCancellationTokenOnDestroy()
            );

            if (CatManager.Instance == null)
                return;

            if (CatManager.Instance.IsCatSpawnAvailable() == false)
                continue;

            CatView cat = CatManager.Instance.GetCatFromPool();
            if (cat == null)
                continue;

            cat.gameObject.transform.position = this.transform.position;
            cat.gameObject.SetActive(true);
        }
    }
}
